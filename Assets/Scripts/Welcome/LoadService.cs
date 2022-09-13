using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace Loderunner.Welcome
{
    public class LoadService : IAsyncStartable
    {
        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            await SceneManager.LoadSceneAsync("UI");
            await SceneManager.LoadSceneAsync("Gameplay", LoadSceneMode.Additive);
        }
    }
}