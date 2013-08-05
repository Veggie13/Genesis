using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genesis.Ambience.Scheduler
{
    public interface IEventProviderFactory
    {
        IEventProvider CreateProvider();
    }
}
