using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestApplication.Models
{
    public class Input
    {
        
        private readonly List<AccountDto> _concurrent = JsonConvert
                .DeserializeObject<List<AccountDto>>(File.ReadAllText("dictionary.json"));
        
        

        public async Task TestIsPrime(HttpClient httpClient)
        {
            var tasks = _concurrent
                .Select(pair => InternalTestIsPrime(httpClient, pair));
            
            await Task.WhenAll(tasks);
            

        }

        public void SyncTest(HttpClient httpClient)
        {
            foreach (var account in _concurrent)
            {
                InternalTestPrime(httpClient,account);
            }
        }
        
        private async Task InternalTestIsPrime(HttpClient httpClient, AccountDto accountDto)
        {
            var thisAcc = new AccountDto
            {
                SessionId = accountDto.SessionId,
                Login = accountDto.Login,
                Password = accountDto.Password,
                LastRequest = DateTime.Now
            };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(httpClient.BaseAddress+"user/login"),
                Content = new StringContent(JsonConvert.SerializeObject(thisAcc), Encoding.UTF8, "application/json")
            };
            
            var response = await httpClient.SendAsync(request).ConfigureAwait(false); //TODO: Cancellation token
            
            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Console.WriteLine($"**SessionID: {thisAcc.SessionId}, Input: Login: {thisAcc.Login}, Pass: {thisAcc.Password}\nCode: {responseBody}**\n");
        }
        
        private void  InternalTestPrime(HttpClient httpClient, AccountDto accountDto)
        {

            //var stringContent = new StringContent(JsonConvert.SerializeObject(inputAccount), Encoding.UTF8, "application/json");
            
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(httpClient.BaseAddress+"user/login"),
                Content = new StringContent(JsonConvert.SerializeObject(accountDto), Encoding.UTF8, "application/json")
            };
            
            var response = httpClient.Send(request); //TODO: Cancellation token
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Console.WriteLine($"**SessionID: {accountDto.SessionId}, Input: Login: {accountDto.Login}, Pass: {accountDto.Password}\nCode: {responseBody}" +
                              $"{responseBody}**");
        }
        
    }
}