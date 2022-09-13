using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Loderunner.Install
{
    public class UILifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
        }

        [ContextMenu("Unload Gameplay scene")]
        public void UnloadGameplayScene()
        {
            SceneManager.UnloadSceneAsync("Gameplay");
        }
    }
}
