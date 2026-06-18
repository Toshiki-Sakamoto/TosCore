using System.Threading;
using Cysharp.Threading.Tasks;

namespace TosCore.UI
{
    public interface IModalPresenter<TResult>
    {
        UniTask<TResult> ShowAsync(CancellationToken ct);

        /// <summary>
        /// 外部から強制的に閉じさせられる時に呼び出される
        /// </summary>
        void Close();
    }
}
