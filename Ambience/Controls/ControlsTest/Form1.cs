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
            _sched.TicksPerSec = 2;

            BasicEvent.Provider prov1 = new BasicEvent.Provider("Chord");
            BasicEvent.Provider prov2 = new BasicEvent.Provider("Ding");
            //SoundEvent.Provider prov1 = new SoundEvent.Provider("Chord", @"C:\Windows\Media\chord.wav");
            //SoundEvent.Provider prov2 = new SoundEvent.Provider("Ding", @"C:\Windows\Media\ding.wav");
            PeriodicEventProvider per1 = new PeriodicEventProvider("Period");

            per1.Subordinate = prov1;
            per1.Period = 4;

            _colors[per1] = Color.Red;
            _colors[prov2] = Color.Orange;

            _sched.Initialize();
            _sched.AddProvider(per1);
            _sched.AddProvider(prov2);

            scheduleView1.ColorProvider = this;
            scheduleView1.Schedule = _sched;
            scheduleView1.TokenMouseEnter += new ScheduleView.TokenMouseEvent(scheduleView1_TokenMouseEnter);
            scheduleView1.TokenMouseLeave += new ScheduleView.TokenMouseEvent(scheduleView1_TokenMouseLeave);
            scheduleView1.LeftColumnChanged += new ScheduleView.ViewValueChangeEvent(scheduleView1_LeftColumnChanged);
            scheduleView1.TopRowChanged += new ScheduleView.ViewValueChangeEvent(scheduleView1_TopRowChanged);

            this.FormClosed += new FormClosedEventHandler(Form1_FormClosed);
        }

        void scheduleView1_TopRowChanged(ScheduleView sender, int oldValue, int newValue)
        {
            updateLabel2(newValue.ToString());
        }

        void scheduleView1_LeftColumnChanged(ScheduleView sender, int oldValue, int newValue)
        {
            updateLabel1(newValue.ToString());
        }

        private void updateLabel1(string text)
        {
            if (InvokeRequired)
                Invoke(new Action<string>(updateLabel1), text);
            else
                label1.Text = text;
        }

        private void updateLabel2(string text)
        {
            if (InvokeRequired)
                Invoke(new Action<string>(updateLabel2), text);
            else
                label2.Text = text;
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

        private void button1_Click(object sender, EventArgs e)
        {
            _sched.Start(false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _sched.Stop();
        }
    }
}
