namespace PM_HW_9.Services
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// This class takes bool Success, string Error, string Duration, and List of int Primes
    /// </summary>
    public class Result
    {
        //Got from PM_HW_6 rep
        
        /// <summary>
        /// boolean Success
        /// </summary>
        [JsonProperty("success")]
        public bool Success { get; init; }
        
        /// <summary>
        /// string Error
        /// </summary>
        [JsonProperty("error")]
        public string Error { get; init; }
        
        /// <summary>
        /// string Duration
        /// </summary>
        [JsonProperty("duration")]
        public string Duration { get; init; }
        
        /// <summary>
        /// List of int Primes
        /// </summary>
        [JsonProperty("primes")]
        public List<int> Primes { get; init; }
    }
}