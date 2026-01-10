using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace TosCore.Tasks
{
    public sealed class TimedTaskRunner : ITimedTaskRunner
    {
        private readonly Dictionary<TimedTaskId, TaskWrapper> _tasks = new();
        private readonly List<TimedTaskId> _completedTaskIds = new();
        private readonly IDisposable _updateSubscription;

        public TimedTaskRunner()
        {
            _updateSubscription = Observable.EveryUpdate()
                .Subscribe(_ => Tick(Time.deltaTime));
        }

        public void StartTask(ITimedTask task)
        {
            task.OnInitialize();
            
            if (_tasks.TryGetValue(task.TaskId, out var existing))
            {
                existing.Cancel();
                _tasks.Remove(task.TaskId);
            }

            var wrapper = new TaskWrapper(task);
            _tasks[task.TaskId] = wrapper;
            wrapper.Start();
        }

        public bool CancelTask(TimedTaskId taskId)
        {
            if (!_tasks.TryGetValue(taskId, out TaskWrapper wrapper)) return false;

            wrapper.Cancel();
            _tasks.Remove(taskId);
            return true;
        }

        public bool IsRunning(TimedTaskId taskId)
        {
            return _tasks.ContainsKey(taskId);
        }

        public IReadOnlyCollection<TimedTaskState> CaptureState()
        {
            var states = new List<TimedTaskState>(_tasks.Count);
            foreach (var wrapper in _tasks.Values)
            {
                states.Add(wrapper.CreateState());
            }

            return states.AsReadOnly();
        }

        public void RestoreState(IEnumerable<TimedTaskState> states, Func<TimedTaskState, ITimedTask> taskFactory)
        {
            _tasks.Clear();

            if (states == null) return;

            foreach (var state in states)
            {
                var task = taskFactory(state);
                if (task == null) continue;

                task.RestoreState(state);
                var wrapper = new TaskWrapper(task, state.ElapsedTime);
                _tasks[task.TaskId] = wrapper;
                wrapper.Start();
            }
        }

        public void Dispose()
        {
            _updateSubscription?.Dispose();
            foreach (var wrapper in _tasks.Values)
            {
                wrapper.Cancel();
            }
            _tasks.Clear();
            _completedTaskIds.Clear();
        }

        private void Tick(float deltaTime)
        {
            if (_tasks.Count == 0) return;

            foreach (var entry in _tasks)
            {
                entry.Value.Tick(deltaTime);
                if (entry.Value.IsCompleted)
                {
                    _completedTaskIds.Add(entry.Key);
                }
            }

            if (_completedTaskIds.Count == 0) return;

            foreach (var id in _completedTaskIds)
            {
                _tasks.Remove(id);
            }
            _completedTaskIds.Clear();
        }

        private sealed class TaskWrapper
        {
            private readonly ITimedTask _task;
            private float _elapsedTime;
            private bool _completed;

            public TaskWrapper(ITimedTask task, float elapsedTime = 0f)
            {
                _task = task;
                _elapsedTime = Mathf.Max(0f, elapsedTime);
            }

            public bool IsCompleted => _completed;

            public void Start()
            {
                if (_completed) return;

                _task.OnStart();

                if (_elapsedTime > 0f)
                {
                    float clampedElapsed = Mathf.Min(_elapsedTime, _task.Duration);
                    _task.OnTick(clampedElapsed, 0f);
                    if (_elapsedTime >= _task.Duration)
                    {
                        Complete();
                    }
                }
            }

            public void Tick(float deltaTime)
            {
                if (_completed) return;

                var previousElapsed = _elapsedTime;
                _elapsedTime += deltaTime;

                var clampedElapsed = Mathf.Min(_elapsedTime, _task.Duration);
                _task.OnTick(clampedElapsed, deltaTime);

                if (previousElapsed < _task.Duration && _elapsedTime >= _task.Duration)
                {
                    Complete();
                }
            }

            public void Cancel()
            {
                if (_completed) return;
                _completed = true;
                _task.OnCancelled();
            }

            public TimedTaskState CreateState()
            {
                return _task.CreateState(Mathf.Min(_elapsedTime, _task.Duration));
            }

            private void Complete()
            {
                if (_completed) return;
                _completed = true;
                _task.OnComplete();
            }
        }
    }
}
