using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PM_HW_9.Exceptions;
using PM_HW_9.Services.Interfaces;

namespace PM_HW_9.Services
{
    public class PrimeAlgorithm : IPrimeAlgorithm
    {
        private readonly ILogger<PrimeAlgorithm> _logger;
        private readonly ISettings _settings;

        public PrimeAlgorithm(
            ILogger<PrimeAlgorithm> logger,
            ISettings settings)
        {
            _logger = logger;
            _settings = settings;
        }

        public async Task<Result> GetPrimes()
        {
            //Put here. To split.
            await using var fs = new FileStream("output.json", FileMode.OpenOrCreate);
            var task = await FindPrimes();
            await JsonSerializer.SerializeAsync<Result>(fs, task);
                
            Console.WriteLine("Data has been saved to file");
            
            //todo: add something fancy. Logger for example.
            return task;
        }
        
        
        public async Task<Result> FindPrimes()
        {
            return await Task.Run(() =>
            {
                var time = DateTime.UtcNow;
                var primes = new List<int>();
                
                try
                {
                    if (_settings.PrimeFrom <= 0 || _settings.PrimeTo <= 0)
                    {
                        throw new ArgumentOutOfRange($"Out of range exception at Prime" +
                                                     $"\nAlgorithm method. Line 28");
                    }

                    if (_settings == null)
                    {
                        throw new ArgumentNull($"This was null {_settings}");
                    }

                    for (var number = _settings.PrimeFrom; number <= _settings.PrimeTo; number++)
                    {
                        var counter = 0;

                        for (var i = 2; i <= number / 2; i++)
                        {
                            if (number % i != 0) continue;
                            counter++;
                            break;
                        }

                        if (counter == 0 && number != 1)
                            primes.Add(number);
                    }

                    var elapsedTime = DateTime.Now.Subtract(time).ToString();

                    //todo: add logger
                    return new Result
                    {
                        Success = true,
                        Error = string.Empty,
                        Duration = elapsedTime,
                        Primes = primes

                    };
                }
                
                catch (ArgumentOutOfRange exception)
                {
                    //todo: add logger
                    return null;
                }
            });
        }
    }
}