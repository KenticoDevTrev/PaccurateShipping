using CMS.Base;

namespace PaccurateShipping.Events
{
    public class BuildRequestEventHandler : AdvancedHandler<BuildRequestEventHandler, BuildRequestEventArgs>
    {
        public BuildRequestEventHandler()
        {

        }

        public BuildRequestEventHandler StartEvent(BuildRequestEventArgs BuildRequestArgs)
        {
            return base.StartEvent(BuildRequestArgs);
        }

        public void FinishEvent()
        {
            base.Finish();
        }
    }
}
