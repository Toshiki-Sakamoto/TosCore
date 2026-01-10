namespace TosCore.UI
{
    public abstract class UIBuilder : IUIBuilder
    {
        public abstract IPresenter Presenter { get; }

        public abstract void Build();
    }
}