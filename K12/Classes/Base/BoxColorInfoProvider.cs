using System;
using System.Data;

using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;

namespace PaccurateShipping
{
    /// <summary>
    /// Class providing <see cref="BoxColorInfo"/> management.
    /// </summary>
    public partial class BoxColorInfoProvider : AbstractInfoProvider<BoxColorInfo, BoxColorInfoProvider>
    {
        /// <summary>
        /// Creates an instance of <see cref="BoxColorInfoProvider"/>.
        /// </summary>
        public BoxColorInfoProvider()
            : base(BoxColorInfo.TYPEINFO)
        {
        }


        /// <summary>
        /// Returns a query for all the <see cref="BoxColorInfo"/> objects.
        /// </summary>
        public static ObjectQuery<BoxColorInfo> GetBoxColors()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns <see cref="BoxColorInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="BoxColorInfo"/> ID.</param>
        public static BoxColorInfo GetBoxColorInfo(int id)
        {
            return ProviderObject.GetInfoById(id);
        }


        /// <summary>
        /// Returns <see cref="BoxColorInfo"/> with specified name.
        /// </summary>
        /// <param name="name"><see cref="BoxColorInfo"/> name.</param>
        public static BoxColorInfo GetBoxColorInfo(string name)
        {
            return ProviderObject.GetInfoByCodeName(name);
        }


        /// <summary>
        /// Sets (updates or inserts) specified <see cref="BoxColorInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="BoxColorInfo"/> to be set.</param>
        public static void SetBoxColorInfo(BoxColorInfo infoObj)
        {
            // Set order if not set.
            if(infoObj.GetValue(infoObj.TypeInfo.OrderColumn) == null)
            {
                infoObj.SetValue(infoObj.TypeInfo.OrderColumn, GetBoxColors().Count + 1);
            }
            ProviderObject.SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified <see cref="BoxColorInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="BoxColorInfo"/> to be deleted.</param>
        public static void DeleteBoxColorInfo(BoxColorInfo infoObj)
        {
            ProviderObject.DeleteInfo(infoObj);
            // Reorder
            infoObj.Generalized.InitObjectsOrder();
        }


        /// <summary>
        /// Deletes <see cref="BoxColorInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="BoxColorInfo"/> ID.</param>
        public static void DeleteBoxColorInfo(int id)
        {
            BoxColorInfo infoObj = GetBoxColorInfo(id);
            DeleteBoxColorInfo(infoObj);
        }
    }
}