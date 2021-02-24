using System;
using System.Data;

using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;

namespace PaccurateShipping
{
    /// <summary>
    /// Class providing <see cref="BoxShippingOptionInfo"/> management.
    /// </summary>
    public partial class BoxShippingOptionInfoProvider : AbstractInfoProvider<BoxShippingOptionInfo, BoxShippingOptionInfoProvider>
    {
        /// <summary>
        /// Creates an instance of <see cref="BoxShippingOptionInfoProvider"/>.
        /// </summary>
        public BoxShippingOptionInfoProvider()
            : base(BoxShippingOptionInfo.TYPEINFO)
        {
        }


        /// <summary>
        /// Returns a query for all the <see cref="BoxShippingOptionInfo"/> objects.
        /// </summary>
        public static ObjectQuery<BoxShippingOptionInfo> GetBoxShippingOptions()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns <see cref="BoxShippingOptionInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="BoxShippingOptionInfo"/> ID.</param>
        public static BoxShippingOptionInfo GetBoxShippingOptionInfo(int id)
        {
            return ProviderObject.GetInfoById(id);
        }


        /// <summary>
        /// Sets (updates or inserts) specified <see cref="BoxShippingOptionInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="BoxShippingOptionInfo"/> to be set.</param>
        public static void SetBoxShippingOptionInfo(BoxShippingOptionInfo infoObj)
        {
            ProviderObject.SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified <see cref="BoxShippingOptionInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="BoxShippingOptionInfo"/> to be deleted.</param>
        public static void DeleteBoxShippingOptionInfo(BoxShippingOptionInfo infoObj)
        {
            ProviderObject.DeleteInfo(infoObj);
        }


        /// <summary>
        /// Deletes <see cref="BoxShippingOptionInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="BoxShippingOptionInfo"/> ID.</param>
        public static void DeleteBoxShippingOptionInfo(int id)
        {
            BoxShippingOptionInfo infoObj = GetBoxShippingOptionInfo(id);
            DeleteBoxShippingOptionInfo(infoObj);
        }
    }
}