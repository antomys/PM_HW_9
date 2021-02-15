using System;

namespace PM_HW_9.Exceptions
{
    public class ArgumentNull : ArgumentOutOfRange
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ArgumentNull()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception details.</param>
        public ArgumentNull(string message) 
            : base(message)
        {
        }
    }
}