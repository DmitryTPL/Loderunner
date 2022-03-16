using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Loderunner.Service;
using UniTaskPubSub.AsyncEnumerable;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class PlayerPresenter : Presenter
    {
        private readonly ICharacterStateContext _characterStateContext;
        private readonly IFallPointHolder _fallPointHolder;
        private readonly IFloorPointHolder _floorPointHolder;

        private ClimbingData _climbingData;
        private CrawlingData _crawlingData;

        private CharacterState _currentState;
        private BorderType _borderType;
        private bool _isNearLadder;
        private CancellationTokenSource _unsubscribeTokenSource = new();

        public event Action<Vector3, float> Moving;
        public event Action<Vector3, float> Climbing;
        public event Action ClimbingFinished;
        public event Action<Vector3, float> Crawling;
        public event Action CrawlingFinished;
        public event Action<Vector3> Falling;

        public CharacterConfig PlayerConfig { get; }
        public GameConfig GameConfig { get; }
        public int CharacterId => 1;

        public PlayerPresenter(CharacterConfig playerConfig, ICharacterStateContext characterStateContext,
            GameConfig gameConfig, IAsyncEnumerableReceiver receiver, IFallPointHolder fallPointHolder, 
            IFloorPointHolder floorPointHolder)
        {
            _characterStateContext = characterStateContext;
            _fallPointHolder = fallPointHolder;
            _floorPointHolder = floorPointHolder;

            bool IsCharacter(int characterId)
            {
                return characterId == CharacterId;
            }

            ((ICharacterFilter)_fallPointHolder).CharacterFilter = IsCharacter;

            PlayerConfig = playerConfig;
            GameConfig = gameConfig;

            receiver.Receive<EnterLadderMessage>().Where(m => IsCharacter(m.CharacterId)).Subscribe(OnEnterLadder).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<ExitLadderMessage>().Where(m => IsCharacter(m.CharacterId)).Subscribe(OnExitLadder).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<BorderReachedMessage>().Where(m => IsCharacter(m.CharacterId)).Subscribe(OnBorderReached).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<MovedAwayFromBorderMessage>().Where(m => IsCharacter(m.CharacterId)).Subscribe(OnMovedAwayFromBorder).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<EnterCrossbarMessage>().Where(m => IsCharacter(m.CharacterId)).Subscribe(OnEnterCrossbar).AddTo(_unsubscribeTokenSource.Token);
            receiver.Receive<ExitCrossbarMessage>().Where(m => IsCharacter(m.CharacterId)).Subscribe(OnExitCrossbar).AddTo(_unsubscribeTokenSource.Token);
        }

        public void UpdateCharacterData(MovingData movingData)
        {
            var data = _characterStateContext.GetStateData(new StateInitialData(movingData, PlayerConfig,
                _climbingData, _crawlingData, _currentState, _borderType, 
                _fallPointHolder.IsGrounded, _fallPointHolder.FallPoint, _floorPointHolder.FloorPoint));

            switch (data.CurrentState)
            {
                case CharacterState.Moving:
                    ResetPlayerActivities(movingData.CharacterPosition);
                    Moving?.Invoke(data.NextCharacterPosition, data.MoveSpeed);
                    break;
                case CharacterState.LadderClimbing:
                    Climbing?.Invoke(data.NextCharacterPosition, data.MoveSpeed);
                    break;
                case CharacterState.CrossbarCrawling:
                    Crawling?.Invoke(data.NextCharacterPosition, data.MoveSpeed);
                    break;
                case CharacterState.Falling:
                    ResetPlayerActivities(movingData.CharacterPosition);
                    Falling?.Invoke(data.NextCharacterPosition);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _currentState = data.CurrentState;
        }

        public override void Dispose()
        {
            _unsubscribeTokenSource?.Dispose();
        }

        private void ResetPlayerActivities(Vector2 playerPosition)
        {
            if (_currentState == CharacterState.LadderClimbing)
            {
                ClimbingFinished?.Invoke();
            }

            if (_currentState == CharacterState.CrossbarCrawling)
            {
                CrawlingFinished?.Invoke();
                
                _fallPointHolder.BeginToFallFromCrossbar(playerPosition.x);

                _crawlingData = new CrawlingData();
            }
        }

        private void OnEnterLadder(EnterLadderMessage message)
        {
            _climbingData = message.Data;
        }

        private void OnExitLadder(ExitLadderMessage message)
        {
            _climbingData = new ClimbingData();
        }

        private void OnBorderReached(BorderReachedMessage message)
        {
            _borderType |= message.Border;
        }

        private void OnMovedAwayFromBorder(MovedAwayFromBorderMessage message)
        {
            _borderType &= ~message.Border;
        }

        private void OnEnterCrossbar(EnterCrossbarMessage message)
        {
            _crawlingData = message.CrawlingData;
        }

        private void OnExitCrossbar(ExitCrossbarMessage message)
        {
            _crawlingData = new CrawlingData(true);
        }
    }
}