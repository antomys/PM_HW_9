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
            var deserialized = JsonConvert.DeserializeObject<Input>(File.ReadAllText("connectionUrl.json"));  //I'm dumb. PARSE by File.ReadAllText NOT STRING FILENAME
 
            return deserialized;
        }
        [JsonProperty("landingtest")]
        public Uri LandingTest { get; set; }
        
        [JsonProperty("isprime")]
        public Dictionary<string,HttpStatusCode> IsPrime { get; set; }
        
        [JsonProperty("getprimes")]
        public Dictionary<string,List<int>> GetPrimes { get; set; }
       
        #region Tests
        public async Task TestLandingPage(HttpClient httpClient)
        {
            var result = false;
            try
            {
                var responseMessage
                    = await httpClient.GetAsync(httpClient.BaseAddress);
                responseMessage.EnsureSuccessStatusCode();
                var responseBody = await responseMessage.Content.ReadAsStringAsync();

                const string expectedOutput = " PM_HW_9, Web service <<Prime Numbers>>\n Volokhovych Ihor ";

                if (responseBody.Equals(expectedOutput))
                {
                    result = true;
                }

                Console.WriteLine($"Input URL: [{LandingTest}]\n" +
                                  $"Expected: [{expectedOutput}]\n" +
                                  $"Received: [{responseBody}]\n" +
                                  $"Test passed: [{result.ToString()}]\n");
                
            }
            
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
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
            var tasks 
                = await Task.Factory.StartNew(() => GetPrimes.Select(pair => InternalTestGetPrimes(httpClient, pair.Key, pair.Value)));

            await Task.WhenAll(tasks);

        }

        #region PrivateTestMethods
        private async Task InternalTestIsPrime(HttpClient httpClient, string key, HttpStatusCode value)
        {
            var inputUri = new Uri(httpClient.BaseAddress + key);
            
            try
            {
                var responseMessage
                    = await httpClient.GetAsync(inputUri);

                if (responseMessage.StatusCode.Equals(value))
                {
                    Console.WriteLine($"Input URL: [{inputUri}]\nExpected: [{value}]\n" +
                                      $"Received: [{responseMessage.StatusCode}]\n" +
                                      $"Test passed: [{value == responseMessage.StatusCode}]\n");
                }
                else
                {
                    Console.WriteLine($"Input URL: [{inputUri}]\n" +
                                      $"Expected: [{value}]\n" +
                                      $"Received: [{responseMessage.StatusCode}]\n" +
                                      $"Test passed: [{value == responseMessage.StatusCode}]\n");
                }

            }
            
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }
        }

        private async Task InternalTestGetPrimes(HttpClient httpClient, string key, IReadOnlyCollection<int> value)
        {
            
            try
            {
                var inputUri = httpClient.BaseAddress + key;
                var responseMessage
                    = await httpClient.GetAsync(inputUri);

                var responseBody = await responseMessage.Content.ReadAsStringAsync();

                if (responseMessage.StatusCode.Equals(HttpStatusCode.OK))
                {
                    var numbers = new List<int>();
                    
                    if(!string.IsNullOrEmpty(responseBody))
                        numbers = responseBody.Split(',').Select(int.Parse).ToList();

                    
                    if (value.All(numbers.Contains) && value.Count == numbers.Count)
                    {
                        Console.WriteLine($"Input URL: [{inputUri}]\nExpected: [{string.Join(",", value)}]\n" +
                                          $"Received: [{responseBody}]\n" +
                                          $"Test passed: [{true}]\n");
                    }
                }
                else
                {
                    Console.WriteLine($"Input URL: [{inputUri}]\nExpected Code:[{HttpStatusCode.BadRequest}]\n" +
                                      $"Received Code:[{responseMessage.StatusCode}]\n" +
                                      $"Test passed: [{responseMessage.StatusCode == HttpStatusCode.BadRequest}]\n");
                }
            }
            
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }
        }
        #endregion
        
        #endregion

        #region ObsoleteMethods

        [Obsolete("Was used just to make json config file")]
        public void CreateInput()
        {
            
            IsPrime = new Dictionary<string, HttpStatusCode>();
            GetPrimes = new Dictionary<string, List<int>>();
            
            
            Console.WriteLine("landing_test Link:");
            LandingTest = new Uri(Console.ReadLine() ?? throw new InvalidOperationException());
            
            Console.WriteLine("is prime Link:");
            
            for (var j = 0; j < 5; j++)
            {
                var url = Console.ReadLine() ?? throw new InvalidOperationException();
                var isPrime = HttpStatusCode.OK;
                Console.WriteLine("Should it be prime?");
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    isPrime = HttpStatusCode.NotFound;
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
        

        #endregion
    }
}