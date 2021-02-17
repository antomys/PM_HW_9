namespace PM_HW_9.Services
{
    public class Settings : ISettings
    {
        public Settings()
        {
            
        }

        public Settings(int primeFrom, int primeTo)
        {
            PrimeFrom = primeFrom;
            PrimeTo = primeTo;
        }
        public int PrimeFrom { get; set; }
        public int PrimeTo { get; set; }
    }
}