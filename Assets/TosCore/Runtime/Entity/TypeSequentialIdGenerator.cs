
using VContainer;

namespace TosCore.Entity
{
    public class TypeSequentialIdGenerator<T> : IIdGenerator<T>
    {
        [Inject] private readonly IIdGeneratorRegistry _idGenratorRegistry;
        
        public EntityId<T> NewId() =>
            new (_idGenratorRegistry.NextFor<T>());

        public long PeekNext() =>
            _idGenratorRegistry.PeekFor<T>();
    }
}