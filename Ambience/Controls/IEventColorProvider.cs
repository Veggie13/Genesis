using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Genesis.Ambience.Scheduler;

namespace Genesis.Ambience.Controls
{
    public interface IEventColorProvider
    {
        Color this[IScheduleEvent evt] { get; }
    }
}
