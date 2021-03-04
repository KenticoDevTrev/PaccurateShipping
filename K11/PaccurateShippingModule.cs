using CMS;
using CMS.DataEngine;

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
