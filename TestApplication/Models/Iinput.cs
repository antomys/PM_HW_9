using System.Collections.Generic;
using System.Net;

namespace TestApplication.Models
{
    public interface IInput
    {
        string LandingTest { get; set; }
        
        Dictionary<string,HttpStatusCode> IsPrime { get; set; }
        
        Dictionary<string,List<int>> GetPrimes { get; set; }
    }
}