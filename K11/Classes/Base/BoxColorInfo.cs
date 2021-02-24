using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using PaccurateShipping;

[assembly: RegisterObjectType(typeof(BoxColorInfo), BoxColorInfo.OBJECT_TYPE)]

namespace PaccurateShipping
{
    /// <summary>
    /// Data container class for <see cref="BoxColorInfo"/>.
    /// </summary>
	[Serializable]
    public partial class BoxColorInfo : AbstractInfo<BoxColorInfo>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "paccurateshipping.boxcolor";


        /// <summary>
        /// Type information.
        /// </summary>
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(BoxColorInfoProvider), OBJECT_TYPE, "PaccurateShipping.BoxColor", "BoxColorID", "BoxColorLastModified", "BoxColorGuid", "BoxColorName", null, null, null, null, null)
        {
            ModuleName = "PaccurateShipping",
            TouchCacheDependencies = true,
            OrderColumn = "BoxColorOrder",
            SynchronizationSettings =
            {
                LogSynchronization = SynchronizationTypeEnum.LogSynchronization, // Enables logging of staging tasks for changes made to Office objects
                ObjectTreeLocations = new List<ObjectTreeLocation>()
                {
                    // Creates a new category in the 'Global objects' section of the staging object tree
                    new ObjectTreeLocation(GLOBAL, "PaccurateShipping")
                }
            }
        };


        /// <summary>
        /// Box color ID.
        /// </summary>
        [DatabaseField]
        public virtual int BoxColorID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("BoxColorID"), 0);
            }
            set
            {
                SetValue("BoxColorID", value);
            }
        }


        /// <summary>
        /// Box color name.
        /// </summary>
        [DatabaseField]
        public virtual string BoxColorName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BoxColorName"), String.Empty);
            }
            set
            {
                SetValue("BoxColorName", value);
            }
        }


        /// <summary>
        /// Box color order.
        /// </summary>
        [DatabaseField]
        public virtual int BoxColorOrder
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("BoxColorOrder"), 0);
            }
            set
            {
                SetValue("BoxColorOrder", value);
            }
        }


        /// <summary>
        /// Box color guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid BoxColorGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("BoxColorGuid"), Guid.Empty);
            }
            set
            {
                SetValue("BoxColorGuid", value);
            }
        }


        /// <summary>
        /// Box color last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime BoxColorLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("BoxColorLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("BoxColorLastModified", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            BoxColorInfoProvider.DeleteBoxColorInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            BoxColorInfoProvider.SetBoxColorInfo(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected BoxColorInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="BoxColorInfo"/> class.
        /// </summary>
        public BoxColorInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="BoxColorInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public BoxColorInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}