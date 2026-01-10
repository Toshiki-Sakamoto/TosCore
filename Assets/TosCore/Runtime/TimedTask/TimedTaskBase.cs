using System;
using VContainer;

namespace TosCore.Tasks
{
    /// <summary>
    /// 共通処理ベースクラス
    /// </summary>
    public abstract class TimedTaskBase<T> : ITimedTask
    {
        [Inject] protected readonly ITimedTaskIdFactory _taskIdFactory;

        public TimedTaskId TaskId { get; private set; }

        public abstract float Duration { get; protected set; }


        public void OnInitialize()
        {
            TaskId = CreateTaskId();
        }
        
        public abstract void OnStart();
        public abstract void OnTick(float elapsedTime, float deltaTime);
        public abstract void OnComplete();
        public abstract void OnCancelled();

        public virtual TimedTaskState CreateState(float elapsedTime)
        {
            var clampedElapsed = ClampElapsedTime(elapsedTime);
            return new TimedTaskState
            {
                TaskId = TaskId,
                ElapsedTime = clampedElapsed,
                Payload = SerializePayload(clampedElapsed) ?? Array.Empty<byte>()
            };
        }

        public virtual void RestoreState(in TimedTaskState state)
        {
            TaskId = state.TaskId;
            DeserializePayload(state.Payload ?? Array.Empty<byte>(), state.ElapsedTime);
        }

        protected virtual TimedTaskId CreateTaskId()
        {
            return _taskIdFactory.Create();
        }

        protected virtual byte[]? SerializePayload(float elapsedTime) => 
            null;

        protected virtual void DeserializePayload(byte[] payload, float elapsedTime)
        {
        }

        private float ClampElapsedTime(float elapsedTime)
        {
            var duration = Duration;
            if (float.IsNaN(duration) || duration < 0f)
            {
                duration = 0f;
            }

            if (float.IsNaN(elapsedTime)) return 0f;
            if (elapsedTime < 0f) return 0f;

            return elapsedTime > duration ? duration : elapsedTime;
        }
    }
}
