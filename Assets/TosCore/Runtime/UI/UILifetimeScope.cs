using TosCore;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TosCore.UI
{
    [DefaultExecutionOrder(-5000)]
    public class UILifetimeScope : LifetimeScope
    {
        [SerializeField] private ModalRootProvider _modalRootProvider;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_modalRootProvider).AsImplementedInterfaces();
            builder.Register<ModalService>(Lifetime.Singleton).As<IModalService>();

            ConfigureUI(builder);
        }

        protected virtual void ConfigureUI(IContainerBuilder builder)
        {
        }

        protected override LifetimeScope FindParent()
        {
            return Find<RootLifetimeScope>();
        }
    }
}
