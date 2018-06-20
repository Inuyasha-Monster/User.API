using System;
using System.Linq;
using System.Net;
using System.Net.Http;
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

                Console.WriteLine($"consul服务发现的api地址: {address}:{port}");

                HttpClient client = new HttpClient();

                var str = client.GetStringAsync($"{address}:{port}/api/values").Result;

                Console.WriteLine($"api服务返回字符串结果->> {str}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.ReadKey();
        }
    }
}
