using System;

namespace TosCore.Tasks
{
    /// <summary>
    /// 保存・復元に利用するタイムドタスクの状態。
    /// </summary>
    [Serializable]
    public struct TimedTaskState
    {
        public TimedTaskId TaskId;
        
        public float ElapsedTime;
        
        public byte[] Payload;
    }
}
