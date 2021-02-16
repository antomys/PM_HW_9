using System;
using System.Collections.Generic;

namespace TestApplication.Models
{
    public interface IInput
    {
        string LandingTest { get; set; }
        
        Dictionary<string,bool> IsPrime { get; set; }
        
        Dictionary<string,List<int>> GetPrimes { get; set; }
    }
}