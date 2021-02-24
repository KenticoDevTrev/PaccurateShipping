using System;
using System.Data;

using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.SiteProvider;

namespace PaccurateShipping
{
    /// <summary>
    /// Class providing <see cref="BoxInfo"/> management.
    /// </summary>
    public partial class BoxInfoProvider : AbstractInfoProvider<BoxInfo, BoxInfoProvider>
    {
        /// <summary>
        /// Creates an instance of <see cref="BoxInfoProvider"/>.
        /// </summary>
        public BoxInfoProvider()
            : base(BoxInfo.TYPEINFO)
        {
        }


        /// <summary>
        /// Returns a query for all the <see cref="BoxInfo"/> objects.
        /// </summary>
        public static ObjectQuery<BoxInfo> GetBoxes()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns <see cref="BoxInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="BoxInfo"/> ID.</param>
        public static BoxInfo GetBoxInfo(int id)
        {
            return ProviderObject.GetInfoById(id);
        }


        /// <summary>
        /// Returns <see cref="BoxInfo"/> with specified name.
        /// </summary>
        /// <param name="name"><see cref="BoxInfo"/> name.</param>
        /// <param name="siteName">Site name.</param>
        public static BoxInfo GetBoxInfo(string name, string siteName)
        {
            return ProviderObject.GetInfoByCodeName(name, SiteInfoProvider.GetSiteID(siteName));
        }


        /// <summary>
        /// Sets (updates or inserts) specified <see cref="BoxInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="BoxInfo"/> to be set.</param>
        public static void SetBoxInfo(BoxInfo infoObj)
        {
            ProviderObject.SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified <see cref="BoxInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="BoxInfo"/> to be deleted.</param>
        public static void DeleteBoxInfo(BoxInfo infoObj)
        {
            ProviderObject.DeleteInfo(infoObj);
        }


        /// <summary>
        /// Deletes <see cref="BoxInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="BoxInfo"/> ID.</param>
        public static void DeleteBoxInfo(int id)
        {
            BoxInfo infoObj = GetBoxInfo(id);
            DeleteBoxInfo(infoObj);
        }
    }
}