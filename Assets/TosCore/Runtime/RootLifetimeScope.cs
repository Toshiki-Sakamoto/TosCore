using MessagePipe;
using TosCore.Entity;
using TosCore.MasterData;
using TosCore.Scene;
using TosCore.TapBlocker;
using TosCore.Tasks;
using TosCore.UI;
using VContainer;
using VContainer.Unity;

namespace TosCore
{
    /// <summary>
    /// Core処理を提供するルートライフタイムスコープ
    /// PJで利用する場合はこのクラスを継承
    /// </summary>
    public abstract class RootLifetimeScope : LifetimeScope
    {
        protected sealed override void Configure(IContainerBuilder builder)
        {
            builder.RegisterMessagePipe();
            
            builder.Register<IdGeneratorRegistry>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<IdGeneratorStateRepository>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SceneInitializer>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SceneStarter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SceneEnterer>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SceneUpdater>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SceneExiter>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.Register<TimedTaskRunner>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<TimedTaskIdFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            
            // UI
            builder.Register<UIService>(Lifetime.Singleton).AsImplementedInterfaces();
//            builder.RegisterComponentInHierarchy<UICameraProvider>().AsSelf();
//            builder.RegisterComponentInHierarchy<TapBlockerRoot>().AsSelf();
//            builder.Register<TapBlockerService>(Lifetime.Singleton).AsImplementedInterfaces();

            // Master
            builder.Register<AddressableMasterDataLoader>(Lifetime.Singleton).AsImplementedInterfaces();
            
            ConfigureCore(builder);
        }

        protected virtual void ConfigureCore(IContainerBuilder builder)
        {
        }
    }
}