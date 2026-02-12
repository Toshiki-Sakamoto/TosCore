using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TosCore.Scene
{
    [DefaultExecutionOrder(-5001)] // 他のLifetimeScopeよりも早く実行されるようにする
    public abstract class SceneLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<SceneLifecycleRunner>().As<ISceneLifecycleRunner>();
            builder.RegisterEntryPoint<SceneTransitionRequestListener>();
            builder.Register<SceneTransitionService>(Lifetime.Singleton).As<ISceneTransitionService>();
            
            ConfigureCore(builder);
        }

        protected virtual void ConfigureCore(IContainerBuilder builder)
        {
        }
        
        protected override LifetimeScope FindParent()
        {
            return Find<RootLifetimeScope>();
        }
    }
}
