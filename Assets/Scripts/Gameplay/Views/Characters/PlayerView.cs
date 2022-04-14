using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class PlayerView : View<PlayerPresenter>, ICharacterInfo
    {
        [SerializeField] private AnimationHandler _animationHandler;
        [SerializeField] private CharacterType _characterType;

        private Rigidbody2D _rigidbody;

        public CharacterType CharacterType => _characterType;
        public int CharacterId => _presenter.CharacterId;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _presenter.Moving += OnMoving;
            _presenter.Climbing += OnClimbing;
            _presenter.ClimbingFinished += OnClimbingFinished;
            _presenter.Crawling += OnCrawling;
            _presenter.CrawlingFinished += OnCrawlingFinished;
            _presenter.Falling += OnFalling;
            _presenter.BlockRemoving += OnBlockRemoving;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _presenter.Moving -= OnMoving;
            _presenter.Climbing -= OnClimbing;
            _presenter.ClimbingFinished -= OnClimbingFinished;
            _presenter.Crawling -= OnCrawling;
            _presenter.CrawlingFinished -= OnCrawlingFinished;
            _presenter.Falling -= OnFalling;
            _presenter.BlockRemoving -= OnBlockRemoving;
            _presenter.BlockRemoved -= OnBlockRemoved;
        }

        private void FixedUpdate()
        {
            var horizontalMove = Input.GetAxis("Horizontal");
            var verticalMove = Input.GetAxis("Vertical");

            var removeBlockType = RemoveBlockType.None;

            if (Input.GetAxis("RemoveBlockLeft") > float.Epsilon)
            {
                removeBlockType = RemoveBlockType.Left;
            }
            else if (Input.GetAxis("RemoveBlockRight") > float.Epsilon)
            {
                removeBlockType = RemoveBlockType.Right;
            }

            _presenter.UpdateCharacterMoveData(new MovingData(horizontalMove, verticalMove, transform.position));
            _presenter.UpdatePlayerRemovingBlock(removeBlockType, transform.position);
            _presenter.UpdateCharacterState();
        }

        private void OnMoving(Vector2 newPosition, float moveSpeed)
        {
            var delta = newPosition.x - transform.position.x;

            _animationHandler.ApplyAnimation(new MoveAnimationAction(moveSpeed));

            _rigidbody.MovePosition(newPosition);

            SetLookDirection(delta);
        }

        private void OnClimbing(Vector2 newPosition, float moveSpeed)
        {
            _animationHandler.ApplyAnimation(new ClimbAnimationAction(moveSpeed));

            _rigidbody.MovePosition(newPosition);
        }

        private void SetLookDirection(float moveValue)
        {
            switch (moveValue)
            {
                case 0:
                    return;
                case > 0:
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                    break;
                default:
                    transform.localRotation = Quaternion.identity;
                    break;
            }
        }

        private void OnClimbingFinished()
        {
            _animationHandler.ApplyAnimation(new ClimbFinishedAnimationAction());
        }

        private void OnFalling(Vector2 newPosition)
        {
            _animationHandler.ApplyAnimation(new FallingAnimationAction());

            _rigidbody.MovePosition(newPosition);
        }

        private void OnCrawling(Vector2 newPosition, float moveSpeed)
        {
            var delta = newPosition.x - transform.position.x;

            _animationHandler.ApplyAnimation(new CrawlAnimationAction(moveSpeed));

            _rigidbody.MovePosition(newPosition);

            SetLookDirection(delta);
        }

        private void OnCrawlingFinished()
        {
            _animationHandler.ApplyAnimation(new CrawlFinishedAnimationAction());
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