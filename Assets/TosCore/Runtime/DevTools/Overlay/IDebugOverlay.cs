using System;
using UnityEngine;

namespace TosCore.DevTools.Overlay
{
    public interface IDebugOverlay
    {
        /// <summary>
        /// IDebugEntry を登録する。戻り値の IDisposable を Dispose すると登録解除される
        /// </summary>
        IDisposable Register(IDebugEntry entry);

        /// <summary>
        /// ラムダ式で簡易登録する。戻り値の IDisposable を Dispose すると登録解除される
        /// </summary>
        IDisposable Register(string label, Func<string> valueProvider, Color? color = null);

        /// <summary>
        /// IDebugAction を登録する。戻り値の IDisposable を Dispose すると登録解除される
        /// </summary>
        IDisposable RegisterAction(IDebugAction action);

        /// <summary>
        /// ラムダ式でデバッグボタンを登録する。戻り値の IDisposable を Dispose すると登録解除される
        /// </summary>
        IDisposable RegisterAction(string label, Action onClick);
    }
}
