using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UniTaskPubSub.AsyncEnumerable;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class GuardiansCommanderFacade : IDisposable, IGuardiansCommander
    {
        private readonly CancellationTokenSource _unsubscribeTokenSource = new();

        private readonly IAsyncEnumerablePublisher _publisher;
        private readonly IPathFinder _pathFinder;
        private readonly ILevelData _levelData;
        private readonly Dictionary<int, GuardianFindPlayerType> _guardians = new();

        private Vector2Int _playerPosition;

        public GuardiansCommanderFacade(IAsyncEnumerableReceiver receiver, IAsyncEnumerablePublisher publisher, IPathFinder pathFinder,
            ILevelData levelData)
        {
            _publisher = publisher;
            _pathFinder = pathFinder;
            _levelData = levelData;

            receiver.Receive<PlayerMovedMessage>().Subscribe(OnPlayerMoved).AddTo(_unsubscribeTokenSource.Token);
        }

        public void Register(int id)
        {
            _guardians[id] = GuardianFindPlayerType.ShortPath;
        }

        public async UniTask<PathResult> GetPath(int id, Vector2Int mapPosition)
        {
            return await _pathFinder.GetPath(_levelData.Map, mapPosition, _playerPosition);
        }

        public void Dispose()
        {
            _unsubscribeTokenSource.Dispose();
        }

        private void OnPlayerMoved(PlayerMovedMessage message)
        {
            var newPlayerPosition = FindPositionOnMap(message.Position);

            if (newPlayerPosition != _playerPosition)
            {
                _playerPosition = newPlayerPosition;

                _publisher.Publish(new UpdateGuardiansPathMessage());
            }
        }

        public Vector2Int FindPositionOnMap(Vector2 position)
        {
            return new Vector2Int((int)Math.Floor(position.x), (int)Math.Floor(position.y));
        }
    }
}