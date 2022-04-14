using System;

namespace Loderunner.Install
{
    public interface IFactory<out T>
        where T: IDisposable
    {
        T Create();
    }
}