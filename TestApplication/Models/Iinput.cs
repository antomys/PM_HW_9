using System;
using System.Collections.Generic;
using System.Net;

namespace TestApplication.Models
{
    public interface IInput
    {
        Uri LandingTest { get; set; }
        
        Dictionary<string,HttpStatusCode> IsPrime { get; set; }
        
        Dictionary<string,List<int>> GetPrimes { get; set; }
    }
}