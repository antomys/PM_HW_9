using System.Threading.Tasks;

namespace PM_HW_9.Services.Interfaces
{
    public interface IPrimeAlgorithm
    {
        Task<Result> FindPrimes();
        Task<Result> GetPrimes();

        Task<bool> IsPrime(int number);
    }
}