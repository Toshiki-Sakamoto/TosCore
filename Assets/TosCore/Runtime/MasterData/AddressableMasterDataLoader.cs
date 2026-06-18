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
        private readonly IReadOnlyList<IAddressableMasterDataLoadTarget> _repositories;

        [Inject]
        public AddressableMasterDataLoader(IEnumerable<IAddressableMasterDataLoadTarget> repositories)
        {
            _repositories = BuildRepositoryList(repositories);
        }

        public async UniTask LoadAsync(CancellationToken cancellationToken)
        {
            if (_repositories.Count == 0) return;

            var tasks = new UniTask[_repositories.Count];
            for (var i = 0; i < _repositories.Count; i++)
            {
                tasks[i] = LoadAndReplaceAsync(_repositories[i], cancellationToken);
            }

            await UniTask.WhenAll(tasks);
        }

        private static IReadOnlyList<IAddressableMasterDataLoadTarget> BuildRepositoryList(
            IEnumerable<IAddressableMasterDataLoadTarget> repositories)
        {
            if (repositories == null) return Array.Empty<IAddressableMasterDataLoadTarget>();
            return repositories
                .Where(repository => repository != null)
                .ToList();
        }

        private static async UniTask LoadAndReplaceAsync(IAddressableMasterDataLoadTarget repository, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(repository.AddressableKey)) return;
            if (cancellationToken.IsCancellationRequested) return;

            var handle = Addressables.LoadAssetsAsync<ScriptableObject>(repository.AddressableKey, null);
            var masters = await handle.Task;

            if (cancellationToken.IsCancellationRequested) return;
            
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                repository.ReplaceAll(masters);
            }
            else
            {
                Debug.LogWarning($"Addressable load failed. Key: {repository.AddressableKey}");
            }
        }
    }
}
