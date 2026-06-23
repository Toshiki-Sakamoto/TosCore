using TosCore.Bootstrap;
using TosCore.SystemInit;
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
            builder.Register<SceneTransitionUseCase>(Lifetime.Singleton).As<ISceneTransitionUseCase>();
            builder.Register<UnitySceneLoader>(Lifetime.Singleton).As<ISceneLoader>();

            // システム初期化（マスタ等）の完了を各シーン init 冒頭（最小 Priority）で待つゲート
            builder.Register<SystemReadyGate>(Lifetime.Singleton).As<ISceneInitializable>();

            // 直起動の補完＋デバッグ表示を常時組み込む（要件0件なら no-op）
            // PJ は ISceneDirectBootRequirement / ISceneDirectBootSupplement を ConfigureCore で足すだけ
            builder.Register<DirectBootIndicator>(Lifetime.Singleton).As<IDirectBootIndicator>();
            builder.Register<DirectBootSupplementInitializer>(Lifetime.Singleton).As<ISceneInitializable>();
            
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
