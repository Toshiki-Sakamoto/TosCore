using UnityDebugSheet.Runtime.Core.Scripts;
using UnityEngine;
using VContainer.Unity;

namespace TosCore.DevTools.DebugPage
{
    /// <summary>
    /// シーン上の <typeparamref name="TScope"/>（LifetimeScope）を探して自身に Inject できる DebugSheet ページ基底。
    /// </summary>
    public abstract class ScopedDebugPageBase<TScope> : DefaultDebugPageBase
        where TScope : LifetimeScope
    {
        /// <summary>
        /// シーン上から <typeparamref name="TScope"/> を探して自身に Inject する。
        /// </summary>
        public bool InjectFromScope()
        {
            var scope = Object.FindFirstObjectByType<TScope>();
            if (scope == null || scope.Container == null) return false;

            scope.Container.Inject(this);
            return true;
        }
    }
}
