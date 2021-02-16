using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
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
        private const string FileName = "output.json";

        public PrimeAlgorithm(
            ILogger<PrimeAlgorithm> logger,
            ISettings settings)
        {
            _logger = logger;
            _settings = settings;
        }

        public async Task<Result> GetPrimes()
        {
            _logger.LogInformation($"Got parameters from request: [{_settings.PrimeFrom};{_settings.PrimeTo}]");
            //options to write in a human friendly format
            var option = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            //Dumb check if file exists to not append text
            /*if (File.Exists(FileName))
                File.Delete(FileName);*/
            
            //todo: change writing method
            
            //Put here. To split.
            await using var fs = new FileStream(FileName, FileMode.Append);
            _logger.LogInformation($"File created successfully");
            var task = await FindPrimes();

            /*if (task is null)
            {
                _logger.LogError($"Error. Task is null");
                return null;
            }*/
            
            await JsonSerializer.SerializeAsync(fs, task, option);
                
            _logger.LogInformation($"Data has been saved to file {FileName}. Method GetPrimes in class PrimeAlgorithm.cs");
            
            //todo: add something fancy. Logger for example.
            
            return task;

        }

        public Task<bool> IsPrime(int number)
        {
            _logger.LogInformation($"Got parameters from request: [{number}]");
            if (number < 2)
            {
                _logger.LogError($"Exception. Number {number} is less than 2");
                
                return Task.FromResult(false);
            }
                
            var isPrime =
                Enumerable.Range(2, (int)Math.Sqrt(number) - 1)
                    .All(divisor => number % divisor != 0);
            
            _logger.LogInformation($"Algorithm passed. Number {number} is prime? {isPrime}");
            
            return Task.FromResult(isPrime);
        }


        public async Task<Result> FindPrimes()
        {
            return await Task.Run(() =>
            {
                var time = DateTime.UtcNow;
                var primes = new List<int>();
                
                try
                {
                    if (_settings.PrimeFrom == 0)
                        _settings.PrimeFrom = 1;
                    
                    if (_settings.PrimeFrom < 0 || _settings.PrimeTo <= 0)
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
                    _logger.LogInformation("Successfully made everything. Creating new Result object");
                    
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
                    _logger.LogWarning(exception.Message);
                    //todo: add logger
                    return new Result
                    {
                        Success = false,
                        Error = exception.Message,
                        Duration = string.Empty,
                        Primes = null
                    };
                }
            });
        }
    }
}