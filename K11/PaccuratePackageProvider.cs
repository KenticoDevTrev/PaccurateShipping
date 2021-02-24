using CMS.Ecommerce;
using CMS.Helpers;
using System.Linq;
using System.Security.Cryptography;

namespace PaccurateShipping
{
    public static class PaccuratePackageProvider
    {
        public static PaccurateDelivery GetPackage(Delivery delivery)
        {
            var PackDelivery = new PaccurateDelivery(delivery);
            PackDelivery.BuildRequest();

            // cache and return the response recieved from Paccurate
            string RequestMD5 = MD5.Create(PackDelivery.Request.ToJson(false)).ToString();
            return CacheHelper.Cache(x => {
                if (x.Cached)
                {
                    x.CacheDependency = CacheHelper.GetCacheDependency(new string[] {
                        "paccurateshipping.box|all",
                        "paccurateshipping.boxshippingoption|all",
                        "paccurateshipping.boxcolor|all",
                        "ecommerce.shippingoption|byid|"+delivery.ShippingOption.ShippingOptionID,
                        "ecommerce.carrier|byid|"+delivery.ShippingOption.ShippingOptionCarrierID,
                        "Ecommerce.Address|byid|"+delivery.DeliveryAddress.AddressID
                    });
                }
                PackDelivery.Process();
                AddUpdateOrderPackageInfo(PackDelivery);
                return PackDelivery;
            }, new CacheSettings(30, ECommerceContext.CurrentShoppingCart.ShoppingCartID, RequestMD5));
        }

        private static void AddUpdateOrderPackageInfo(PaccurateDelivery Paccurate)
        {
            if (Paccurate.Response != null)
            {
                var boxes = Paccurate.Response.Boxes.ToArray();
                var images = Paccurate.Response.Images.ToArray();
                string imageString = "";
                for(var i = 0;i < boxes.Length; i++)
                {
                    imageString += images[i].Data;
                    var box = boxes[i].Box;
                    imageString += $"<div class='p-box'><div class='box-name'>{box.Name + " Total Weight: " + box.WeightUsed?.ToString()}:<div><div class='box-message'>{box.Items?.Select(x => (string.IsNullOrEmpty(x.Item.Name) ? "" : x.Item.Name + " | ") + (x.Item.Weight > 0? "Item Weight: " + x.Item.Weight.ToString() + " | " : "") + x.Item.Message).Join("<br/>") }</div></div>";
                }
                var cart = ECommerceContext.CurrentShoppingCart;

                cart.ShoppingCartCustomData.SetValue(PaccurateShippingHelper.PaccurateImageField, imageString);

                ShoppingCartInfoProvider.SetShoppingCartInfo(cart);
            }
        }
    }
}
