using System;
using System.Linq;
using System.Net;
using DnsClient;

namespace TestDnsClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            try
            {
                IDnsQuery dnsQuery = new LookupClient(IPAddress.Parse("127.0.0.1"), 8600);
                var result = dnsQuery.ResolveService("service.consul", "api");

                var first = result.First();

                var addressList = first.AddressList;

                var address = addressList.Any() ? addressList.First().ToString() : first.HostName;

                var port = result.First().Port;

                Console.WriteLine($"consul地址: {address}:{port}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.ReadKey();
        }
    }
}
