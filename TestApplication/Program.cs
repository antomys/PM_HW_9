using System;
using System.Net.Http;
using System.Threading.Tasks;
using TestApplication.Models;

namespace TestApplication
{
    internal static class Program
    {
        private static readonly HttpClient Client = new();

        private static async Task Main()
        {
            Client.BaseAddress = new Uri("http://localhost:5000/");
            var input = Input.Construct();

            Console.WriteLine("*********************************************");
            await input.TestLandingPage(Client);
            Console.WriteLine("*********************************************");
            await input.TestIsPrime(Client);
            Console.WriteLine("*********************************************");
            await input.TestGetPrimes(Client);
        }
    }
}