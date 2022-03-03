using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Loderunner.Service;
using UniTaskPubSub;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class PlayerPresenter : Presenter
    {
        private readonly ICharacterStateContext _characterStateContext;
        private readonly IFallPointHolder _fallPointHolder;

        private CharacterState _currentState;
        private BorderType _borderType;
        private bool _isNearLadder;
        private CancellationTokenSource _unsubscribeTokenSource;

        private HashSet<int> _enteredGroundColliders = new ();

        public event Action<Vector3> Moving;
        public event Action<Vector3> Climbing;
        public event Action ClimbingFinished;
        public event Action<Vector3> Falling;

        public CharacterConfig PlayerConfig { get; }
        public GameConfig GameConfig { get; }
        public ClimbingData ClimbingData { get; private set; }

        public PlayerPresenter(CharacterConfig playerConfig, ICharacterStateContext characterStateContext, GameConfig gameConfig, IAsyncSubscriber subscriber,
            IFallPointHolder fallPointHolder)
        {
            _characterStateContext = characterStateContext;
            _fallPointHolder = fallPointHolder;
            
            PlayerConfig = playerConfig;
            GameConfig = gameConfig;

            subscriber.Subscribe<EnterLadderMessage>(OnEnterLadder).AddTo(_unsubscribeTokenSource.Token);
            subscriber.Subscribe<ExitLadderMessage>(OnExitLadder).AddTo(_unsubscribeTokenSource.Token);
            subscriber.Subscribe<BorderReachedMessage>(OnBorderReached).AddTo(_unsubscribeTokenSource.Token);
            subscriber.Subscribe<MovedAwayFromBorderMessage>(OnMovedAwayFromBorder).AddTo(_unsubscribeTokenSource.Token);
            subscriber.Subscribe<GotOffTheFloorMessage>(GotOffTheFloor).AddTo(_unsubscribeTokenSource.Token);
            subscriber.Subscribe<FloorReachedMessage>(OnFloorReached).AddTo(_unsubscribeTokenSource.Token);
        }

        public void UpdateCharacterData(MovingData movingData)
        {
            var data = _characterStateContext.GetStateData(new StateInitialData(movingData, PlayerConfig,
                ClimbingData, _currentState, _borderType, _enteredGroundColliders.Count > 0, _fallPointHolder.FallPoint));

            switch (data.CurrentState)
            {
                case CharacterState.Moving:
                    if (_currentState == CharacterState.LadderClimbing)
                    {
                        ClimbingFinished?.Invoke();
                    }

                    Moving?.Invoke(data.NextCharacterPosition);
                    break;
                case CharacterState.LadderClimbing:
                    Climbing?.Invoke(data.NextCharacterPosition);
                    break;
                case CharacterState.CrossbarCrawling:
                    break;
                case CharacterState.Falling:
                    Falling?.Invoke(data.NextCharacterPosition);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _currentState = data.CurrentState;
        }

        public override void Destroy()
        {
            base.Destroy();
            
            _unsubscribeTokenSource.Cancel();
        }

        private UniTask OnEnterLadder(EnterLadderMessage message)
        {
            if (message.CharacterView is not PlayerView)
            {
                return UniTask.CompletedTask;
            }

            ClimbingData = message.Data;

            return UniTask.CompletedTask;
        }

        private UniTask OnExitLadder(ExitLadderMessage message)
        {
            if (message.CharacterView is not PlayerView)
            {
                return UniTask.CompletedTask;
            }

            ClimbingData = new ClimbingData();

            return UniTask.CompletedTask;
        }

        private UniTask OnBorderReached(BorderReachedMessage message)
        {
            if (message.CharacterView is not PlayerView)
            {
                return UniTask.CompletedTask;
            }
            
            _borderType = message.BorderType;
            
            return UniTask.CompletedTask;
        }

        private UniTask OnMovedAwayFromBorder(MovedAwayFromBorderMessage message)
        {
            if (message.CharacterView is not PlayerView)
            {
                return UniTask.CompletedTask;
            }
            
            _borderType = BorderType.None;
            
            return UniTask.CompletedTask;
        }

        private UniTask GotOffTheFloor(GotOffTheFloorMessage message)
        {
            if (message.CharacterView is not PlayerView)
            {
                return UniTask.CompletedTask;
            }

            _enteredGroundColliders.Remove(message.ColliderId);
            
            return UniTask.CompletedTask;
        }

        private UniTask OnFloorReached(FloorReachedMessage message)
        {
            if (message.CharacterView is not PlayerView)
            {
                return UniTask.CompletedTask;
            }

            _enteredGroundColliders.Add(message.ColliderId);
            
            return UniTask.CompletedTask;
        }
    }
}