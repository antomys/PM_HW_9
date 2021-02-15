using System;

namespace PM_HW_9.Exceptions
{
    public class ArgumentOutOfRange : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ArgumentOutOfRange()
        {
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Exception details</param>
        public ArgumentOutOfRange(string message) 
            : base(message)
        {
        }
    }
}