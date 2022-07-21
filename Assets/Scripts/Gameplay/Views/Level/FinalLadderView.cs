using Cysharp.Threading.Tasks;
using Loderunner.Gameplay.Ladder;
using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class FinalLadderView : View<FinalLadderPresenter>
    {
        [SerializeField] private BoxCollider2D _mainCollider;
        [SerializeField] private AnimationHandler _foldingScreenAnimationHandler;

        private void Start()
        {
            _presenter.LevelCompleted += OnLevelCompleted;
            _presenter.LevelReset += OnLevelReset;
        }

        private void OnDestroy()
        {
            _presenter.LevelCompleted -= OnLevelCompleted;
            _presenter.LevelReset -= OnLevelReset;
        }

        private void OnLevelCompleted()
        {
            _foldingScreenAnimationHandler.ApplyAnimation(new ShowFinalLadderAnimationAction());
            
            TurnOnMainCollider().Forget();
        }

        private async UniTask TurnOnMainCollider()
        {
            await UniTask.Delay(1000);

            _mainCollider.enabled = true;
        }

        private void OnLevelReset()
        {
            _mainCollider.enabled = false;
            _foldingScreenAnimationHandler.ApplyAnimation(new ResetFinalLadderAnimationAction());
        }
    }
}