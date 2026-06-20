using UnityDebugSheet.Runtime.Core.Scripts;
using VContainer;
using VContainer.Unity;

namespace TosCore.DevTools.DebugPage
{
    /// <summary>
    /// 起動時に DebugSheet へ初期ページ <typeparamref name="TRootPage"/> を登録する。
    /// Page 生成時に VContainer の Inject を挟むので、サブページで [Inject] が使える。
    /// </summary>
    /// <remarks>
    /// 各 PJ は自前のルートページ型を <typeparamref name="TRootPage"/> に指定し、
    /// <c>DebugPageBootstrap&lt;MyRootPage&gt;</c> を IInitializable として登録する。
    /// </remarks>
    public class DebugPageBootstrap<TRootPage> : IInitializable
        where TRootPage : DefaultDebugPageBase
    {
        private readonly IObjectResolver _resolver;

        [Inject]
        public DebugPageBootstrap(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        public void Initialize()
        {
            var sheet = DebugSheet.Instance;
            if (sheet == null) return;

            sheet.GetOrCreateInitialPage<TRootPage>(onLoad: x => _resolver.Inject(x.page));
        }
    }
}
