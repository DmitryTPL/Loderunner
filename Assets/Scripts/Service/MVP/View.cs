using System;
using Cysharp.Threading.Tasks;
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
        public void Constructor(TPresenter presenter)
        {
            _presenter = presenter;
            
            PresenterAttached();
        }

        protected virtual void PresenterAttached()
        {
            
        }
    }
}
