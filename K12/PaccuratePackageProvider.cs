using CMS.Ecommerce;
using CMS.Helpers;
using System.Linq;
using CMS.Core;
using CMS.SiteProvider;
using CMS.Membership;

namespace PaccurateShipping
{
    public static class PaccuratePackageProvider
    {
        public static PaccurateDelivery GetPackage(Delivery delivery)
        {
            PaccurateDelivery PackDelivery = new PaccurateDelivery(delivery);
            PackDelivery.BuildRequest();

            var RequestMD5 = PackDelivery.GetShippingHash();

            PackDelivery = CacheHelper.Cache(x => {
                if (x.Cached)
                {
                    x.CacheDependency = CacheHelper.GetCacheDependency(new string[3] {
                        "paccurateshipping.box|all",
                        "paccurateshipping.boxshippingoption|all",
                        "paccurateshipping.boxcolor|all"
                    });
                }
                PackDelivery.Process();
                PackDelivery.AddUpdateOrderPackageInfo();
                return PackDelivery;
            }, new CacheSettings(30.0, "maindelivery", RequestMD5));

            return PackDelivery;
        }

        private static void AddUpdateOrderPackageInfo(this PaccurateDelivery Paccurate)
        {
            if (Paccurate.Response != null)
            {
                var boxes = Paccurate.Response.Boxes.ToArray();
                var images = Paccurate.Response.Images.ToArray();
                string imageString = "";
                for (var i = 0; i < boxes.Length; i++)
                {
                    imageString += images[i].Data;
                    var box = boxes[i].Box;
                    imageString += $"<div class='p-box'><div class='box-name'>{box.Name + " Total Weight: " + box.WeightUsed?.ToString()}:<div><div class='box-message'>{box.Items?.Select(x => (string.IsNullOrEmpty(x.Item.Name) ? "" : x.Item.Name + " | ") + (x.Item.Weight > 0 ? "Item Weight: " + x.Item.Weight.ToString() + " | " : "") + x.Item.Message).Join("<br/>") }</div></div>";
                }

                var cart = Service.Resolve<ICurrentShoppingCartService>().GetCurrentShoppingCart(MembershipContext.AuthenticatedUser, SiteContext.CurrentSiteID);

                cart.ShoppingCartCustomData.SetValue(PaccurateShippingHelper.PaccurateImageField, imageString);

                ShoppingCartInfoProvider.SetShoppingCartInfo(cart);
            }
        }
    }
}
