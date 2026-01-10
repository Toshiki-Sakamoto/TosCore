using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using VContainer;

namespace TosCore.Entity
{
    public class IdGeneratorRegistry : IIdGeneratorRegistry
    {
        private readonly IIdGeneratorStateRepository _stateRepository;
        private readonly ConcurrentDictionary<Type, long> _idMap = new();

        [Inject]
        public IdGeneratorRegistry(IIdGeneratorStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
            RestoreState();
        }

        public long NextFor<T>()
        {
            return _idMap.AddOrUpdate(typeof(T), 1, (_, last) => checked(last + 1));
        }

        public long PeekFor<T>()
        {
            return _idMap.TryGetValue(typeof(T), out var last) ? last + 1 : 1;
        }

        public void SaveState()
        {
            _stateRepository.SetStates(CreateSnapshot());
        }

        private void RestoreState()
        {
            var snapshot = _stateRepository.GetStates();
            foreach (var entry in snapshot)
            {
                var type = entry.Key.Resolve();
                if (type == null) continue;
                
                _idMap[type] = entry.Value;
            }
        }

        private Dictionary<TypeToken, long> CreateSnapshot()
        {
            Dictionary<TypeToken, long> snapshot = new (_idMap.Count);
            foreach (var entry in _idMap)
            {
                snapshot[new TypeToken(entry.Key)] = entry.Value;
            }

            return snapshot;
        }
    }
}
