using System;
using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class PlayerView : View<PlayerPresenter>, ICharacterView
    {
        [SerializeField] private CharacterAnimationHandler _animationHandler;

        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _presenter.Moving += OnMoving;
            _presenter.Climbing += OnClimbing;
            _presenter.ClimbingFinished += OnClimbingFinished;
            _presenter.Falling += OnFalling;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            _presenter.Moving -= OnMoving;
            _presenter.Climbing -= OnClimbing;
            _presenter.ClimbingFinished -= OnClimbingFinished;
            _presenter.Falling -= OnFalling;
        }

        private void FixedUpdate()
        {
            var horizontalMove = Input.GetAxis("Horizontal");
            var verticalMove = Input.GetAxis("Vertical");

            _presenter.UpdateCharacterData(new MovingData(horizontalMove, verticalMove, transform.position));
        }

        private void OnMoving(Vector3 newPosition)
        {
            var delta = newPosition.x - transform.position.x;
            var isPositionChanged = Math.Abs(delta) > float.Epsilon;

            _animationHandler.ApplyAnimation(new MoveAnimationAction(isPositionChanged));

            _rigidbody.MovePosition(newPosition);

            SetLookDirection(delta);
        }

        private void OnClimbing(Vector3 newPosition)
        {
            var isPositionChanged = Math.Abs(newPosition.y - transform.position.y) > float.Epsilon;
            
            _animationHandler.ApplyAnimation(new ClimbAnimationAction(isPositionChanged));

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
    }
}