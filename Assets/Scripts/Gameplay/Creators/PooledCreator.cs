using System.Collections.Generic;
using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public interface IInstantiateData
    {
        Transform Parent { get; }
    }
    
    public abstract class PooledCreator<TView, TPresenter>
        where TPresenter: Presenter
        where TView: View<TPresenter>
    {
        private readonly Transform _pool;
        private readonly Stack<TView> _objects = new();

        protected abstract TView Instantiate(IInstantiateData rawData);

        protected PooledCreator(Transform pool)
        {
            _pool = pool;
        }

        protected TView Pop(IInstantiateData data)
        {
            if (_objects.Count == 0)
            {
                return Instantiate(data);
            }

            var view = _objects.Pop();

            view.transform.parent = data.Parent;

            return view;
        }

        protected void Push(TView view)
        {
            view.gameObject.SetActive(false);
            view.transform.parent = _pool;
            _objects.Push(view);
        }
    }
}