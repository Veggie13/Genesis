using System;
using System.Collections.Generic;
using System.Text;

namespace Genesis.Ambience.Scheduler
{
    public interface IScheduleEvent : IDisposable
    {
        ulong Length { get; }
        bool Active { get; }
        bool Started { get; }
        string Name { get; }
        IEventProvider Source { get; }

        void Start();
        void Update();
        void Stop();
    }
}
