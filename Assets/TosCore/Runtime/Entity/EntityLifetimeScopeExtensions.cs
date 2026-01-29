using VContainer;

namespace TosCore.Entity
{
    public static class EntityLifetimeScopeExtensions
    {
        public static void RegisterEntity<TEntity>(this IContainerBuilder builder)
            where TEntity : IEntity, new()
        {
            builder.Register<EntityForIdFactory<TEntity>>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<TypeSequentialIdGenerator<TEntity>>(Lifetime.Singleton).AsImplementedInterfaces();
        }
        
        public static void RegisterEntity<TEntity, TEntityAbstract>(this IContainerBuilder builder)
            where TEntity : TEntityAbstract, new()
            where TEntityAbstract: IEntity
        {
            builder.Register<EntityForIdFactory<TEntity, TEntityAbstract>>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<TypeSequentialIdGenerator<TEntity>>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}