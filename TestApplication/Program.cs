using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TestApplication.Models;

namespace TestApplication
{
    internal static class Program
    {
        private static readonly HttpClient Client = new();

        private static async Task Main()
        {
            Client.BaseAddress = new Uri("http://localhost:5000/");
            var input = new Input();
            
            await input.TestIsPrime(Client);
            //input.SyncTest(Client);
        }
    }
}