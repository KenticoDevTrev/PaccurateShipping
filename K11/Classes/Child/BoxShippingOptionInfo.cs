using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;
using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using PaccurateShipping;

[assembly: RegisterObjectType(typeof(BoxShippingOptionInfo), BoxShippingOptionInfo.OBJECT_TYPE)]

namespace PaccurateShipping
{
    /// <summary>
    /// Data container class for <see cref="BoxShippingOptionInfo"/>.
    /// </summary>
	[Serializable]
    public partial class BoxShippingOptionInfo : AbstractInfo<BoxShippingOptionInfo>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "paccurateshipping.boxshippingoption";


        /// <summary>
        /// Type information.
        /// </summary>
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(BoxShippingOptionInfoProvider), OBJECT_TYPE, "PaccurateShipping.BoxShippingOption", "BoxShippingOptionID", "BoxShippingOptionLastModified", "BoxShippingOptionGuid", null, null, null, null, nameof(BoxShippingOptionBoxID), BoxInfo.OBJECT_TYPE)
        {
            ModuleName = "PaccurateShipping",
            TouchCacheDependencies = true,
            SynchronizationSettings =
            {
                LogSynchronization = SynchronizationTypeEnum.TouchParent, // Enables logging of staging tasks for changes made to Office objects
                ObjectTreeLocations = new List<ObjectTreeLocation>()
                {
                    // Creates a new category in the 'Global objects' section of the staging object tree
                    new ObjectTreeLocation(GLOBAL, "PaccurateShipping")
                }
            },
            DependsOn = new List<ObjectDependency>()
            {
                new ObjectDependency("BoxShippingOptionBoxID", "paccurateshipping.box", ObjectDependencyEnum.Required),
                new ObjectDependency("BoxShippingOptionShippingOptionID", "ecommerce.shippingoption", ObjectDependencyEnum.Required),
            },
            EnabledColumn = "BoxShippingOptionEnabled",
        };


        /// <summary>
        /// Box shipping option ID.
        /// </summary>
		[DatabaseField]
        public virtual int BoxShippingOptionID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("BoxShippingOptionID"), 0);
            }
            set
            {
                SetValue("BoxShippingOptionID", value);
            }
        }


        /// <summary>
        /// Box shipping option box ID.
        /// </summary>
        [DatabaseField]
        public virtual int BoxShippingOptionBoxID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("BoxShippingOptionBoxID"), 0);
            }
            set
            {
                SetValue("BoxShippingOptionBoxID", value);
            }
        }


        /// <summary>
        /// If unchecked, this box will not be used for this shipping option..
        /// </summary>
        [DatabaseField]
        public virtual bool BoxShippingOptionEnabled
        {
            get
            {
                return ValidationHelper.GetBoolean(GetValue("BoxShippingOptionEnabled"), true);
            }
            set
            {
                SetValue("BoxShippingOptionEnabled", value);
            }
        }


        /// <summary>
        /// Box shipping option shipping option ID.
        /// </summary>
        [DatabaseField]
        public virtual int BoxShippingOptionShippingOptionID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("BoxShippingOptionShippingOptionID"), 0);
            }
            set
            {
                SetValue("BoxShippingOptionShippingOptionID", value);
            }
        }


        /// <summary>
        /// Zone of rate table to use.
        /// </summary>
        [DatabaseField]
        public virtual string BoxShippingOptionZone
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BoxShippingOptionZone"), String.Empty);
            }
            set
            {
                SetValue("BoxShippingOptionZone", value, String.Empty);
            }
        }


        /// <summary>
        /// Custom Weight to Rate Listing will allow you to specify the cost per weight unit you specify..
        /// .
        /// Universal Cost Per Weight is a straight Cost * Weight Unit..
        /// </summary>
        [DatabaseField]
        public virtual bool BoxShippingOptionRateUseSpecifiedRates
        {
            get
            {
                return ValidationHelper.GetBoolean(GetValue("BoxShippingOptionRateUseSpecifiedRates"), true);
            }
            set
            {
                SetValue("BoxShippingOptionRateUseSpecifiedRates", value);
            }
        }


        /// <summary>
        /// One per line, weight;price.
        /// </summary>
        [DatabaseField]
        public virtual string BoxShippingOptionRateWeights
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BoxShippingOptionRateWeights"), String.Empty);
            }
            set
            {
                SetValue("BoxShippingOptionRateWeights", value, String.Empty);
            }
        }


        /// <summary>
        /// Box shipping option price increase rate.
        /// </summary>
        [DatabaseField]
        public virtual decimal BoxShippingOptionPriceIncreaseRate
        {
            get
            {
                return ValidationHelper.GetDecimal(GetValue("BoxShippingOptionPriceIncreaseRate"), 0m);
            }
            set
            {
                SetValue("BoxShippingOptionPriceIncreaseRate", value, 0m);
            }
        }


        /// <summary>
        /// Base Price of the box.
        /// </summary>
        [DatabaseField]
        public virtual decimal BoxShippingOptionBasePrice
        {
            get
            {
                return ValidationHelper.GetDecimal(GetValue("BoxShippingOptionBasePrice"), 0m);
            }
            set
            {
                SetValue("BoxShippingOptionBasePrice", value, 0m);
            }
        }


        /// <summary>
        /// This is the Dimensional Weight divisor. It is given in units of volume per unit weight, e.g., the standard of “139” represents 139 cubic inches per pound, and is used to convert the total volume of a carton into a functional minimum weight to be used when rating the carton. E.g., a carton with dimensions 10" x 10" x 13.9" would yield a volume of 1390 cubic inches..
        /// </summary>
        [DatabaseField]
        public virtual decimal BoxShippingOptionDimFactor
        {
            get
            {
                return ValidationHelper.GetDecimal(GetValue("BoxShippingOptionDimFactor"), 0m);
            }
            set
            {
                SetValue("BoxShippingOptionDimFactor", value, 0m);
            }
        }


        /// <summary>
        /// Box shipping option guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid BoxShippingOptionGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("BoxShippingOptionGuid"), Guid.Empty);
            }
            set
            {
                SetValue("BoxShippingOptionGuid", value);
            }
        }


        /// <summary>
        /// Box shipping option last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime BoxShippingOptionLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("BoxShippingOptionLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("BoxShippingOptionLastModified", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            BoxShippingOptionInfoProvider.DeleteBoxShippingOptionInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            BoxShippingOptionInfoProvider.SetBoxShippingOptionInfo(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected BoxShippingOptionInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="BoxShippingOptionInfo"/> class.
        /// </summary>
        public BoxShippingOptionInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="BoxShippingOptionInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public BoxShippingOptionInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}