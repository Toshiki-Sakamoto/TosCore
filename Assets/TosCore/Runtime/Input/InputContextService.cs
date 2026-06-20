using System;
using TosCore.Scene;

namespace TosCore.Input
{
    /// <summary>
    /// コンテキスト専用サービスの基底。1つの <see cref="IInputContext"/> と 1:1 で対応し、
    /// 自コンテキストがスタックの top（＝ Action Map が Enable）になっている間だけ稼働する。
    /// 活性の検出は Action Map の enabled 状態の遷移で行い、稼働中は毎フレーム <see cref="OnTick"/> が呼ばれる。
    /// このクラス（と派生の具象サービス）までが Input System に触れてよい層。
    /// </summary>
    public abstract class InputContextService : ISceneTickable, IDisposable
    {
        /// <summary>このサービスが担当するコンテキスト。</summary>
        protected IInputContext Context { get; }

        /// <summary>自コンテキストが現在アクティブ（top）かどうか。</summary>
        protected bool IsActive { get; private set; }

        protected InputContextService(IInputContext context)
        {
            Context = context;
        }

        public void Tick(float deltaTime)
        {
            // 活性は「自分の Action Map が Enable されているか」で判定する。
            // スタックが top の Map だけ Enable するため、これがそのまま top 判定になる。
            var now = Context.ActionMap.enabled;
            if (now != IsActive)
            {
                IsActive = now;
                if (now)
                {
                    OnActivated();
                }
                else
                {
                    OnDeactivated();
                }
            }

            if (IsActive)
            {
                OnTick(deltaTime);
            }
        }

        /// <summary>自コンテキストがアクティブになった瞬間。</summary>
        protected virtual void OnActivated()
        {
        }

        /// <summary>自コンテキストが非アクティブになった瞬間。連続値の中立化などを行う。</summary>
        protected virtual void OnDeactivated()
        {
        }

        /// <summary>アクティブな間、毎フレーム呼ばれる。Action Map を解釈する本体。</summary>
        protected virtual void OnTick(float deltaTime)
        {
        }

        public virtual void Dispose()
        {
        }
    }
}
