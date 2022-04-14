using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Loderunner.Service;
using UniTaskPubSub.AsyncEnumerable;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public abstract class CharacterPresenter : Presenter, ICharacterFilter
    {
        private readonly ICharacterStateContext _characterStateContext;
        private readonly ICharacterFallObserver _characterFallObserver;
        private readonly StateData _stateData;

        protected readonly CancellationTokenSource _unsubscribeTokenSource = new();

        public event Action<Vector2, float> Moving;
        public event Action<Vector2, float> Climbing;
        public event Action ClimbingFinished;
        public event Action<Vector2, float> Crawling;
        public event Action CrawlingFinished;
        public event Action<Vector2> Falling;

        public abstract int CharacterId { get; set; }

        protected CharacterPresenter(ICharacterStateContext characterStateContext, IAsyncEnumerableReceiver receiver, 
            ICharacterFallObserver characterFallObserver, StateData stateData)
        {
            _characterStateContext = characterStateContext;
            _characterFallObserver = characterFallObserver;
            _stateData = stateData;

            ((ICharacterFilter)_characterFallObserver).CharacterId = CharacterId;

            receiver.Receive<EnterLadderMessage>().Where(m => this.IsCharacterMatch(m.CharacterId)).Subscribe(OnEnterLadder).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<ExitLadderMessage>().Where(m => this.IsCharacterMatch(m.CharacterId)).Subscribe(OnExitLadder).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<BorderReachedMessage>().Where(m => this.IsCharacterMatch(m.CharacterId)).Subscribe(OnBorderReached).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<MovedAwayFromBorderMessage>().Where(m => this.IsCharacterMatch(m.CharacterId)).Subscribe(OnMovedAwayFromBorder).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<EnterCrossbarMessage>().Where(m => this.IsCharacterMatch(m.CharacterId)).Subscribe(OnEnterCrossbar).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<ExitCrossbarMessage>().Where(m => this.IsCharacterMatch(m.CharacterId)).Subscribe(OnExitCrossbar).AddTo(_unsubscribeTokenSource.Token);
        }

        public void UpdateCharacterMoveData(MovingData movingData)
        {
            _stateData.MovingData = movingData;
        }

        public void UpdateCharacterState()
        {
            _characterFallObserver.UpdateFallData(_stateData);
            
            var updatedStateData = _characterStateContext.GetStateData();

            ApplyState(updatedStateData);

            _stateData.PreviousState = updatedStateData.CurrentState;
        }

        public override void Dispose()
        {
            _unsubscribeTokenSource?.Dispose();
        }

        protected virtual void ApplyState(UpdatedStateData updatedStateData)
        {
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

                _characterFallObserver.BeginToFallFromCrossbar(playerPosition.x);

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