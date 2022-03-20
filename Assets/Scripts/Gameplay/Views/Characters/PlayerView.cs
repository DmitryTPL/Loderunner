using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class PlayerView : View<PlayerPresenter>, ICharacterInfo
    {
        [SerializeField] private CharacterAnimationHandler _animationHandler;
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
        }

        private void FixedUpdate()
        {
            var horizontalMove = Input.GetAxis("Horizontal");
            var verticalMove = Input.GetAxis("Vertical");

            _presenter.UpdateCharacterData(new MovingData(horizontalMove, verticalMove, transform.position));
        }

        private void OnMoving(Vector3 newPosition, float moveSpeed)
        {
            var delta = newPosition.x - transform.position.x;

            _animationHandler.ApplyAnimation(new MoveAnimationAction(moveSpeed));

            _rigidbody.MovePosition(newPosition);

            SetLookDirection(delta);
        }

        private void OnClimbing(Vector3 newPosition, float moveSpeed)
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

        private void OnFalling(Vector3 newPosition)
        {
            _animationHandler.ApplyAnimation(new FallingAnimationAction());
            
            _rigidbody.MovePosition(newPosition);
        }

        private void OnCrawling(Vector3 newPosition, float moveSpeed)
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
    }
}