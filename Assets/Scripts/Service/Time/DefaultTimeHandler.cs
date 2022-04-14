using System;

namespace Loderunner.Service
{
    public class DefaultTimeHandler: ITime
    {
        public double Now => DateTime.Now.ToTimestamp();
    }
}