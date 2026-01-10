using MessagePipe;
using TosCore.Entity;
using TosCore.Scene;
using TosCore.Tasks;
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
            
            ConfigureCore(builder);
        }

        protected virtual void ConfigureCore(IContainerBuilder builder)
        {
        }
    }
}