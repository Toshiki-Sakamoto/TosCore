
using TosCore.Entity;
using VContainer;

namespace TosCore.Tasks
{
    public sealed class TimedTaskIdFactory : ITimedTaskIdFactory
    {
        private readonly IIdGeneratorRegistry _idGeneratorRegistry;

        [Inject]
        public TimedTaskIdFactory(IIdGeneratorRegistry idGeneratorRegistry)
        {
            _idGeneratorRegistry = idGeneratorRegistry;
        }

        public TimedTaskId Create() =>
            Create(string.Empty);

        public TimedTaskId Create(string prefix)
        {
            var seed = _idGeneratorRegistry.NextFor<TimedTaskId>();
            return CreateCore(prefix, seed);
        }

        public TimedTaskId Create<T>(IIdentifier identifier) =>
            Create<T>(identifier.Id);

        public TimedTaskId Create<T>(long id) =>
            CreateCore(typeof(T).Name, id);

        private TimedTaskId CreateCore(string prefix, long id)
        {
            var seedText = id.ToString();
            if (string.IsNullOrEmpty(prefix))
            {
                return new TimedTaskId(seedText);
            }

            return new TimedTaskId($"{prefix}-{seedText}");
        }
    }
}
