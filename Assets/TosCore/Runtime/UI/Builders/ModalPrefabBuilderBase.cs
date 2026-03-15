using TosCore.Instantiation;
using UnityEngine;

namespace TosCore.UI
{
    public abstract class ModalPrefabBuilderBase<TResult, TView> : IModalBuilder<TResult>
        where TView : Component
    {
        private readonly IObjectInstantiator _instantiator;
        private readonly TView _prefab;

        protected ModalPrefabBuilderBase(IObjectInstantiator instantiator, TView prefab)
        {
            _instantiator = instantiator;
            _prefab = prefab;
        }

        public ModalBuildResult<TResult> Build(Transform root)
        {
            var view = _instantiator.Instantiate(_prefab, root);
            var presenter = CreatePresenter(view);
            return new ModalBuildResult<TResult>(presenter, view.transform);
        }

        protected abstract IModalPresenter<TResult> CreatePresenter(TView view);
    }
}
