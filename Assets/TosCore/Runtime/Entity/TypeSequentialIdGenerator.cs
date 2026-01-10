
using VContainer;

namespace TosCore.Entity
{
    public class TypeSequentialIdGenerator<T> : IIdGenerator<T>
    {
        [Inject] private readonly IIdGeneratorRegistry _idGeneratorRegistry;
        
        public EntityId<T> NewId() =>
            new (_idGeneratorRegistry.NextFor<T>());

        public long PeekNext() =>
            _idGeneratorRegistry.PeekFor<T>();
    }
}