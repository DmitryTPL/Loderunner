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

        private bool _isClimbing;
        
        public event Action<float> Moving; 
        public event Action<float> Climbing; 
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
        }

        public void UpdateCharacterData(float horizontalMove, float verticalMove, Vector3 playerPosition)
        {
            var data = _characterStateContext.GetStateData(new StateInitialData(horizontalMove, verticalMove, PlayerConfig, 
                ClimbingData, playerPosition, _isClimbing));

            this.Log(data.CurrentState.ToString());
            
            switch (data.CurrentState)
            {
                case State.Idle:
                case State.Moving:
                    Moving?.Invoke(data.MovementValue);
                    break;
                case State.LadderClimbing:
                    _isClimbing = true;
                    Climbing?.Invoke(data.MovementValue);
                    break;
                case State.LadderClimbingIdle:
                    Climbing?.Invoke(data.MovementValue);
                    break;
                case State.LadderClimbingFinished:
                    _isClimbing = false;
                    ClimbingFinished?.Invoke();
                    break;
                case State.CrossbarCrawling:
                    break;
                case State.CrossbarCrawlingIdle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private UniTask OnPlayerEnterLadder(PlayerEnterLadderMessage message, CancellationToken cancellationToken)
        {
            this.Log("OnPlayerEnterLadder");
            
            ClimbingData = message.Data;
            
            return UniTask.CompletedTask;
        }

        private UniTask OnPlayerExitLadder(PlayerExitLadderMessage message, CancellationToken cancellationToken)
        {
            ClimbingData = new ClimbingData();
            
            return UniTask.CompletedTask;
        }
    }
}