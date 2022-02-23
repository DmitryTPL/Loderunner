using System;
using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class PlayerView : View<PlayerPresenter>, ICharacterView
    {
        private static readonly int _isMovingAnimationParameter = Animator.StringToHash("IsMoving");
        private static readonly int _isClimbingAnimationParameter = Animator.StringToHash("IsClimbing");
        private static readonly int _climbFinishedAnimationParameter = Animator.StringToHash("ClimbFinished");

        private Rigidbody2D _rigidbody;
        private Animator _animator;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            _presenter.Moving += OnMoving;
            _presenter.Climbing += OnClimbing;
            _presenter.ClimbingFinished += OnClimbingFinished;
        }

        private void OnDestroy()
        {
            _presenter.Moving -= OnMoving;
            _presenter.Climbing -= OnClimbing;
            _presenter.ClimbingFinished -= OnClimbingFinished;
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
            
            _animator.SetBool(_isClimbingAnimationParameter, false);
            _animator.SetBool(_isMovingAnimationParameter, isPositionChanged);
            
            _rigidbody.MovePosition(newPosition);

            SetLookDirection(delta);
        }

        private void OnClimbing(Vector3 newPosition)
        {
            var isPositionChanged = Math.Abs(newPosition.y - transform.position.y) > float.Epsilon;

            _animator.SetBool(_isClimbingAnimationParameter, isPositionChanged);
            _animator.SetBool(_isMovingAnimationParameter, false);

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
            _animator.SetTrigger(_climbFinishedAnimationParameter);
        }
    }
}