using System;

namespace Loderunner.Service
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// Return Unix Timestamp in seconds
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static double ToTimestamp(this DateTime origin)
        {
            return DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}