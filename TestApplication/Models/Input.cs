using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestApplication.Models
{
    public class Input : IInput
    {

        public static Input Construct()
        {
            var deserialized = JsonConvert.DeserializeObject<Input>(File.ReadAllText("connectionUrl.json"));  //I'm dumb. PARSE FILE.READALLTEXT NOT STRING FILENAME
 
            return deserialized;
        }
        [JsonProperty("landingtest")]
        public string LandingTest { get; set; }
        
        [JsonProperty("isprime")]
        public Dictionary<string,bool> IsPrime { get; set; }
        
        [JsonProperty("getprimes")]
        public Dictionary<string,List<int>> GetPrimes { get; set; }

        [Obsolete("Was used just to make json config file")]
        public void CreateInput()
        {
            
            IsPrime = new Dictionary<string, bool>();
            GetPrimes = new Dictionary<string, List<int>>();
            
            
            Console.WriteLine("landing_test Link:");
            LandingTest = Console.ReadLine() ?? throw new InvalidOperationException();
            
            Console.WriteLine("is prime Link:");
            
            for (var j = 0; j < 5; j++)
            {
                var url = Console.ReadLine() ?? throw new InvalidOperationException();
                var isPrime = true;
                Console.WriteLine("Should it be prime?");
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    isPrime = false;
                }
                IsPrime.Add(url,isPrime);
            }
            
            Console.WriteLine("get primes Link:");
            for (var j = 0; j < 5; j++)
            {
                var url = Console.ReadLine() ?? throw new InvalidOperationException();
                var primeList = new List<int>();
                
                Console.WriteLine("Please input separated by comma");
                var input = Console.ReadLine();
                if(input != string.Empty)
                    if (input != null)
                        primeList = input.Split(',').Select(int.Parse).ToList();

                GetPrimes.Add(url,primeList);
            }

            File.WriteAllText(
                "connectionUrl.json",
                JsonConvert.SerializeObject(this, Formatting.Indented));
        }
        
        public async Task<bool> TestLandingPage(HttpClient httpClient)
        {
            var result = false;
            try
            {
                HttpResponseMessage responseMessage
                    = await httpClient.GetAsync(new Uri(LandingTest));
                responseMessage.EnsureSuccessStatusCode();
                string responseBody = await responseMessage.Content.ReadAsStringAsync();

                var expectedOutput = " PM_HW_9, Web service <<Prime Numbers>>\n Volokhovych Ihor ";

                if (responseBody.Equals(expectedOutput))
                {
                    result = true;
                }

                Console.WriteLine($" Input URL: [{LandingTest}]\nExpected: [{expectedOutput}]\nReceived: [{responseBody}]\nTest passed: [{result.ToString()}]");

                return result;
            }
            
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
                return false;
            }
        }

        public async Task TestIsPrime(HttpClient httpClient)
        {
            var tasks = IsPrime
                .Select(pair => InternalTestIsPrime(httpClient, pair.Key, pair.Value));
            
            await Task.WhenAll(tasks);
            

        }

        public async Task TestGetPrimes(HttpClient httpClient)
        {
            var tasks = GetPrimes
                .Select(pair => InternalTestGetPrimes(httpClient, pair.Key, pair.Value));
            
            await Task.WhenAll(tasks);
            
        }

        private async Task<bool> InternalTestIsPrime(HttpClient httpClient, string key, bool value)
        {
            var result = false;
            try
            {
                HttpResponseMessage responseMessage
                    = await httpClient.GetAsync(new Uri(key));
                //responseMessage.EnsureSuccessStatusCode();
                
                //string responseBody = await responseMessage.Content.ReadAsStringAsync();
                

                if (responseMessage.StatusCode.Equals(HttpStatusCode.OK))
                {
                    result = true;
                }

                Console.WriteLine($" Input URL: [{key}]\nExpected: [{value}]\nReceived: [{responseMessage.StatusCode}]\nTest passed: [{result.ToString()}]");

                return result;
            }
            
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
                return false;
            }
        }

        private async Task<bool> InternalTestGetPrimes(HttpClient httpClient, string key, List<int> value)
        {
            var result = false;
                try
                {
                    var responseMessage
                        = await httpClient.GetAsync(new Uri(key));
                    //responseMessage.EnsureSuccessStatusCode();
                
                    var responseBody = await responseMessage.Content.ReadAsStringAsync();

                    if (responseMessage.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        var numbers = responseBody.Split(',').Select(int.Parse).ToList();

                        if (value.All(numbers.Contains) && value.Count == numbers.Count)
                        {
                            result = true;
                        }
                    }

                    Console.WriteLine($" Input URL: [{key}]\nExpected: [{string.Join(",", value)}]\nReceived: [{responseBody}]\nTest passed: [{result.ToString()}]");

                    return result;
                }
            
                catch(HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");	
                    Console.WriteLine("Message :{0} ",e.Message);
                    return false;
                }
                
        }
    }
}