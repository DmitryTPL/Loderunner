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

        protected override void Awake()
        {
            base.Awake();
            
            CharacterId = -1;
        }

        protected override void PresenterAttached()
        {
            base.PresenterAttached();
            
            _presenter.BlockRemoving += OnBlockRemoving;
            _presenter.BlockRemoved += OnBlockRemoved;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            _presenter.BlockRemoving -= OnBlockRemoving;
            _presenter.BlockRemoved -= OnBlockRemoved;
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

            _presenter.UpdateCharacterMoveData(new MovingData(horizontalMove, verticalMove, transform.position));
            _presenter.UpdatePlayerRemovingBlock(removeBlockType, transform.position);
            _presenter.UpdateCharacterState();
        }
        
        private void OnBlockRemoving(Vector2 newPosition, RemoveBlockType removeBlockType)
        {
            _animationHandler.ApplyAnimation(new CharacterRemoveBlockAnimationAction());
            
            SetLookDirection(removeBlockType == RemoveBlockType.Right ? 1 : -1);
            
            _rigidbody.MovePosition(newPosition);
        }

        private void OnBlockRemoved()
        {
            _animationHandler.ApplyAnimation(new CharacterRemoveBlockFinishedAnimationAction());
        }
    }
}