namespace TosCore.Tasks
{
    /// <summary>
    /// 時間経過で進行するタスクの共通インターフェース。
    /// </summary>
    public interface ITimedTask
    {
        TimedTaskId TaskId { get; }
        float Duration { get; }

        void OnInitialize();
        void OnStart();
        void OnTick(float elapsedTime, float deltaTime);
        void OnComplete();
        void OnCancelled();

        TimedTaskState CreateState(float elapsedTime);
        void RestoreState(in TimedTaskState state);
    }
}
