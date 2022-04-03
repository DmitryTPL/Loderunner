using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Loderunner.Service;
using UniTaskPubSub.AsyncEnumerable;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public abstract class CharacterPresenter : Presenter
    {
        private readonly ICharacterStateContext _characterStateContext;
        private readonly IFallPointHolder _fallPointHolder;
        private readonly IFloorPointHolder _floorPointHolder;
        private readonly StateData _stateData;

        private readonly CancellationTokenSource _unsubscribeTokenSource = new();

        public event Action<Vector3, float> Moving;
        public event Action<Vector3, float> Climbing;
        public event Action ClimbingFinished;
        public event Action<Vector3, float> Crawling;
        public event Action CrawlingFinished;
        public event Action<Vector3> Falling;

        public abstract int CharacterId { get; }

        protected CharacterPresenter(ICharacterStateContext characterStateContext, IAsyncEnumerableReceiver receiver, 
            IFallPointHolder fallPointHolder, IFloorPointHolder floorPointHolder, StateData stateData)
        {
            _characterStateContext = characterStateContext;
            _fallPointHolder = fallPointHolder;
            _floorPointHolder = floorPointHolder;
            _stateData = stateData;

            bool IsCharacter(int characterId)
            {
                return characterId == CharacterId;
            }

            ((ICharacterFilter)_fallPointHolder).CharacterFilter = IsCharacter;

            receiver.Receive<EnterLadderMessage>().Where(m => IsCharacter(m.CharacterId)).Subscribe(OnEnterLadder).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<ExitLadderMessage>().Where(m => IsCharacter(m.CharacterId)).Subscribe(OnExitLadder).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<BorderReachedMessage>().Where(m => IsCharacter(m.CharacterId)).Subscribe(OnBorderReached).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<MovedAwayFromBorderMessage>().Where(m => IsCharacter(m.CharacterId)).Subscribe(OnMovedAwayFromBorder).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<EnterCrossbarMessage>().Where(m => IsCharacter(m.CharacterId)).Subscribe(OnEnterCrossbar).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<ExitCrossbarMessage>().Where(m => IsCharacter(m.CharacterId)).Subscribe(OnExitCrossbar).AddTo(_unsubscribeTokenSource.Token);
        }

        public void UpdateCharacterMoveData(MovingData movingData)
        {
            _stateData.MovingData = movingData;
        }

        public void UpdateCharacterState()
        {
            _fallPointHolder.UpdateFallData(_stateData);
            _floorPointHolder.UpdateFloorData(_stateData);
            
            var updatedStateData = _characterStateContext.GetStateData();

            switch (updatedStateData.CurrentState)
            {
                case CharacterState.Moving:
                    ResetPlayerActivities(_stateData.MovingData.CharacterPosition);
                    Moving?.Invoke(updatedStateData.NextCharacterPosition, updatedStateData.MoveSpeed);
                    break;
                case CharacterState.LadderClimbing:
                    Climbing?.Invoke(updatedStateData.NextCharacterPosition, updatedStateData.MoveSpeed);
                    break;
                case CharacterState.CrossbarCrawling:
                    Crawling?.Invoke(updatedStateData.NextCharacterPosition, updatedStateData.MoveSpeed);
                    break;
                case CharacterState.Falling:
                    ResetPlayerActivities(_stateData.MovingData.CharacterPosition);
                    Falling?.Invoke(updatedStateData.NextCharacterPosition);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _stateData.PreviousState = updatedStateData.CurrentState;
        }

        public override void Dispose()
        {
            _unsubscribeTokenSource?.Dispose();
        }

        private void ResetPlayerActivities(Vector2 playerPosition)
        {
            if (_stateData.PreviousState == CharacterState.LadderClimbing)
            {
                ClimbingFinished?.Invoke();
            }

            if (_stateData.PreviousState == CharacterState.CrossbarCrawling)
            {
                CrawlingFinished?.Invoke();

                _fallPointHolder.BeginToFallFromCrossbar(playerPosition.x);

                _stateData.CrawlingData = new CrawlingData();
            }
        }

        private void OnEnterLadder(EnterLadderMessage message)
        {
            _stateData.ClimbingData = message.Data;
        }

        private void OnExitLadder(ExitLadderMessage message)
        {
            _stateData.ClimbingData = new ClimbingData();
        }

        private void OnBorderReached(BorderReachedMessage message)
        {
            _stateData.BorderReachedType |= message.Border;
        }

        private void OnMovedAwayFromBorder(MovedAwayFromBorderMessage message)
        {
            _stateData.BorderReachedType &= ~message.Border;
        }

        private void OnEnterCrossbar(EnterCrossbarMessage message)
        {
            _stateData.CrawlingData = message.CrawlingData;
        }

        private void OnExitCrossbar(ExitCrossbarMessage message)
        {
            _stateData.CrawlingData = new CrawlingData(true);
        }
    }
}