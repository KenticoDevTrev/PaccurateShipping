using CMS.Base;
using CMS.Ecommerce;
using Paccurate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaccurateShipping.Events
{
    public class GetItemPackagesEventArgs : CMSEventArgs
    {
        /// <summary>
        /// The Full delivery for reference
        /// </summary>
        public Delivery KenticoDelivery { get; set; }

        /// <summary>
        /// The Item that we need the Item
        /// </summary>
        public DeliveryItem Item { get; set; }

        /// <summary>
        /// Next item color
        /// </summary>
        public string DefaultItemColor { get; set; }

        /// <summary>
        /// The Item's dimensions and settings that Paccurate will use to determine packaging
        /// </summary>
        public List<ItemSet> ItemSets { get; set; }

        /// <summary>
        /// Pre-packed boxes, you can use this if you wish to determine yourself
        /// the item's packaging and not leave it up to paccurate to fit.
        /// </summary>
        public List<BoxContainer> Boxes { get; set; }

    }
}
