using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public abstract class CharacterView<TPresenter> : View<TPresenter>, ICharacterInfo
        where TPresenter : CharacterPresenter
    {
        [SerializeField] protected AnimationHandler _animationHandler;

        protected Rigidbody2D _rigidbody;

        public abstract CharacterType CharacterType { get; }
        public int CharacterId { get; set; }
        public Vector2 Position => transform.position;

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start()
        {
            _presenter.CharacterCreated(CharacterId);
        }

        protected void Update()
        {
            _presenter.Position = Position;
        }

        protected virtual void OnDestroy()
        {
            _presenter.Moving -= OnMoving;
            _presenter.Climbing -= OnClimbing;
            _presenter.ClimbingFinished -= OnClimbingFinished;
            _presenter.Crawling -= OnCrawling;
            _presenter.CrawlingFinished -= OnCrawlingFinished;
            _presenter.Falling -= OnFalling;
        }

        protected override void PresenterAttached()
        {
            base.PresenterAttached();

            _presenter.Moving += OnMoving;
            _presenter.Climbing += OnClimbing;
            _presenter.ClimbingFinished += OnClimbingFinished;
            _presenter.Crawling += OnCrawling;
            _presenter.CrawlingFinished += OnCrawlingFinished;
            _presenter.Falling += OnFalling;
        }

        protected void SetLookDirection(float moveValue)
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
    }
}