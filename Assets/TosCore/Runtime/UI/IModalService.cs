using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace TosCore.UI
{
    /// <summary>
    /// モーダルサービス
    /// </summary>
    public interface IModalService
    {
        /// <summary>
        /// awaitで結果を待機できる
        /// </summary>
        UniTask<TResult> ShowAsync<TResult>(IModalBuilder<TResult> builder, CancellationToken ct = default);

        /// <summary>
        /// 例外を投げずにResultとして返す
        /// </summary>
        UniTask<ModalResult<TResult>> TryShowAsync<TResult>(IModalBuilder<TResult> builder, CancellationToken ct = default);

        /// <summary>
        /// コールバックで結果を受け取る
        /// </summary>
        void Show<TResult>(IModalBuilder<TResult> builder, Action<ModalResult<TResult>> onCompleted, CancellationToken ct = default);

        /// <summary>
        /// モーダルが開いた時
        /// </summary>
        event Action<int> Opened;
        
        /// <summary>
        /// モーダルが閉じた時
        /// </summary>
        event Action<int> Closed;

        /// <summary>
        /// 現在のモーダル数
        /// </summary>
        int ActiveCount { get; }

        void CloseTop();

        void CloseAll();
    }
}
