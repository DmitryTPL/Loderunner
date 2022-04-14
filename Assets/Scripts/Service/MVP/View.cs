using System;
using UnityEngine;
using VContainer;

namespace Loderunner.Service
{
    public abstract class View<TPresenter> : MonoBehaviour
        where TPresenter: Presenter
    {
        protected TPresenter _presenter;

        public TPresenter Presenter => _presenter;
        
        [Inject]
        public void Constructor(Func<TPresenter> presenterFactory)
        {
            _presenter = presenterFactory();
        }

        protected virtual void OnDestroy()
        {
            _presenter.Dispose();
        }
    }
}
