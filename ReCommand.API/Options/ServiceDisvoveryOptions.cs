namespace ReCommand.API.Options
{
    public class ServiceDisvoveryOptions
    {
        public string ContactServiceName { get; set; }
        public string UserServiceName { get; set; }
        public string ServiceName { get; set; }

        public ConsulOptions Consul { get; set; }
    }
}