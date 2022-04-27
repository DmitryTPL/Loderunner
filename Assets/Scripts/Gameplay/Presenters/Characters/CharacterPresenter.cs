using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Loderunner.Service;
using UniTaskPubSub.AsyncEnumerable;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public abstract class CharacterPresenter : Presenter, ICharacterFilter, ICharacter
    {
        private readonly ICharacterStateContext _characterStateContext;
        private readonly ICharacterFallObserver _characterFallObserver;
        private readonly StateData _stateData;
        public event Action<Vector2, float> Moving;
        public event Action<Vector2, float> Climbing;
        public event Action ClimbingFinished;
        public event Action<Vector2, float> Crawling;
        public event Action CrawlingFinished;
        public event Action<Vector2> Falling;

        public abstract int Id { get; set; }
        public bool CanAct { get; private set; }
        public Vector2 Position { get; private set; }

        protected CharacterPresenter(ICharacterStateContext characterStateContext, IAsyncEnumerableReceiver receiver, 
            ICharacterFallObserver characterFallObserver, StateData stateData)
        {
            _characterStateContext = characterStateContext;
            _characterFallObserver = characterFallObserver;
            _stateData = stateData;

            ((ICharacterFilter)_characterFallObserver).Id = Id;

            receiver.Receive<EnterLadderMessage>().Where(m => this.IsCharacterMatch(m.CharacterId)).Subscribe(OnEnterLadder).AddTo(DisposeCancellationToken);
            receiver.Receive<ExitLadderMessage>().Where(m => this.IsCharacterMatch(m.CharacterId)).Subscribe(OnExitLadder).AddTo(DisposeCancellationToken);
            receiver.Receive<BorderReachedMessage>().Where(m => this.IsCharacterMatch(m.CharacterId)).Subscribe(OnBorderReached).AddTo(DisposeCancellationToken);
            receiver.Receive<MovedAwayFromBorderMessage>().Where(m => this.IsCharacterMatch(m.CharacterId)).Subscribe(OnMovedAwayFromBorder).AddTo(DisposeCancellationToken);
            receiver.Receive<EnterCrossbarMessage>().Where(m => this.IsCharacterMatch(m.CharacterId)).Subscribe(OnEnterCrossbar).AddTo(DisposeCancellationToken);
            receiver.Receive<ExitCrossbarMessage>().Where(m => this.IsCharacterMatch(m.CharacterId)).Subscribe(OnExitCrossbar).AddTo(DisposeCancellationToken);
            receiver.Receive<GameStartedMessage>().Subscribe(OnGameStarted).AddTo(DisposeCancellationToken);
        }

        public void UpdateCharacterMoveData(MovingData movingData)
        {
            _stateData.MovingData = movingData;

            Position = movingData.CharacterPosition;
        }

        public void UpdateCharacterState()
        {
            if (!CanAct)
            {
                return;
            }
            
            _characterFallObserver.UpdateFallData(_stateData);
            
            var updatedStateData = _characterStateContext.GetStateData();

            ApplyState(updatedStateData);

            _stateData.PreviousState = updatedStateData.CurrentState;
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

        private void OnGameStarted(GameStartedMessage message)
        {
            CanAct = true;
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