namespace TosCore.UI
{
    public interface IUIBuilder
    {
        IPresenter Presenter { get; }
    
        void Build();
    }
}