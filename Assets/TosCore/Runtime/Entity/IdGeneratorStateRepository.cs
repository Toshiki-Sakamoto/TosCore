using System;
using System.Collections.Generic;
using TosCore.Collection;
using UnityEngine;

namespace TosCore.Entity
{
    [Serializable]
    public class IdGeneratorStateRepository : IIdGeneratorStateRepository
    {
        [SerializeField] private SerializableDictionary<TypeToken, long> _stateMap = new();

        public IReadOnlyDictionary<TypeToken, long> GetStates()
        {
            return _stateMap;
        }

        public void SetStates(IReadOnlyDictionary<TypeToken, long> state)
        {
            _stateMap.Clear();

            foreach (var entry in state)
            {
                _stateMap[entry.Key] = entry.Value;
            }
        }
    }
}
