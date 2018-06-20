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
                Console.WriteLine($"请输入consul server地址用于测试，默认 192.168.182.131");

                var inputStr = Console.ReadLine();

                var input = string.IsNullOrWhiteSpace(inputStr) ? "192.168.182.131" : inputStr;

                IDnsQuery dnsQuery = new LookupClient(IPAddress.Parse($"{input}"), 8600);
                var result = dnsQuery.ResolveService("service.consul", "api");

                var first = result.OrderBy(x => Guid.NewGuid()).First();

                var addressList = first.AddressList;

                var address = addressList.Any() ? addressList.First().ToString() : first.HostName;

                var port = first.Port;

                Console.WriteLine($"consul服务发现的api地址: {address}:{port}");

                HttpClient client = new HttpClient();

                Console.WriteLine($"请求地址: http://{address}:{port}/api/values");

                address = address.Replace(".", "");

                var str = client.GetStringAsync($"http://{address}:{port}/api/values").Result;

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
