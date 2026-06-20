using R3;

namespace TosCore.Input
{
    /// <summary>
    /// 入力コンテキストをスタックで排他管理する。top のコンテキストの Action Map だけが Enable され、
    /// それ以外は Disable される。スタック順がそのまま優先度になる。
    /// 基盤は具体的なアクションを知らず、Map の有効/無効の切り替えだけを担う。
    /// </summary>
    public interface IInputContextStack
    {
        /// <summary>現在 top の（アクティブな）コンテキスト。スタックが空なら null。</summary>
        ReadOnlyReactiveProperty<IInputContext> Current { get; }

        /// <summary>コンテキストを積む。直前の top はサスペンド（Disable）される。</summary>
        void Push(IInputContext context);

        /// <summary>top を外して1つ前のコンテキストへ戻す。空なら何もしない。</summary>
        void Pop();

        /// <summary>指定コンテキストがスタックに含まれているか。</summary>
        bool Contains(IInputContext context);
    }
}
