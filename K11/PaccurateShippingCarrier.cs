using CMS.Ecommerce;
using System;
using System.Collections.Generic;
using PaccurateShipping;
using CMS;

[assembly: RegisterCustomClass("PaccurateShippingCarrier", typeof(PaccurateShippingCarrier))]

namespace PaccurateShipping
{
    public class PaccurateShippingCarrier : ICarrierProvider
    {
        public const string _PaccurateWeightBasedShippingOptionName = "CUSTOM_BOXES_PACCURATE";

        public string CarrierProviderName => "PACCURATE";

        public bool CanDeliver(Delivery delivery)
        {
            PaccurateDelivery PackDelivery = new PaccurateDelivery(delivery);
            return PackDelivery.Response.Leftovers.Count > 0;
        }

        public Guid GetConfigurationUIElementGUID()
        {
            return Guid.Empty;
        }

        public decimal GetPrice(Delivery delivery, string currencyCode)
        {
            PaccurateDelivery PackDelivery = new PaccurateDelivery(delivery);           
            return CurrencyConverter.Convert(PackDelivery.Response.TotalCost ?? 0, CurrencyInfoProvider.GetMainCurrencyCode(delivery.ShippingOption.ShippingOptionSiteID), currencyCode, delivery.ShippingOption.ShippingOptionSiteID);
        }

        public Guid GetServiceConfigurationUIElementGUID(string serviceName)
        {
            return Guid.Empty;
        }

        public List<KeyValuePair<string, string>> GetServices()
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("CUSTOM_BOXES_PACCURATE", "{$paccurate.customboxes$}")
            };
        }
    }
}
