using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UniTaskPubSub.AsyncEnumerable;

namespace Loderunner.Gameplay
{
    public class GuardiansIdPool : IGuardiansIdPool, IDisposable
    {
        private readonly CancellationTokenSource _disposeCancellationTokenSource = new();
        private CancellationToken DisposeCancellationToken => _disposeCancellationTokenSource.Token;

        private int _maxId;
        private readonly Stack<int> _identifiers = new();

        public GuardiansIdPool(IAsyncEnumerableReceiver receiver)
        {
            receiver.Receive<LevelCreatedMessage>().Subscribe(OnLevelCreated).AddTo(DisposeCancellationToken);
        }

        private void OnLevelCreated(LevelCreatedMessage obj)
        {
            _maxId = 0;
            _identifiers.Clear();
        }

        public int GetId()
        {
            return _identifiers.Count == 0 ? _maxId++ : _identifiers.Pop();
        }

        public void ReturnId(int id)
        {
            _identifiers.Push(id);
        }

        public void Dispose()
        {
            _disposeCancellationTokenSource?.Dispose();
        }
    }
}