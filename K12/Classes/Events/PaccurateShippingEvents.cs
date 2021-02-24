namespace PaccurateShipping.Events
{
    public static class PaccurateShippingEvents
    {
        public static BuildRequestEventHandler BuildRequest;
        public static GetItemPackagesEventHandler GetItemPackagesRequest;

        static PaccurateShippingEvents()
        {
            BuildRequest = new BuildRequestEventHandler()
            {
                Name = "PaccurageShipping.BuildRequest"
            };

            GetItemPackagesRequest = new GetItemPackagesEventHandler()
            {
                Name = "PaccurageShipping.GetItemPackagesRequest"
            };

        }
    }
}
