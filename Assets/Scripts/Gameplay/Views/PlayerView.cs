using System;
using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class PlayerView : View<PlayerPresenter>
    {
        private static readonly int _moveSpeedAnimationParameter = Animator.StringToHash("MoveSpeed");
        private static readonly int _climbSpeedAnimationParameter = Animator.StringToHash("ClimbSpeed");
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

        private void Update()
        {
            var horizontalMove = Input.GetAxis("Horizontal");
            var verticalMove = Input.GetAxis("Vertical");

            _presenter.UpdateCharacterData(horizontalMove, verticalMove, transform.position);
        }

        private void OnMoving(float movementValue)
        {
            MoveHorizontal(movementValue);
        }

        private void OnClimbing(float movementValue)
        {
            Climb(movementValue);
        }

        private void MoveHorizontal(float movementValue)
        {
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;

            _rigidbody.velocity = new Vector2(movementValue, 0);

            _animator.SetFloat(_climbSpeedAnimationParameter, 0);
            _animator.SetFloat(_moveSpeedAnimationParameter, Math.Abs(movementValue));

            SetLookDirection(movementValue);
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

        private void Climb(float moveValue)
        {
            _rigidbody.bodyType = RigidbodyType2D.Kinematic;

            _animator.SetFloat(_moveSpeedAnimationParameter, 0);
            _animator.SetFloat(_climbSpeedAnimationParameter, Math.Abs(moveValue) > 0 ? 1 : 0);

            if (Math.Abs(moveValue) <= float.Epsilon)
            {
                return;
            }

            if (!transform.position.x.Equals(_presenter.ClimbingData.LadderCenter))
            {
                transform.position = new Vector3(_presenter.ClimbingData.LadderCenter, transform.position.y, transform.position.z);
            }

            _rigidbody.MovePosition(transform.position + new Vector3(0, moveValue, 0));
        }

        private void OnClimbingFinished()
        {
            _animator.SetFloat(_climbSpeedAnimationParameter, 0);
            _animator.SetTrigger(_climbFinishedAnimationParameter);
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}