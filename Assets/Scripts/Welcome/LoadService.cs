using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace Loderunner.Welcome
{
    public class LoadService : IStartable
    {
        public async void Start()
        {
            await SceneManager.LoadSceneAsync("UI");
            await SceneManager.LoadSceneAsync("Gameplay", LoadSceneMode.Additive);
        }
    }
}