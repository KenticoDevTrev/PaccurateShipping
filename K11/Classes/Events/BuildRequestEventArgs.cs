using CMS.Base;
using CMS.Ecommerce;
using Paccurate.Models;
using System;
using System.Web;

namespace PaccurateShipping.Events
{
    public class BuildRequestEventArgs : CMSEventArgs
    {
        /// <summary>
        /// The Kentico Delivery Object
        /// </summary>
        public Delivery KenticoDelivery { get; set; }

        public PackRequest PaccurateRequest { get; set; }
    }
}
