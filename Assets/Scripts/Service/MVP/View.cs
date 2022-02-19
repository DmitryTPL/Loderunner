using UnityEngine;
using VContainer;

namespace Loderunner.Service
{
    public abstract class View<TPresenter> : MonoBehaviour
        where TPresenter: Presenter
    {
        protected TPresenter _presenter;
        protected IObjectResolver _resolver;
        
        [Inject]
        public void Constructor(TPresenter presenter, IObjectResolver resolver)
        {
            _presenter = presenter;
            _resolver = resolver;
        }
    }
}
