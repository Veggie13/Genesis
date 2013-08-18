﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genesis.Ambience.Scheduler;

namespace Genesis.Ambience.DataModel
{
    public class DelayEventElement : AEventElement<DelayEventProvider>
    {
        public DelayEventElement(DelayEventProvider provider)
            : base(provider)
        {
        }

        public override void Accept(IEventElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
