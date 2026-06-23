using System.Threading;
using Cysharp.Threading.Tasks;

namespace TosCore.SystemInit
{
    /// <summary>
    /// 登録された <see cref="ISystemInitializer"/> をアプリ全体で一度だけ実行するランナー
    /// </summary>
    public interface ISystemInitializationRunner
    {
        /// <summary>システム初期化が完了済みか</summary>
        bool IsInitialized { get; }

        /// <summary>
        /// システム初期化を保証する
        /// </summary>
        UniTask InitializedOneShotAsync(CancellationToken ct = default);
    }
}
