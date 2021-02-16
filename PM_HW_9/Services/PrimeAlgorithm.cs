using System;
using System.Collections.Generic;
using System.Linq;
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
        
        /// <summary>
        /// Returns async list of primes found in a range
        /// </summary>
        /// <returns>List of primes</returns>
        public async Task<Result> GetPrimes()
        {
            _logger.LogInformation($"Got parameters from request: [{_settings.PrimeFrom};{_settings.PrimeTo}]");
            
            var task = await FindPrimes();
            
            if (task is null)
            {
                _logger.LogError($"Error. Task is null");
                return null;
            }

            _logger.LogInformation($"Method GetPrimes in class PrimeAlgorithm.cs Operated successfully");

            return task;

        }

        /// <summary>
        /// Checks if given number is a prime
        /// </summary>
        /// <param name="number">input number</param>
        /// <returns>boolean</returns>
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

        /// <summary>
        /// internal method to find primes
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRange"> If given range does not satisfy given algorithm</exception>
        /// <exception cref="ArgumentNull">if file settings is null. OBSOLETE FROM NOW</exception>
        private async Task<Result> FindPrimes()
        {
            return await Task.Run(() =>
            {
                var time = DateTime.UtcNow;
                var primes = new List<int>();
                
                try
                {
                    if (_settings.PrimeFrom <= 0)
                        _settings.PrimeFrom = 1;
                    
                    if (_settings.PrimeFrom < 0 || _settings.PrimeTo <= 0
                                                ||_settings.PrimeFrom > _settings.PrimeTo)
                    {
                        throw new ArgumentOutOfRange($"Out of range exception at Prime" +
                                                     $"\nAlgorithm method. Line 28");
                    }

                    if (_settings == null )
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