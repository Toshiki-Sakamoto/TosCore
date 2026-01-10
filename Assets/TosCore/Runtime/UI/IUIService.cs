namespace TosCore.UI
{
    public interface IUIService
    {
        void Show(IUIBuilder builder);

        void Hide(IPresenter presenter);
    }
}