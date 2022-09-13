using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class PlayerView : CharacterView<PlayerPresenter>
    {
        private const string HorizontalAxisName = "Horizontal";
        private const string VerticalAxisName = "Vertical";
        private const string RemoveBlockLeftAxisName = "RemoveBlockLeft";
        private const string RemoveBlockRightAxisName = "RemoveBlockRight";

        public override CharacterType CharacterType => CharacterType.Player;

        private Action _cachedEventHandler;

        protected override void Awake()
        {
            base.Awake();
            
            CharacterId = -1;
        }

        protected override void PresenterAttached()
        {
            base.PresenterAttached();

            _cachedEventHandler = UniTask.Action(OnCached);
            
            _presenter.BlockRemoving += OnBlockRemoving;
            _presenter.BlockRemoved += OnBlockRemoved;
            _presenter.Cached += _cachedEventHandler;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            _presenter.BlockRemoving -= OnBlockRemoving;
            _presenter.BlockRemoved -= OnBlockRemoved;
            _presenter.Cached -= _cachedEventHandler;
        }

        private void FixedUpdate()
        {
            var horizontalMove = Input.GetAxis(HorizontalAxisName);
            var verticalMove = Input.GetAxis(VerticalAxisName);

            var removeBlockType = RemoveBlockType.None;

            if (Input.GetAxis(RemoveBlockLeftAxisName) > float.Epsilon)
            {
                removeBlockType = RemoveBlockType.Left;
            }
            else if (Input.GetAxis(RemoveBlockRightAxisName) > float.Epsilon)
            {
                removeBlockType = RemoveBlockType.Right;
            }

            var position = transform.position;
            
            _presenter.UpdateCharacterMoveData(new MovingData(horizontalMove, verticalMove, position));
            _presenter.UpdatePlayerRemovingBlock(removeBlockType, position);
            _presenter.UpdateCharacterState();
        }
        
        private void OnBlockRemoving(Vector2 newPosition, RemoveBlockType removeBlockType)
        {
            _animationHandler.ApplyAnimation(new CharacterRemoveBlockAnimationAction()).Forget();
            
            SetLookDirection(removeBlockType == RemoveBlockType.Right ? 1 : -1);
            
            _rigidbody.MovePosition(newPosition);
        }

        private void OnBlockRemoved()
        {
            _animationHandler.ApplyAnimation(new CharacterRemoveBlockFinishedAnimationAction()).Forget();
        }

        private async UniTaskVoid OnCached()
        {
            await _animationHandler.ApplyAnimation(new PlayerDiedAnimationAction());

            _presenter.PlayerDeathFinished();
        }
    }
}