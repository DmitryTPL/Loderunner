using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class GuardianView : CharacterView<GuardianPresenter>
    {
        [SerializeField] private BoxCollider2D _collider;

        public override CharacterType CharacterType => CharacterType.Guardian;

        private Action _respawnEventHandler;

        protected override void PresenterAttached()
        {
            base.PresenterAttached();

            _presenter.CurrentRemovedWallBlockState.ForEachAsync(OnRemovedWallBlockStateChanged).Forget();

            _respawnEventHandler = UniTask.Action(OnRespawn);
            
            _presenter.Respawn += _respawnEventHandler;
        }

        private void FixedUpdate()
        {
            var (horizontalDirection, verticalDirection) = _presenter.GetDirection();

            _presenter.UpdateCharacterMoveData(new MovingData(horizontalDirection, verticalDirection, Position));
            _presenter.UpdateCharacterState();
        }

        protected override void OnDestroy()
        {
            _presenter.Respawn -= _respawnEventHandler;
        }

        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            var character = otherCollider.TryGetCharacter();

            if (character != null)
            {
                switch (character.CharacterType)
                {
                    case CharacterType.Player:
                        _presenter.PlayerCached();
                        break;
                }
            }
        }

        private void OnRemovedWallBlockStateChanged(GuardianPresenter.RemovedWallBlockState state)
        {
            _collider.enabled = state switch
            {
                GuardianPresenter.RemovedWallBlockState.ClimbingUp => false,
                _ => true
            };
        }

        private async UniTaskVoid OnRespawn()
        {
            await _animationHandler.ApplyAnimation(new GuardianSpawnAnimationAction());

            await _presenter.RespawnFinished();
        }
    }
}