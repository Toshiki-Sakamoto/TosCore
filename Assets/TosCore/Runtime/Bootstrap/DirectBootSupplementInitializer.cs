using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using TosCore.Scene;
using UnityEngine;

namespace TosCore.Bootstrap
{
    /// <summary>
    /// シーン直起動の補完を駆動する初期化処理。
    /// 通常初期化（他の <see cref="ISceneInitializable"/>）の後に最遅優先度で走り、
    /// <see cref="ISceneDirectBootRequirement"/> が一つでも未充足なら直起動とみなして
    /// <see cref="IDirectBootIndicator"/> を点灯し、<see cref="ISceneDirectBootSupplement"/> を実行する。
    /// 前提が全て揃っていれば（通常フロー）何もしない。
    /// SceneLifetimeScope が常時登録するため、PJ は要件と補完処理を足すだけでよい。
    /// </summary>
    public sealed class DirectBootSupplementInitializer : ISceneInitializable, ILifecyclePrioritized
    {
        private readonly IReadOnlyList<ISceneDirectBootRequirement> _requirements;
        private readonly IReadOnlyList<ISceneDirectBootSupplement> _supplements;
        private readonly IDirectBootIndicator _indicator;

        public DirectBootSupplementInitializer(
            IEnumerable<ISceneDirectBootRequirement> requirements,
            IEnumerable<ISceneDirectBootSupplement> supplements,
            IDirectBootIndicator indicator)
        {
            _requirements = requirements?.ToArray() ?? Array.Empty<ISceneDirectBootRequirement>();
            _supplements = supplements?.ToArray() ?? Array.Empty<ISceneDirectBootSupplement>();
            _indicator = indicator;
        }

        // 通常初期化の後に判定するため最遅優先度
        public int Priority => int.MaxValue;

        public async UniTask InitializeAsync(CancellationToken ct)
        {
            // 直起動チェック対象（要件）が無ければ何もしない
            if (_requirements.Count == 0) return;

            var unmet = _requirements.Where(r => !r.IsSatisfied()).ToArray();
            if (unmet.Length == 0) return; // 通常フロー: 前提は揃っている

            foreach (var requirement in unmet)
            {
                Debug.LogWarning($"[DirectBoot] 前提未充足: {requirement.Label}");
            }

            _indicator.Enable();

            foreach (var supplement in _supplements)
            {
                await supplement.RunAsync(ct);
            }
        }
    }
}
