using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace TosCore.UI
{
    public class UIService : IUIService
    {
        private CancellationTokenSource _cancellationTokenSource = new();
    
        public void Show(IUIBuilder builder)
        {
            builder.Build();

            var presenter = builder.Presenter;
            presenter.ShowAsync(_cancellationTokenSource.Token).Forget();
        }

        public void Hide(IPresenter presenter)
        {
            presenter.HideAsync(_cancellationTokenSource.Token).Forget();
        }
    }
}