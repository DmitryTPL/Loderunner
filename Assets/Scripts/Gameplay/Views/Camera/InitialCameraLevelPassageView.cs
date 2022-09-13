using Cysharp.Threading.Tasks;
using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    [RequireComponent(typeof(Camera))]
    public class InitialCameraLevelPassageView : View<InitialCameraLevelPassagePresenter>
    {
        private Camera _camera;
        
        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        protected override void PresenterAttached()
        {
            base.PresenterAttached();

            _presenter.LevelCreated += OnLevelCreated;
        }

        private async UniTask StartPassage()
        {
            _camera.transform.position = new Vector3(0, 0, 1);
            
            await UniTask.Delay(1000);
            
            _presenter.PassageCompleted();
        }

        private void OnLevelCreated()
        {
            StartPassage().Forget();
        }
    }
}