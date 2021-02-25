using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IO;
using Newtonsoft.Json;
using TestApplication.Models;

namespace TestApplication
{
    internal static class Program
    {
        private static readonly HttpClient Client = new();
        private const string _fileName = "dictionary.json";

        private static async Task Main()
        {
            Client.BaseAddress = new Uri("http://localhost:5000/");
            var input = new Input();
            
            await input.TestIsPrime(Client);
            
            //await FillDictionary();



        }

        private static async Task FillDictionary()
        {
            for (int i = 0; i < 100; i++)
            {
                var deserialized = Deserialize().Result;
                var acc = new AccountDto
                {
                    SessionId = Guid.NewGuid()
                        .ToString(),
                    Login = Input.RandomString(),
                    Password = Input.RandomString(),
                    LastRequest = DateTime.Now
                };
                deserialized.Add(acc);

                await Serialize(deserialized);
            }
        }
        
        private static async Task<List<AccountDto>> Deserialize()
        {
            
            FileStream reader;
            if(File.ReadAllTextAsync(_fileName).Result != "")
            {
                byte[] fileText;
                await using (reader = File.Open(_fileName, FileMode.Open))
                {
                    fileText = new byte[reader.Length];
                    await reader.ReadAsync(fileText, 0, (int)reader.Length);
                }

                var decoded = Encoding.ASCII.GetString(fileText);
                    
                    
                var list = await Task.Run(() => 
                    JsonConvert.DeserializeObject<List<AccountDto>>(decoded));
                return list;
            }
            reader = File.Open(_fileName, FileMode.OpenOrCreate);
            reader.Close();
            return new List<AccountDto>();
            
           
        }

        private static async Task Serialize(List<AccountDto> accountDtos)
        {
            var streamManager = new RecyclableMemoryStreamManager();

            using var file = File.Open(_fileName, FileMode.OpenOrCreate);
            using var memoryStream = streamManager.GetStream();
            using var writer = new StreamWriter(memoryStream);
            
            var serializer = JsonSerializer.CreateDefault();
                
            serializer.Serialize(writer, accountDtos);      // FROM STACKOVERFLOW
                
            await writer.FlushAsync().ConfigureAwait(false);
                
            memoryStream.Seek(0, SeekOrigin.Begin);
                
            await memoryStream.CopyToAsync(file).ConfigureAwait(false);

            await file.FlushAsync().ConfigureAwait(false);
            
        }
        
    }
}