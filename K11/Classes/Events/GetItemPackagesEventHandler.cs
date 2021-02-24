using CMS.Base;

namespace PaccurateShipping.Events
{
    public class GetItemPackagesEventHandler : AdvancedHandler<GetItemPackagesEventHandler, GetItemPackagesEventArgs>
    {
        public GetItemPackagesEventHandler()
        {

        }

        public GetItemPackagesEventHandler StartEvent(GetItemPackagesEventArgs GetItemPackagesArgs)
        {
            return base.StartEvent(GetItemPackagesArgs);
        }

        public void FinishEvent()
        {
            base.Finish();
        }
    }
}