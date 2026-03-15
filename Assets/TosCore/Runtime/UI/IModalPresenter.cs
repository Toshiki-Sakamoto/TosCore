using System.Threading;
using Cysharp.Threading.Tasks;

namespace TosCore.UI
{
    public interface IModalPresenter<TResult>
    {
        UniTask<TResult> ShowAsync(CancellationToken ct);

        void Close();
    }
}
