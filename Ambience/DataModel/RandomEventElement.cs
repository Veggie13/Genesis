using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genesis.Ambience.Scheduler;

namespace Genesis.Ambience.DataModel
{
    public class RandomEventElement : AEventElement<RandomEventSelector>
    {
        public RandomEventElement(RandomEventSelector provider)
            : base(provider)
        {
        }

        public override void Accept(IEventElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
