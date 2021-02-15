namespace PM_HW_9.Services
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    /// <summary>
    /// This class takes bool Success, string Error, string Duration, and List of int Primes
    /// </summary>
    public class Result
    {
        //Got from PM_HW_6 rep
        
        /// <summary>
        /// boolean Success
        /// </summary>
        [JsonPropertyName("success")]
        public bool Success { get; init; }
        
        /// <summary>
        /// string Error
        /// </summary>
        [JsonPropertyName("error")]
        public string Error { get; init; }
        
        /// <summary>
        /// string Duration
        /// </summary>
        [JsonPropertyName("duration")]
        public string Duration { get; init; }
        
        /// <summary>
        /// List of int Primes
        /// </summary>
        [JsonPropertyName("primes")]
        public List<int> Primes { get; init; }

        public override string ToString()
        {
            return $"Success: {Success}\n" +
                   $"Duration: {Duration}\n";
        }
    }
}