using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Loderunner.Service;
using UniTaskPubSub.AsyncEnumerable;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public abstract class CharacterPresenter : Presenter, ICharacter
    {
        private readonly ICharacterStateContext _characterStateContext;
        private readonly StateData _stateData;

        protected readonly ICharacterFallObserver _characterFallObserver;
        protected readonly IAsyncEnumerableReceiver _receiver;
        protected readonly IAsyncEnumerablePublisher _publisher;

        public event Action<Vector2, float> Moving;
        public event Action<Vector2, float> Climbing;
        public event Action ClimbingFinished;
        public event Action<Vector2, float> Crawling;
        public event Action CrawlingFinished;
        public event Action<Vector2> Falling;

        public int Id { get; private set; }
        public abstract CharacterType CharacterType { get; }
        public Vector2 Position { get; set; }
        
        protected bool CanAct { get; set; }

        protected CharacterPresenter(ICharacterStateContext characterStateContext, IAsyncEnumerableReceiver receiver,
            IAsyncEnumerablePublisher publisher, ICharacterFallObserver characterFallObserver, StateData stateData)
        {
            _characterStateContext = characterStateContext;
            _receiver = receiver;
            _publisher = publisher;
            _characterFallObserver = characterFallObserver;
            _stateData = stateData;
        }

        public virtual void CharacterCreated(int id)
        {
            Id = id;
            _publisher.Publish(new CharacterCreatedMessage(Id, CharacterType));

            _characterFallObserver.BindCharacter(Id);

            _receiver.Receive<EnterLadderMessage>().Where(m => m.IsCharacterMatch(Id)).Subscribe(OnEnterLadder).AddTo(DisposeCancellationToken);
            _receiver.Receive<ExitLadderMessage>().Where(m => m.IsCharacterMatch(Id)).Subscribe(OnExitLadder).AddTo(DisposeCancellationToken);
            _receiver.Receive<BorderReachedMessage>().Where(m => m.IsCharacterMatch(Id)).Subscribe(OnBorderReached).AddTo(DisposeCancellationToken);
            _receiver.Receive<MovedAwayFromBorderMessage>().Where(m => m.IsCharacterMatch(Id)).Subscribe(OnMovedAwayFromBorder).AddTo(DisposeCancellationToken);
            _receiver.Receive<EnterCrossbarMessage>().Where(m => m.IsCharacterMatch(Id)).Subscribe(OnEnterCrossbar).AddTo(DisposeCancellationToken);
            _receiver.Receive<ExitCrossbarMessage>().Where(m => m.IsCharacterMatch(Id)).Subscribe(OnExitCrossbar).AddTo(DisposeCancellationToken);
            _receiver.Receive<GameStartedMessage>().Subscribe(OnGameStarted).AddTo(DisposeCancellationToken);
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
                    ResetCharacterActivities(_stateData.MovingData.CharacterPosition);
                    Moving?.Invoke(updatedStateData.NextCharacterPosition, updatedStateData.MoveSpeed);
                    break;
                case CharacterState.LadderClimbing:
                    Climbing?.Invoke(updatedStateData.NextCharacterPosition, updatedStateData.MoveSpeed);
                    break;
                case CharacterState.CrossbarCrawling:
                    Crawling?.Invoke(updatedStateData.NextCharacterPosition, updatedStateData.MoveSpeed);
                    break;
                case CharacterState.Falling:
                    ResetCharacterActivities(_stateData.MovingData.CharacterPosition);
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

        private void ResetCharacterActivities(Vector2 playerPosition)
        {
            if (_stateData.PreviousState == CharacterState.LadderClimbing)
            {
                FinishClimbing();
            }

            if (_stateData.PreviousState == CharacterState.CrossbarCrawling)
            {
                CrawlingFinished?.Invoke();

                _characterFallObserver.BeginToFallFromCrossbar(playerPosition.x);

                _stateData.CrawlingData = new CrawlingData();
            }
        }

        protected virtual void FinishClimbing()
        {
            ClimbingFinished?.Invoke();
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