using CMS.Base;
using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.SiteProvider;
using Paccurate;
using Paccurate.Enum;
using Paccurate.Models;
using PaccurateShipping.Events;
using PaccurateShipping.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PaccurateShipping
{
    public class PaccurateDelivery
    {
        /// <summary>
        /// The Kentico Delivery Object
        /// </summary>
        public Delivery DeliveryObj { get; set; }

        /// <summary>
        /// The Paccurate Request, set on Process method, can be adjusted through PaccurateShippingEvents.BuildRequest
        /// </summary>
        public PackRequest Request { get; set; }

        /// <summary>
        /// The Paccurate Pack response
        /// </summary>
        public PackResponse Response { get; set; }

        /// <summary>
        /// If the response errored in the request
        /// </summary>
        public bool ResponseErrored { get; set; }

        /// <summary>
        /// If ResponseErrored is true, this is the response error information
        /// </summary>
        public Error ResponseError { get; set; }

        public PaccurateDelivery()
        {

        }

        /// <summary>
        /// Builds a new Kentico Paccurate Delivery object
        /// </summary>
        /// <param name="delivery"></param>
        public PaccurateDelivery(Delivery delivery)
        {
            DeliveryObj = delivery;
        }

        public void BuildRequest()
        {
            try
            {
                var NewRequest = GetBaseRequest();
                var RequestBuilderArgs = new BuildRequestEventArgs()
                {
                    KenticoDelivery = DeliveryObj,
                    PaccurateRequest = NewRequest
                };
                using (var PaccurateShippingBuildRequestTaskHandler = PaccurateShippingEvents.BuildRequest.StartEvent(RequestBuilderArgs))
                {
                    // Convert Delivery object into a PackRequest, then get the Response
                    if (DeliveryObj == null)
                    {
                        throw new NullReferenceException("DeliveryObj cannot be null, please provide or set before processing");
                    }

                    // Add Boxes if not set already
                    if (
                        (RequestBuilderArgs.PaccurateRequest.BoxTypeSets == null || RequestBuilderArgs.PaccurateRequest.BoxTypeSets.Count == 0) &&
                        (RequestBuilderArgs.PaccurateRequest.BoxTypes == null || RequestBuilderArgs.PaccurateRequest.BoxTypes.Count == 0) &&
                        (RequestBuilderArgs.PaccurateRequest.Boxes == null || RequestBuilderArgs.PaccurateRequest.Boxes.Count == 0)
                        )
                    {
                        AddBoxes(RequestBuilderArgs.PaccurateRequest, RequestBuilderArgs.KenticoDelivery);
                    }

                    // Create the items
                    if (RequestBuilderArgs.PaccurateRequest.ItemSets == null || RequestBuilderArgs.PaccurateRequest.ItemSets.Count == 0)
                    {
                        AddItems(RequestBuilderArgs.PaccurateRequest, RequestBuilderArgs.KenticoDelivery);
                    }

                    PaccurateShippingBuildRequestTaskHandler.FinishEvent();
                    Request = RequestBuilderArgs.PaccurateRequest;
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("Paccurate Packaging", "BUILDDELIVERY", ex);
            }
        }

        /// <summary>
        /// Processes the PaccurateDelivery, fills in the Response, or ResponseError 
        /// </summary>
        public PackResponse Process()
        {
            try
            {
                // Now send the request
                string RequestMD5 = MD5.Create(Request.ToJson(false)).ToString();
                var ResponseHandler = CacheHelper.Cache(x =>
                {
                    if (x.Cached)
                    {
                        x.CacheDependency = CacheHelper.GetCacheDependency(new string[] {
                        "paccurateshipping.box|all",
                        "paccurateshipping.boxshippingoption|all",
                        "paccurateshipping.boxcolor|all"
                    });
                    }
                    return new PackRequestHandler(Request).GetResponse();
                }, new CacheSettings(30, ECommerceContext.CurrentShoppingCart.ShoppingCartID, RequestMD5));

                ResponseErrored = ResponseHandler.ResponseErrored;
                if (ResponseErrored)
                {
                    ResponseError = ResponseHandler.ResponseError;
                    EventLogProvider.LogEvent("W", "Paccurate Packaging", "RESPONSEERROR", eventDescription: $"Error {ResponseError.Code}: {ResponseError.Message}.  For Request {Request.ToString() }");
                }
                else
                {
                    Response = ResponseHandler.Response;

                    #region "leftover handling"

                    // Handle "Left Overs", if an item is found to be a leftover, see if it can ship by itself and adjust Request and re-send
                    if (Request.Boxes == null)
                    {
                        Request.Boxes = new List<BoxContainer>();
                    }
                    if (ResponseHandler.Response.Leftovers.Count > 0)
                    {
                        List<string> UnshippableItems = new List<string>();
                        foreach (var LeftoverItem in ResponseHandler.Response.Leftovers)
                        {
                            List<BoxContainer> LeftoverItemBoxes = GetItemBoxes(LeftoverItem.Item, DeliveryObj.ShippingOption.ShippingOptionSiteID);
                            if (LeftoverItemBoxes.Count > 0)
                            {
                                // Remove the item from the ItemSets and add to predefined boxes
                                Request.Boxes.AddRange(LeftoverItemBoxes);
                                Request.ItemSets.RemoveAll(x => x.Name.Equals(LeftoverItem.Item.Name));
                            }
                            else
                            {
                                // Could not create boxes, add unshippable item for error alerting.
                                UnshippableItems.Add(LeftoverItem.Item.ToString());
                            }
                        }

                        if (UnshippableItems.Count > 0)
                        {
                            Response = null;
                            EventLogProvider.LogEvent("I", "PACCURATESHIPPING", "ItemsUnableToShip", eventDescription: $"The following items were not able to be configured for shipping and may require configuration: {string.Join("\n\n", UnshippableItems)}");
                        }
                        else
                        {
                            // Re-run request now that everything is boxed up.
                            string SecondRequestMD5 = MD5.Create(Request.ToJson(false)).ToString();
                            var SecondaryResponseHandler = CacheHelper.Cache(x =>
                            {
                                if (x.Cached)
                                {
                                    x.CacheDependency = CacheHelper.GetCacheDependency(new string[] {
                                        "paccurateshipping.box|all",
                                        "paccurateshipping.boxshippingoption|all",
                                        "paccurateshipping.boxcolor|all",
                                    });
                                }
                                return new PackRequestHandler(Request).GetResponse();
                            }, new CacheSettings(30, ECommerceContext.CurrentShoppingCart.ShoppingCartID, SecondRequestMD5));

                            var SecondaryResponseErrored = ResponseHandler.ResponseErrored;
                            if (SecondaryResponseErrored)
                            {
                                var SecondaryResponseError = SecondaryResponseHandler.ResponseError;
                                EventLogProvider.LogEvent("W", "Paccurate Packaging", "SECONDRESPONSEERROR", eventDescription: $"Error {SecondaryResponseError.Code}: {SecondaryResponseError.Message}.  For Request secondary (preboxed leftovers) {Request.ToString() }");
                            }
                            else
                            {
                                Response = SecondaryResponseHandler.Response;
                            }
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("Paccurate Packaging", "PROCCESSDELIVERY", ex);
            }
            return Response;
        }

        /// <summary>
        /// Takes the leftover Item and creates Box(es) out of it.  Item must be shippable as is.
        /// </summary>
        /// <param name="item">The leftover Item</param>
        /// <param name="SiteID">The Order's Site ID</param>
        /// <returns>A list of pre-packed boxes for the item</returns>
        private List<BoxContainer> GetItemBoxes(Item item, int SiteID)
        {
            List<BoxContainer> ItemBoxes = new List<BoxContainer>();
            SiteInfoIdentifier SiteIdentifier = new SiteInfoIdentifier(SiteID);
            BoxableSKUItem SkuSettings = new BoxableSKUItem()
            {
                IsShippableAsIs = ValidationHelper.GetBoolean(SettingsKeyInfoProvider.GetBoolValue("ItemShippableAsIs", SiteIdentifier), false),
                UseSpecifiedRates = ValidationHelper.GetBoolean(SettingsKeyInfoProvider.GetBoolValue("ShippableAsIsDefaultUseSpecifiedRates", SiteIdentifier), false),
                WeightRates = ValidationHelper.GetString(SettingsKeyInfoProvider.GetValue("ShippableAsIsDefaultWeightRates", SiteIdentifier), "0;0"),
                CostPerWeightUnit = ValidationHelper.GetDecimal(SettingsKeyInfoProvider.GetDecimalValue("ShippableAsIsDefaultCostPerWeightUnit", SiteIdentifier), 0)
            };

            SKUInfo Sku = SKUInfoProvider.GetSKUInfo(item.RefId ?? 0);
            // Configuration is on the parent.
            if (Sku.SKUParentSKUID > 0)
            {
                Sku = SKUInfoProvider.GetSKUInfo(Sku.SKUParentSKUID);
            }
            // See if Sku has Box settings
            if (Sku != null && Sku.SKUCustomData.GetValue(BoxableSKUItem._BoxableSKUItemSettingKey) != null)
            {
                var SkuActualSettings = BoxableSKUItem.XmlToObject(ValidationHelper.GetString(Sku.SKUCustomData.GetValue(BoxableSKUItem._BoxableSKUItemSettingKey), ""));
                if (SkuActualSettings != null)
                {
                    switch (SkuActualSettings.IsShippableSeparate)
                    {
                        case BoxableSKUItem.ShippableSeparateType.Yes:
                            SkuSettings.IsShippableAsIs = SkuActualSettings.IsShippableAsIs;
                            SkuSettings.UseSpecifiedRates = SkuActualSettings.UseSpecifiedRates;
                            if (!string.IsNullOrWhiteSpace(SkuActualSettings.WeightRates))
                            {
                                SkuSettings.WeightRates = SkuActualSettings.WeightRates;
                            }
                            if (SkuActualSettings.CostPerWeightUnit.HasValue)
                            {
                                SkuSettings.CostPerWeightUnit = SkuActualSettings.CostPerWeightUnit.Value;
                            }
                            SkuSettings = SkuActualSettings;
                            break;
                        case BoxableSKUItem.ShippableSeparateType.No:
                            return new List<BoxContainer>();
                        case BoxableSKUItem.ShippableSeparateType.UseSettingsDefault:
                            // Keep SkuSettings as is.
                            break;
                    }
                }
            }

            // Get defaults
            if (SkuSettings.IsShippableAsIs)
            {
                decimal ItemWeight = (item.Weight ?? 0);
                Box box = new Box(ItemWeight, item.Dimensions)
                {
                    Name = item.Name,
                    RefId = item.RefId,
                    CenterOfMass = item.CenterOfMass,
                    BoxType = new BoxType(ItemWeight, item.Dimensions)
                    {
                        Name = "itempackaging_" + item.Name,
                        WeightTare = 0,
                    },
                    Items = new List<ItemContainer>()
                    {
                        new ItemContainer()
                        {
                            Item = item
                        }
                    }
                };

                // Set pricing
                if (SkuSettings.UseSpecifiedRates)
                {
                    box.RateTable = GetRateTableFromConfig(SkuSettings.WeightRates);
                }
                else
                {
                    box.RateTable = new RateTable()
                    {
                        BasePrice = 0,
                        PriceIncreaseRate = SkuSettings.CostPerWeightUnit ?? 0
                    };
                }
                ItemBoxes.Add(new BoxContainer() { Box = box });
            }

            return ItemBoxes;
        }

        /// <summary>
        /// Converts the Response into an XML String which can be stored on the order
        /// </summary>
        /// <returns>Converts the Response into a Serialized Object</returns>
        public string GetSerializedResponse()
        {
            if (this.Response != null)
            {
                string SerializedXml = "";
                StringBuilder builder = new StringBuilder(SerializedXml);
                XmlSerializer serializer = new XmlSerializer(typeof(PackResponse));
                using (TextWriter writer = new StringWriter(builder))
                {
                    serializer.Serialize(writer, Response);
                }
                return SerializedXml;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Deserializes the Response XML into the PackResponse object
        /// </summary>
        /// <param name="ResponseXml"></param>
        /// <returns></returns>
        public static PackResponse DeserializedPackResponse(string ResponseXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PackResponse));
            PackResponse Response = null;
            using (TextReader reader = new StringReader(ResponseXml))
            {
                Response = (PackResponse)serializer.Deserialize(reader);
            }
            return Response;
        }

        /// <summary>
        /// Adds Items from the Delivery to the Pack Request
        /// </summary>
        /// <param name="PRequest">The Request</param>
        /// <param name="KDelivery">The Delivery</param>
        private static void AddItems(PackRequest PRequest, Delivery KDelivery)
        {
            List<string> BoxCssColors = GetBoxColors();
            int ItemIndex = 0;
            PRequest.ItemSets = new List<ItemSet>();
            PRequest.Boxes = new List<BoxContainer>();


            // Add Items
            foreach (var Item in KDelivery.Items)
            {
                var GetItemPackagesArgs = new GetItemPackagesEventArgs()
                {
                    KenticoDelivery = KDelivery,
                    Item = Item,
                    DefaultItemColor = BoxCssColors[ItemIndex % BoxCssColors.Count]
                };
                using (var PaccurateGetItemRequestTaskHandler = PaccurateShippingEvents.GetItemPackagesRequest.StartEvent(GetItemPackagesArgs))
                {
                    // If ItemSets and Boxes are not defined, then define as normal.
                    if ((GetItemPackagesArgs.ItemSets == null || GetItemPackagesArgs.ItemSets.Count == 0) &&
                        (GetItemPackagesArgs.Boxes == null || GetItemPackagesArgs.Boxes.Count == 0))
                    {
                        GetItemPackagesArgs.ItemSets = new List<ItemSet>();
                        // Round up to nearest whole integar
                        for (int i = 1; i <= Convert.ToInt32(Math.Ceiling(Item.Amount)); i++)
                        {
                            ItemSet PaccurateItem = new ItemSet(Convert.ToDecimal(Item.Product.SKUWeight), new Point(Convert.ToDecimal(Item.Product.SKUHeight), Convert.ToDecimal(Item.Product.SKUDepth), Convert.ToDecimal(Item.Product.SKUWidth)))
                            {
                                Color = GetItemPackagesArgs.DefaultItemColor,
                                Name = $"{Item.Product.SKUName} [{i}]",
                                RefId = Item.Product.SKUID
                            };
                            GetItemPackagesArgs.ItemSets.Add(PaccurateItem);
                        }
                    }

                    // Finish the event
                    PaccurateGetItemRequestTaskHandler.FinishEvent();

                    if (GetItemPackagesArgs.ItemSets != null)
                    {
                        PRequest.ItemSets.AddRange(GetItemPackagesArgs.ItemSets);
                    }
                    if (GetItemPackagesArgs.Boxes != null)
                    {
                        PRequest.Boxes.AddRange(GetItemPackagesArgs.Boxes);
                    }
                }
                ItemIndex++;
            }
        }

        /// <summary>
        /// Adds Boxes to the Paccurate Request, if the shipping option is the Fedex or USPS flat rate options, then just sets the Box Type Sets.
        /// </summary>
        /// <param name="PRequest">The Request</param>
        /// <param name="KDelivery">The Delivery</param>
        private static void AddBoxes(PackRequest PRequest, Delivery KDelivery)
        {
            var SiteIdentifier = new SiteInfoIdentifier(SiteContext.CurrentSiteID);
            CarrierInfo Carrier = CarrierInfoProvider.GetCarrierInfo(KDelivery.ShippingOption.ShippingOptionCarrierID);

            // Get Shipping Option Names for Fedex Flat Rate and USPS Flat Rate
            string FedexFlatRateShippingOptionName = DataHelper.GetNotEmpty(SettingsKeyInfoProvider.GetValue("PaccurateFedexFlatRateShippingOption", SiteContext.CurrentSiteID), "");
            string USPSFlatRateShippingOptionName = DataHelper.GetNotEmpty(SettingsKeyInfoProvider.GetValue("PaccurateUSPSFlatRateShippingOption", SiteContext.CurrentSiteID), "");

            if (KDelivery.ShippingOption.ShippingOptionName.Equals(FedexFlatRateShippingOptionName, StringComparison.InvariantCultureIgnoreCase))
            {
                PRequest.BoxTypeSets = new List<BoxTypeSetEnum?>() { BoxTypeSetEnum.Fedex };
            }
            else if (KDelivery.ShippingOption.ShippingOptionName.Equals(USPSFlatRateShippingOptionName, StringComparison.InvariantCultureIgnoreCase))
            {
                PRequest.BoxTypeSets = new List<BoxTypeSetEnum?>() { BoxTypeSetEnum.USPS };
            }
            else
            {
                // Find boxes for the current Shipping option
                PRequest.BoxTypes = new List<BoxType>();
                foreach (BoxInfo Box in BoxInfoProvider.GetBoxes()
                    .Source(x => x.Join<BoxShippingOptionInfo>("BoxID", "BoxShippingOptionBoxID"))
                    .WhereEquals("BoxShippingOptionShippingOptionID", KDelivery.ShippingOption.ShippingOptionID)
                    .WhereTrue("BoxShippingOptionEnabled")
                    .WhereTrue("BoxEnabled"))
                {
                    BoxType PaccurateBox = new BoxType(Box.BoxMaxWeight, new Point(Box.BoxDimensionX, Box.BoxDimensionY, Box.BoxDimensionZ))
                    {
                        Name = Box.BoxDisplayName,
                        Price = Box.BoxPrice,
                        RefId = Box.BoxID,
                        WeightTare = Box.BoxWeight
                    };
                    BoxShippingOptionInfo BoxShippingOption = BoxShippingOptionInfoProvider.GetBoxShippingOptions()
                        .WhereEquals("BoxShippingOptionBoxID", Box.BoxID)
                        .WhereEquals("BoxShippingOptionShippingOptionID", KDelivery.ShippingOption.ShippingOptionID)
                        .WhereTrue("BoxShippingOptionEnabled")
                        .FirstOrDefault();

                    if (BoxShippingOption.BoxShippingOptionRateUseSpecifiedRates)
                    {
                        PaccurateBox.RateTable = GetRateTableFromConfig(BoxShippingOption.BoxShippingOptionRateWeights);
                    }
                    else
                    {
                        PaccurateBox.RateTable = new RateTable()
                        {
                            BasePrice = BoxShippingOption.BoxShippingOptionBasePrice,
                            PriceIncreaseRate = BoxShippingOption.BoxShippingOptionPriceIncreaseRate
                        };
                    }
                    PaccurateBox.RateTable.Zone = (BoxShippingOption.GetValue("BoxShippingOptionZone") != null ? BoxShippingOption.BoxShippingOptionZone : null);
                    PaccurateBox.RateTable.BasePrice = (BoxShippingOption.GetValue("BoxShippingOptionDimFactor") != null ? (decimal?)BoxShippingOption.BoxShippingOptionDimFactor : null);

                    // Add the box
                    PRequest.BoxTypes.Add(PaccurateBox);
                }

            }
        }

        private static RateTable GetRateTableFromConfig(string Configuration)
        {
            var WeightToRateList = Configuration.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            Dictionary<decimal?, decimal?> WeightToRate = new Dictionary<decimal?, decimal?>();
            foreach (string WeightToRateItem in WeightToRateList.Where(x => x.Contains(";")))
            {
                decimal? Weight = ValidationHelper.GetDecimal(WeightToRateItem.Split(";".ToCharArray())[0], 0);
                decimal? Rate = ValidationHelper.GetDecimal(WeightToRateItem.Split(";".ToCharArray())[1], 0);
                if (!WeightToRate.ContainsKey(Weight))
                {
                    WeightToRate.Add(Weight, Rate);
                }
            }
            return new RateTable()
            {
                Rates = WeightToRate.Values.ToList(),
                Weights = WeightToRate.Keys.ToList(),
            };
        }

        /// <summary>
        /// Gets a list of Box Colors to use in order, the colors will cycle through for each item (if more items then colors, will loop back).  Blue is the default if none set.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetBoxColors()
        {
            return CacheHelper.Cache(cs =>
            {
                if (cs.Cached)
                {
                    cs.CacheDependency = CacheHelper.GetCacheDependency("paccurate.boxcolor|all");
                }
                List<string> List = BoxColorInfoProvider.GetBoxColors().OrderBy("BoxColorOrder").Select(x => x.BoxColorName.ToLower().Trim()).ToList();
                if (List.Count == 0)
                {
                    List.Add("blue");
                }
                return List;
            }, new CacheSettings(1440, "GetBoxColors"));
        }

        /// <summary>
        /// Gets the base request using the Settings Keys
        /// </summary>
        /// <returns></returns>
        private PackRequest GetBaseRequest()
        {
            var SiteIdentifier = new SiteInfoIdentifier(SiteContext.CurrentSiteID);
            var Request = new PackRequest(SettingsKeyInfoProvider.GetValue("PaccurateApiKey", SiteIdentifier));

            switch (SettingsKeyInfoProvider.GetValue("PaccurateShippingImageFormat", SiteIdentifier))
            {
                case "svg":
                    Request.ImageFormat = ImageFormatEnum.Svg;
                    break;
                case "png":
                    Request.ImageFormat = ImageFormatEnum.Png;
                    break;
            }

            int BoxTypeLookahead = ValidationHelper.GetInteger(SettingsKeyInfoProvider.GetIntValue("BoxTypeChoiceLookahead", SiteIdentifier), 0);
            if (BoxTypeLookahead > 0)
            {
                Request.BoxTypeChoiceLookahead = BoxTypeLookahead;
            }

            switch (SettingsKeyInfoProvider.GetValue("PaccurateShippingImageFormat", SiteIdentifier))
            {
                case "lowest-cost":
                    Request.BoxTypeChoiceGoal = BoxTypeChoiceGoalEnum.LowestCost;
                    break;
                case "most-items":
                    Request.BoxTypeChoiceGoal = BoxTypeChoiceGoalEnum.MostItems;
                    break;
            }

            Request.IncludeScripts = SettingsKeyInfoProvider.GetBoolValue("PaccurateIncludeScripts", SiteIdentifier, false);

            return Request;
        }


    }
}
