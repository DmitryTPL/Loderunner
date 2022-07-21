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

        private void Start()
        {
            StartPassage().Forget();
        }

        private async UniTask StartPassage()
        {
            await UniTask.Delay(1000);
            
            _presenter.PassageCompleted();
        }
    }
}