using System.Threading;
using Cysharp.Threading.Tasks;

namespace TosCore.UI
{
    public interface IPresenter
    {
        UniTask ShowAsync(CancellationToken cancellationToken);
        
        UniTask HideAsync(CancellationToken cancellationToken);
    }
}