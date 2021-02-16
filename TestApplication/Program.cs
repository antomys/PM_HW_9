using System;
using System.Net.Http;
using System.Threading.Tasks;
using TestApplication.Models;

namespace TestApplication
{
    class Program
    {
        private static readonly HttpClient Client = new HttpClient();

        private static async Task Main()
        {
            var input = Input.Construct();
            //var input = new Input();
            //input.CreateInput();
            //await input.TestLandingPage(Client);
            //await input.TestIsPrime(Client);
            await input.TestGetPrimes(Client);
        }
    }
}