using CMS;
using CMS.Base.Web.UI;
using CMS.Helpers;
using CMS.UIControls;
using PaccurateShipping.Extensions;

[assembly: RegisterCustomClass("BoxColorUnigridExtension", typeof(BoxColorUnigridExtension))]

namespace PaccurateShipping.Extensions
{
    public class BoxColorUnigridExtension : ControlExtender<UniGrid>
    {
        public override void OnInit()
        {
            Control.OnExternalDataBound += Control_OnExternalDataBound;

        }

        private object Control_OnExternalDataBound(object sender, string sourceName, object parameter)
        {
            switch(sourceName.ToLowerInvariant())
            {
                case "colorswatch":
                    return $"<span style=\"display: inline-block; height: 25px; width: 25px; border: 1px solid black; background-color: {ValidationHelper.GetString(parameter, "white")};\"></span>";
                default:
                    return parameter;
            }
        }
    }
}
