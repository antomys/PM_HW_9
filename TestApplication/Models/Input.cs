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
            /*for (int i = 0; i < 100; i++)
                await RegisterTest(httpClient, _concurrent[i]);*/
            /*for (int i = 0; i < 100; i++)
                await LoginTest(httpClient, _concurrent[i]);
            for (int i = 0; i < 100; i++)
                await CreatePublicRoom(httpClient, _concurrent[i]);*/
            /*for (int i = 0; i < 200; i++)
                await JoinPublicRoom(httpClient, _concurrent[i]);*/
            /*for (int i = 0; i < 200; i++)
                await JoinPrivateRoom(httpClient, _concurrent[i], "oimi2");*/
            /*for (int i = 0; i < 200; i++)
                await ChangePlayerState(httpClient, _concurrent[i]);*/
            for (int i = 0; i < 200; i++)
                await LogOutTest(httpClient, _concurrent[i]);
            /*foreach (var account in _concurrent)
            {
                await CreatePublicRoom(httpClient, account);
            }*/

        }

        public void SyncTest(HttpClient httpClient)
        {
            foreach (var account in _concurrent)
            {
                InternalTestPrime(httpClient,account);
            }
        }
        
        private async Task LoginTest(HttpClient httpClient, AccountDto thisAcc)
        { 
            var stringContent = new StringContent(JsonConvert.SerializeObject(thisAcc), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"user/login", stringContent);
            var responseBody = await response.Content.ReadAsStringAsync();
            
            Console.WriteLine($"**SessionID: {thisAcc.SessionId}, Input: Login: {thisAcc.Login}, Pass: {thisAcc.Password}\nCode: {responseBody}**\n");
        }
        
        private async Task LogOutTest(HttpClient httpClient, AccountDto thisAcc)
        { 
            var stringContent = new StringContent(JsonConvert.SerializeObject(thisAcc), Encoding.UTF8, "application/json");

            var response = await httpClient.GetAsync($"/user/logout/{thisAcc.SessionId}");
            var responseBody = await response.Content.ReadAsStringAsync();
            
            Console.WriteLine($"**SessionID: {thisAcc.SessionId}, Input: Login: {thisAcc.Login}, Pass: {thisAcc.Password}\nCode: {responseBody}**\n");
        }
        
        private async Task RegisterTest(HttpClient httpClient, AccountDto thisAcc)
        { 
            var stringContent = new StringContent(JsonConvert.SerializeObject(thisAcc), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"user/create", stringContent);
            var responseBody = await response.Content.ReadAsStringAsync();
            
            Console.WriteLine($"**SessionID: {thisAcc.SessionId}, Input: Login: {thisAcc.Login}, Pass: {thisAcc.Password}\nCode: {responseBody}**\n");
        }
        
        private async Task JoinPrivateRoom(HttpClient httpClient, AccountDto thisAcc, string roomId)
        { 
            var stringContent = new StringContent(JsonConvert.SerializeObject(thisAcc), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"room/join/{thisAcc.SessionId}&{roomId}", stringContent);
            var responseBody = await response.Content.ReadAsStringAsync();
            
            Console.WriteLine($"**SessionID: {thisAcc.SessionId}, Input: Login: {thisAcc.Login}, Pass: {thisAcc.Password}\nCode: {responseBody}**\n");
        }
        
        private async Task CreatePublicRoom(HttpClient httpClient, AccountDto thisAcc)
        { 
            var stringContent = new StringContent(JsonConvert.SerializeObject(thisAcc), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"room/create/{thisAcc.SessionId}&{false}", stringContent);
            var responseBody = await response.Content.ReadAsStringAsync();
            
            Console.WriteLine($"**SessionID: {thisAcc.SessionId}, Input: Login: {thisAcc.Login}, Pass: {thisAcc.Password}\nCode: {responseBody}**\n");
        }
        private async Task JoinPublicRoom(HttpClient httpClient, AccountDto thisAcc)
        { 
            var stringContent = new StringContent(JsonConvert.SerializeObject(thisAcc), Encoding.UTF8, "application/json");

            var response = await httpClient.GetAsync($"room/join/{thisAcc.SessionId}");
            var responseBody = await response.Content.ReadAsStringAsync();
            
            Console.WriteLine($"**SessionID: {thisAcc.SessionId}, Input: Login: {thisAcc.Login}, Pass: {thisAcc.Password}\nCode: {responseBody}**\n");
        }
        
        private async Task ChangePlayerState(HttpClient httpClient, AccountDto thisAcc)
        { 
            var stringContent = new StringContent(JsonConvert.SerializeObject(thisAcc), Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"room/updateState/{thisAcc.SessionId}&{true}",stringContent);
            var responseBody = await response.Content.ReadAsStringAsync();
            
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
        
        private static readonly Random random = new();
        public static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        
    }
}