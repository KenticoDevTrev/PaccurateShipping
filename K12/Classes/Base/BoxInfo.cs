using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using PaccurateShipping;

[assembly: RegisterObjectType(typeof(BoxInfo), BoxInfo.OBJECT_TYPE)]

namespace PaccurateShipping
{
    /// <summary>
    /// Data container class for <see cref="BoxInfo"/>.
    /// </summary>
	[Serializable]
    public partial class BoxInfo : AbstractInfo<BoxInfo>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "paccurateshipping.box";


        /// <summary>
        /// Type information.
        /// </summary>
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(BoxInfoProvider), OBJECT_TYPE, "PaccurateShipping.Box", "BoxID", "BoxLastModified", "BoxGuid", "BoxCodeName", "BoxDisplayName", null, "BoxSiteID", null, null)
        {
            ModuleName = "PaccurateShipping",
            TouchCacheDependencies = true,
            SynchronizationSettings =
            {
                LogSynchronization = SynchronizationTypeEnum.LogSynchronization, // Enables logging of staging tasks for changes made to Office objects
                ObjectTreeLocations = new List<ObjectTreeLocation>()
                {
                    // Creates a new category in the 'Global objects' section of the staging object tree
                    new ObjectTreeLocation(GLOBAL, "PaccurateShipping")
                }
            },
            EnabledColumn = "BoxEnabled",
        };


        /// <summary>
        /// Box ID.
        /// </summary>
        [DatabaseField]
        public virtual int BoxID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("BoxID"), 0);
            }
            set
            {
                SetValue("BoxID", value);
            }
        }


        /// <summary>
        /// Box site ID.
        /// </summary>
        [DatabaseField]
        public virtual int BoxSiteID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("BoxSiteID"), 0);
            }
            set
            {
                SetValue("BoxSiteID", value, 0);
            }
        }


        /// <summary>
        /// If unchecked, this box will not be used in calculations.
        /// </summary>
        [DatabaseField]
        public virtual bool BoxEnabled
        {
            get
            {
                return ValidationHelper.GetBoolean(GetValue("BoxEnabled"), true);
            }
            set
            {
                SetValue("BoxEnabled", value);
            }
        }


        /// <summary>
        /// Box display name.
        /// </summary>
        [DatabaseField]
        public virtual string BoxDisplayName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BoxDisplayName"), String.Empty);
            }
            set
            {
                SetValue("BoxDisplayName", value);
            }
        }


        /// <summary>
        /// Box code name.
        /// </summary>
        [DatabaseField]
        public virtual string BoxCodeName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BoxCodeName"), String.Empty);
            }
            set
            {
                SetValue("BoxCodeName", value);
            }
        }


        /// <summary>
        /// Box dimension X.
        /// </summary>
        [DatabaseField]
        public virtual decimal BoxDimensionX
        {
            get
            {
                return ValidationHelper.GetDecimal(GetValue("BoxDimensionX"), 0m);
            }
            set
            {
                SetValue("BoxDimensionX", value);
            }
        }


        /// <summary>
        /// Box dimension Y.
        /// </summary>
        [DatabaseField]
        public virtual decimal BoxDimensionY
        {
            get
            {
                return ValidationHelper.GetDecimal(GetValue("BoxDimensionY"), 0m);
            }
            set
            {
                SetValue("BoxDimensionY", value);
            }
        }


        /// <summary>
        /// Box dimension Z.
        /// </summary>
        [DatabaseField]
        public virtual decimal BoxDimensionZ
        {
            get
            {
                return ValidationHelper.GetDecimal(GetValue("BoxDimensionZ"), 0m);
            }
            set
            {
                SetValue("BoxDimensionZ", value);
            }
        }


        /// <summary>
        /// Box max weight.
        /// </summary>
        [DatabaseField]
        public virtual decimal BoxMaxWeight
        {
            get
            {
                return ValidationHelper.GetDecimal(GetValue("BoxMaxWeight"), 0m);
            }
            set
            {
                SetValue("BoxMaxWeight", value, 0m);
            }
        }


        /// <summary>
        /// Weight of the box itself, empty..
        /// </summary>
        [DatabaseField]
        public virtual decimal BoxWeight
        {
            get
            {
                return ValidationHelper.GetDecimal(GetValue("BoxWeight"), 0m);
            }
            set
            {
                SetValue("BoxWeight", value, 0m);
            }
        }


        /// <summary>
        /// Optional if the box has a fixed price.
        /// </summary>
        [DatabaseField]
        public virtual decimal BoxPrice
        {
            get
            {
                return ValidationHelper.GetDecimal(GetValue("BoxPrice"), 0m);
            }
            set
            {
                SetValue("BoxPrice", value, 0m);
            }
        }


        /// <summary>
        /// Box guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid BoxGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("BoxGuid"), Guid.Empty);
            }
            set
            {
                SetValue("BoxGuid", value);
            }
        }


        /// <summary>
        /// Box last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime BoxLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("BoxLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("BoxLastModified", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            BoxInfoProvider.DeleteBoxInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            BoxInfoProvider.SetBoxInfo(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected BoxInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="BoxInfo"/> class.
        /// </summary>
        public BoxInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="BoxInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public BoxInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}