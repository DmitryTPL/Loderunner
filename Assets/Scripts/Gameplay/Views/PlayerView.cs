using System;
using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class PlayerView : View<PlayerPresenter>
    {
        private static readonly int _moveSpeedAnimationParameter = Animator.StringToHash("MoveSpeed");

        private Rigidbody2D _rigidbody;
        private Animator _animator;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            SetLookDirection();
            MoveHorizontal();
        }

        private void MoveHorizontal()
        {
            var inputX = Input.GetAxis("Horizontal");

            var movement = new Vector3(_presenter.PlayerConfig.Speed.x * inputX, 0, 0);

            _rigidbody.velocity = movement;

            _animator.SetFloat(_moveSpeedAnimationParameter, Math.Abs(inputX));
        }

        private void SetLookDirection()
        {
            var inputX = Input.GetAxis("Horizontal");

            switch (inputX)
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
    }
}