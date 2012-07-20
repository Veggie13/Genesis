using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genesis.Ambience.Scheduler;
using System.Drawing;

namespace Genesis.Ambience.Controls
{
    public class EventToken
    {
        private int _start;
        private IScheduleEvent _event;
        private IEventColorProvider _colorer;

        public EventToken(int start, IScheduleEvent evt, IEventColorProvider colorer)
        {
            _start = start;
            _event = evt;
            _colorer = colorer;
        }

        public void Finish()
        {
            _event = null;
        }
        
        public IScheduleEvent Event
        {
            get { return _event; }
        }
        
        public string Name
        {
            get { return _event.Source.Name; }
        }

        public Color Color
        {
            get
            {
                if (_colorer == null || _event == null)
                    return Color.Gray;
                return _colorer[_event];
            }
        }

        public int Length
        {
            get { return (int)_event.Length; }
        }

        public int Start
        {
            get { return _start; }
        }

        public int Last
        {
            get { return _start + Length - 1; }
        }

        private bool _highlit = false;
        public bool IsHighlighted
        {
            get { return _highlit; }
            set { _highlit = value; }
        }
    }
}
