using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genesis.Ambience.Audio;

namespace Genesis.Ambience.DataModel
{
    public class SoundEventElement : AEventElement<SoundEvent.Provider>
    {
        public SoundEventElement(SoundEvent.Provider provider)
            : base(provider)
        {
        }

        public override void Accept(IEventElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
