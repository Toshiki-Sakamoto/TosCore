using TosCore.Entity;

namespace TosCore.Tasks
{
    public interface ITimedTaskIdFactory
    {
        TimedTaskId Create();
        TimedTaskId Create(string prefix);
        TimedTaskId Create<T>(IIdentifier identifier);
        TimedTaskId Create<T>(long id);
    }
}
