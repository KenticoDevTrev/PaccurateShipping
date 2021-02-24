using System;
using System.Data;

using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;

namespace PaccurateShipping
{
    /// <summary>
    /// Class providing <see cref="ShippingLayoutInfo"/> management.
    /// </summary>
    public partial class ShippingLayoutInfoProvider : AbstractInfoProvider<ShippingLayoutInfo, ShippingLayoutInfoProvider>
    {
        /// <summary>
        /// Creates an instance of <see cref="ShippingLayoutInfoProvider"/>.
        /// </summary>
        public ShippingLayoutInfoProvider()
            : base(ShippingLayoutInfo.TYPEINFO)
        {
        }


        /// <summary>
        /// Returns a query for all the <see cref="ShippingLayoutInfo"/> objects.
        /// </summary>
        public static ObjectQuery<ShippingLayoutInfo> GetShippingLayouts()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns <see cref="ShippingLayoutInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="ShippingLayoutInfo"/> ID.</param>
        public static ShippingLayoutInfo GetShippingLayoutInfo(int id)
        {
            return ProviderObject.GetInfoById(id);
        }


        /// <summary>
        /// Sets (updates or inserts) specified <see cref="ShippingLayoutInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="ShippingLayoutInfo"/> to be set.</param>
        public static void SetShippingLayoutInfo(ShippingLayoutInfo infoObj)
        {
            ProviderObject.SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified <see cref="ShippingLayoutInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="ShippingLayoutInfo"/> to be deleted.</param>
        public static void DeleteShippingLayoutInfo(ShippingLayoutInfo infoObj)
        {
            ProviderObject.DeleteInfo(infoObj);
        }


        /// <summary>
        /// Deletes <see cref="ShippingLayoutInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="ShippingLayoutInfo"/> ID.</param>
        public static void DeleteShippingLayoutInfo(int id)
        {
            ShippingLayoutInfo infoObj = GetShippingLayoutInfo(id);
            DeleteShippingLayoutInfo(infoObj);
        }
    }
}