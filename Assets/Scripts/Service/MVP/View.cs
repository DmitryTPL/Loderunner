using UnityEngine;
using VContainer;

namespace Loderunner.Service
{
    public abstract class View<TPresenter> : MonoBehaviour
        where TPresenter: Presenter
    {
        protected TPresenter _presenter;
        
        [Inject]
        public void Constructor(TPresenter presenter)
        {
            _presenter = presenter;
        }

        protected virtual void OnDestroy()
        {
            _presenter.Dispose();
        }
    }
}
