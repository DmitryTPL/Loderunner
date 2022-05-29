using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UniTaskPubSub.AsyncEnumerable;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class GuardianPresenter : CharacterPresenter
    {
        private static readonly Vector2Int _undefinedMapPosition = new Vector2Int(-1, -1);
        private readonly IGuardiansCommander _guardiansCommander;

        private Stack<Vector2Int> _pathToPlayer = new();

        private Vector2Int _mapPosition = _undefinedMapPosition;

        public override CharacterType CharacterType => CharacterType.Guardian;

        public GuardianPresenter(GuardianStateContext stateContext, IAsyncEnumerableReceiver receiver, IAsyncEnumerablePublisher publisher,
            ICharacterFallObserver characterFallObserver, IGuardiansCommander guardiansCommander)
            : base(stateContext, receiver, publisher, characterFallObserver, stateContext.StateData)
        {
            _guardiansCommander = guardiansCommander;

            receiver.Receive<UpdateGuardiansPathMessage>().Subscribe(OnUpdatePath);
        }

        public override void CharacterCreated(int id)
        {
            base.CharacterCreated(id);

            _guardiansCommander.Register(id);
        }

        public (int HorizontalDirection, int VerticalDirection) GetDirection()
        {
            if (_pathToPlayer.Count == 0)
            {
                return (0, 0);
            }

            var nextPointOnPath = _pathToPlayer.Peek();
            
            var direction = nextPointOnPath - _mapPosition;

            if (IsPointReached(direction, nextPointOnPath))
            {
                _pathToPlayer.Pop();

                if (_pathToPlayer.Count == 0)
                {
                    return (0, 0);
                }

                _mapPosition = nextPointOnPath;

                return (0, 0);
            }

            return (direction.x, direction.y);
        }

        private bool IsPointReached(Vector2Int direction, Vector2Int goalPosition)
        {
            var shiftedPosition = Position - new Vector2(0.5f, 0);

            var xCoordinateReached = (goalPosition.x - shiftedPosition.x) * direction.x < float.Epsilon;
            var yCoordinateReached = (goalPosition.y - shiftedPosition.y) * direction.y < float.Epsilon;
            
            return xCoordinateReached && yCoordinateReached;
        }

        private async UniTaskVoid OnUpdatePath(UpdateGuardiansPathMessage message)
        {
            if (_mapPosition == _undefinedMapPosition)
            {
                _mapPosition = _guardiansCommander.FindPositionOnMap(Position);
            }
            
            var findPathResult = await _guardiansCommander.GetPath(Id, _mapPosition);

            if (findPathResult.SearchResult == SearchPathResult.Success)
            {
                _pathToPlayer = findPathResult.Path;
            }
            else
            {
                this.LogError($"Can't find path to player; guardian: {Id}");
            }
        }
    }
}