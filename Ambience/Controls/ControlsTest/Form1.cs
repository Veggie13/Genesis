using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Genesis.Ambience.Scheduler;
using Genesis.Ambience.Audio;
using Genesis.Ambience.Controls;

namespace ControlsTest
{
    public partial class Form1 : Form, IEventColorProvider
    {
        private EventSchedule _sched;
        private Dictionary<IEventProvider, Color> _colors = new Dictionary<IEventProvider, Color>();

        public Form1()
        {
            InitializeComponent();

            _sched = new EventSchedule(32);
            _sched.TicksPerSec = 1;

            //BasicEvent.Provider prov = new BasicEvent.Provider();
            SoundEvent.Provider prov1 = new SoundEvent.Provider("Chord", @"C:\Windows\Media\chord.wav");
            SoundEvent.Provider prov2 = new SoundEvent.Provider("Ding", @"C:\Windows\Media\ding.wav");

            _colors[prov1] = Color.Red;
            _colors[prov2] = Color.Orange;

            _sched.Initialize();
            _sched.AddProvider(prov1);
            _sched.AddProvider(prov2);

            scheduleView1.ColorProvider = this;
            scheduleView1.Schedule = _sched;
            scheduleView1.TokenMouseEnter += new ScheduleView.TokenMouseEvent(scheduleView1_TokenMouseEnter);
            scheduleView1.TokenMouseLeave += new ScheduleView.TokenMouseEvent(scheduleView1_TokenMouseLeave);

            this.FormClosed += new FormClosedEventHandler(Form1_FormClosed);
        }

        void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _sched.Dispose();
            _sched = null;
        }

        void scheduleView1_TokenMouseLeave(EventToken token, Control sender, Point loc)
        {
            token.IsHighlighted = false;
            scheduleView1.Invalidate();
        }

        void scheduleView1_TokenMouseEnter(EventToken token, Control sender, Point loc)
        {
            token.IsHighlighted = true;
            scheduleView1.Invalidate();
        }

        public Color this[IScheduleEvent evt]
        {
            get { return _colors[evt.Source]; }
        }
    }
}
