using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;

namespace TosCore.UI
{
    public abstract class PresenterBase : IPresenter
    {
        [Inject] protected IUIService _uiService;
    
        public abstract UniTask ShowAsync(CancellationToken cancellationToken);
    
        public abstract UniTask HideAsync(CancellationToken cancellationToken);
    }
}