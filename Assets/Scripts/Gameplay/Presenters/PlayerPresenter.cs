using System;
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

        private CharacterState _currentState;
        private BorderType _borderType;

        public event Action<Vector3> Moving;
        public event Action<Vector3> Climbing;
        public event Action ClimbingFinished;

        public CharacterConfig PlayerConfig { get; }
        public GameConfig GameConfig { get; }
        public ClimbingData ClimbingData { get; private set; }

        public PlayerPresenter(CharacterConfig playerConfig, ICharacterStateContext characterStateContext, GameConfig gameConfig, IAsyncSubscriber subscriber)
        {
            _characterStateContext = characterStateContext;
            PlayerConfig = playerConfig;
            GameConfig = gameConfig;

            subscriber.Subscribe<PlayerEnterLadderMessage>(OnPlayerEnterLadder);
            subscriber.Subscribe<PlayerExitLadderMessage>(OnPlayerExitLadder);
            subscriber.Subscribe<BorderReachedMessage>(OnBorderReached);
            subscriber.Subscribe<MovedAwayFromBorderMessage>(OnMovedAwayFromBorder);
        }

        public void UpdateCharacterData(MovingData movingData)
        {
            var data = _characterStateContext.GetStateData(new StateInitialData(movingData, PlayerConfig,
                ClimbingData, _currentState, _borderType));

            //this.Log(data.CurrentState.ToString());

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
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _currentState = data.CurrentState;
        }

        private UniTask OnPlayerEnterLadder(PlayerEnterLadderMessage message, CancellationToken cancellationToken)
        {
            if (message.CharacterView is not PlayerView)
            {
                return UniTask.CompletedTask;
            }

            ClimbingData = message.Data;

            return UniTask.CompletedTask;
        }

        private UniTask OnPlayerExitLadder(PlayerExitLadderMessage message, CancellationToken cancellationToken)
        {
            if (message.CharacterView is not PlayerView)
            {
                return UniTask.CompletedTask;
            }
            
            ClimbingData = new ClimbingData();

            return UniTask.CompletedTask;
        }

        private UniTask OnBorderReached(BorderReachedMessage message, CancellationToken cancellationToken)
        {
            if (message.CharacterView is not PlayerView)
            {
                return UniTask.CompletedTask;
            }
            
            _borderType = message.BorderType;
            
            return UniTask.CompletedTask;
        }

        private UniTask OnMovedAwayFromBorder(MovedAwayFromBorderMessage message, CancellationToken cancellationToken)
        {
            if (message.CharacterView is not PlayerView)
            {
                return UniTask.CompletedTask;
            }
            
            _borderType = BorderType.None;
            
            return UniTask.CompletedTask;
        }
    }
}