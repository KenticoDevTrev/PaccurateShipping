using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using PaccurateShipping;

[assembly: RegisterObjectType(typeof(ShippingLayoutInfo), ShippingLayoutInfo.OBJECT_TYPE)]

namespace PaccurateShipping
{
    /// <summary>
    /// Data container class for <see cref="ShippingLayoutInfo"/>.
    /// </summary>
	[Serializable]
    public partial class ShippingLayoutInfo : AbstractInfo<ShippingLayoutInfo>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "paccurateshipping.shippinglayout";


        /// <summary>
        /// Type information.
        /// </summary>
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(ShippingLayoutInfoProvider), OBJECT_TYPE, "PaccurateShipping.ShippingLayout", "ShippingLayoutID", "ShippingLayoutLastModified", "ShippingLayoutGuid", null, null, null, null, null, null)
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
            DependsOn = new List<ObjectDependency>()
            {
                new ObjectDependency("ShippingLayoutOrderID", "ecommerce.order", ObjectDependencyEnum.Required),
            },
        };


        /// <summary>
        /// Shipping layout ID.
        /// </summary>
        [DatabaseField]
        public virtual int ShippingLayoutID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("ShippingLayoutID"), 0);
            }
            set
            {
                SetValue("ShippingLayoutID", value);
            }
        }


        /// <summary>
        /// Shipping layout order ID.
        /// </summary>
        [DatabaseField]
        public virtual int ShippingLayoutOrderID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("ShippingLayoutOrderID"), 0);
            }
            set
            {
                SetValue("ShippingLayoutOrderID", value);
            }
        }


        /// <summary>
        /// Shipping layout response JSON.
        /// </summary>
        [DatabaseField]
        public virtual string ShippingLayoutResponseJSON
        {
            get
            {
                return ValidationHelper.GetString(GetValue("ShippingLayoutResponseJSON"), String.Empty);
            }
            set
            {
                SetValue("ShippingLayoutResponseJSON", value);
            }
        }


        /// <summary>
        /// Shipping layout packing instructions.
        /// </summary>
        [DatabaseField]
        public virtual string ShippingLayoutPackingInstructions
        {
            get
            {
                return ValidationHelper.GetString(GetValue("ShippingLayoutPackingInstructions"), String.Empty);
            }
            set
            {
                SetValue("ShippingLayoutPackingInstructions", value, String.Empty);
            }
        }


        /// <summary>
        /// Shipping layout packing SVG.
        /// </summary>
        [DatabaseField]
        public virtual string ShippingLayoutPackingSVG
        {
            get
            {
                return ValidationHelper.GetString(GetValue("ShippingLayoutPackingSVG"), String.Empty);
            }
            set
            {
                SetValue("ShippingLayoutPackingSVG", value, String.Empty);
            }
        }


        /// <summary>
        /// If packages are left over, the user should be warned here..
        /// </summary>
        [DatabaseField]
        public virtual string ShippingLayoutLeftoverNotice
        {
            get
            {
                return ValidationHelper.GetString(GetValue("ShippingLayoutLeftoverNotice"), String.Empty);
            }
            set
            {
                SetValue("ShippingLayoutLeftoverNotice", value, String.Empty);
            }
        }


        /// <summary>
        /// Shipping layout guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid ShippingLayoutGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("ShippingLayoutGuid"), Guid.Empty);
            }
            set
            {
                SetValue("ShippingLayoutGuid", value);
            }
        }


        /// <summary>
        /// Shipping layout last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime ShippingLayoutLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("ShippingLayoutLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("ShippingLayoutLastModified", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            ShippingLayoutInfoProvider.DeleteShippingLayoutInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            ShippingLayoutInfoProvider.SetShippingLayoutInfo(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected ShippingLayoutInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="ShippingLayoutInfo"/> class.
        /// </summary>
        public ShippingLayoutInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="ShippingLayoutInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public ShippingLayoutInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}