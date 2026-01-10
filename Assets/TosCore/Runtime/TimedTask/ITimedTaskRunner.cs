using System;
using System.Collections.Generic;

namespace TosCore.Tasks
{
    /// <summary>
    /// 指定した時間だけITimedTaskを呼び出し続ける
    /// </summary>
    public interface ITimedTaskRunner : IDisposable
    {
        /// <summary>
        /// 時間経過の開始
        /// </summary>
        void StartTask(ITimedTask task);

        /// <summary>
        /// 指定したTaskを終了させる
        /// </summary>
        bool CancelTask(TimedTaskId taskId);
        
        /// <summary>
        /// 指定したTaskが実行中か
        /// </summary>
        bool IsRunning(TimedTaskId taskId);
        
        IReadOnlyCollection<TimedTaskState> CaptureState();
        
        void RestoreState(IEnumerable<TimedTaskState> states, Func<TimedTaskState, ITimedTask> taskFactory);
    }
}
