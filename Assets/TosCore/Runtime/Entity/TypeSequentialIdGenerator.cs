
using VContainer;

namespace TosCore.Entity
{
    public class TypeSequentialIdGenerator<T> : IIdGenerator<T>
    {
        [Inject] private readonly IIdSequenceService _idSequenceService;
        
        public EntityId<T> NewId() =>
            new (_idSequenceService.NextFor<T>());

        public long PeekNext() =>
            _idSequenceService.PeekFor<T>();
    }
}
