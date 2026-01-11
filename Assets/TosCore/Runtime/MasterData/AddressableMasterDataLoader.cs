using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer;

namespace TosCore.MasterData
{
    public class AddressableMasterDataLoader : IMasterDataLoader
    {
        private readonly IReadOnlyList<IAddressableMasterDataRegistry> _registries;

        [Inject]
        public AddressableMasterDataLoader(IEnumerable<IAddressableMasterDataRegistry> registries)
        {
            _registries = BuildRegistryList(registries);
        }

        public async UniTask LoadAsync(CancellationToken cancellationToken)
        {
            foreach (var registry in _registries)
            {
                registry.Clear();
            }

            if (_registries.Count == 0) return;

            var tasks = new UniTask[_registries.Count];
            for (var i = 0; i < _registries.Count; i++)
            {
                tasks[i] = LoadAndRegisterAsync(_registries[i], cancellationToken);
            }

            await UniTask.WhenAll(tasks);
        }

        private static IReadOnlyList<IAddressableMasterDataRegistry> BuildRegistryList(
            IEnumerable<IAddressableMasterDataRegistry> registries)
        {
            if (registries == null) return Array.Empty<IAddressableMasterDataRegistry>();
            return registries
                .Where(registry => registry != null)
                .ToList();
        }

        private static async UniTask LoadAndRegisterAsync(
            IAddressableMasterDataRegistry registry,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(registry.AddressableKey)) return;
            if (cancellationToken.IsCancellationRequested) return;

            var handle = Addressables.LoadAssetsAsync<ScriptableObject>(registry.AddressableKey, null);
            var masters = await handle.Task;

            if (cancellationToken.IsCancellationRequested) return;
            
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                registry.Register(masters);
            }
            else
            {
                Debug.LogWarning($"Addressable load failed. Key: {registry.AddressableKey}");
            }
        }
    }
}
