using System;

namespace eathappy.order.domain.Common
{
    public static class Precondition
    {
        public static void IsNotNull<T>(T input)
            where T : class
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
        }
    }
}
