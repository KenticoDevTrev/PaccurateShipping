using CMS;
using CMS.DataEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: RegisterModule(typeof(PaccurateShipping.PaccurateShippingModule))]


namespace PaccurateShipping
{
    public class PaccurateShippingModule : Module
    {
        public PaccurateShippingModule() : base("PaccurateShipping")
        {
        }

        protected override void OnInit()
        {
            base.OnInit();
        }
    }
}
