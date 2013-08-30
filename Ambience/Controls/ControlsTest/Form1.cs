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
using System.IO;

namespace ControlsTest
{
    public partial class Form1 : Form, IEventColorProvider
    {
        private EventSchedule _sched;
        private ResourceManager _manager;
        private Dictionary<IEventProvider, Color> _colors = new Dictionary<IEventProvider, Color>();
        private RandomEventSelector _randEventSel = new RandomEventSelector("Randy");
        DelayEventProvider del1 = new DelayEventProvider("Delay1");
        PeriodicEventProvider per2 = new PeriodicEventProvider("Period2");

        public Form1()
        {
            InitializeComponent();

            _manager = new ResourceManager();
            ILibrary lib = _manager.LoadLibrary(@"C:\Corey Derochie\ATCNY");
            ILibrary lib2 = _manager.LoadLibrary(@"C:\Corey Derochie\Paramore");
            _manager.Start();
            libraryView1.Resources = _manager;

            _sched = new EventSchedule(32);
            _sched.TicksPerSec = 2;

            BasicEvent.Provider prov1 = new BasicEvent.Provider("Chord");
            BasicEvent.Provider prov2 = new BasicEvent.Provider("Ding");
            //SoundEvent.Provider prov1 = new SoundEvent.Provider("Chord", _manager, "chord.wav");
            //SoundEvent.Provider prov2 = new SoundEvent.Provider("Ding", _manager, "krkfunny.WAV");
            PeriodicEventProvider per1 = new PeriodicEventProvider("Period");
            //SimultaneousEventProvider sim2 = new SimultaneousEventProvider("Simul");
            _randEventSel.Selection.Add(prov1);
            _randEventSel.Selection.Add(prov2);

            //sim2.Group.Add(prov1);
            //sim2.Group.Add(prov2);
            
            per1.Subordinate = prov1;
            per1.Period = 4;

            per2.Subordinate = prov2;
            per2.Period = 5;

            del1.Subordinate = prov2;
            del1.Delay = 5;

            _colors[per1] = Color.Red;
            _colors[prov2] = Color.Orange;
            _colors[prov1] = Color.Green;

            _sched.Initialize();
            _sched.AddProvider(prov2);
            _sched.AddProvider(per1);
            //_sched.AddProvider(del1);
            _sched.AddProvider(per2);
            //_sched.AddProvider(prov2);

            scheduleView1.ColorProvider = this;
            scheduleView1.Schedule = _sched;
            scheduleView1.TokenMouseEnter += new ScheduleView.TokenMouseEvent(scheduleView1_TokenMouseEnter);
            scheduleView1.TokenMouseLeave += new ScheduleView.TokenMouseEvent(scheduleView1_TokenMouseLeave);
            scheduleView1.LeftColumnChanged += new ScheduleView.ViewValueChangeEvent(scheduleView1_LeftColumnChanged);
            scheduleView1.TopRowChanged += new ScheduleView.ViewValueChangeEvent(scheduleView1_TopRowChanged);

            eventTokenTile1.Token = new ProviderToken(prov1, this);

            providerTokenList1.AllowItemDrag = false;
            providerTokenList1.Items.Add(prov1);
            providerTokenList1.Items.Add(prov2);

            eventTokenTile2.MouseMove += new MouseEventHandler(eventTokenTile2_MouseMove);
            eventTokenTile2.MouseLeave += new EventHandler(eventTokenTile2_MouseLeave);
            eventTokenTile2.MouseUp += new MouseEventHandler(eventTokenTile2_MouseLeave);

            providerTokenButton1.TileClicked += new ProviderTokenButton.TileClickedEvent(providerTokenButton1_TileClicked);

            this.Load += new EventHandler(Form1_Load);
            this.FormClosed += new FormClosedEventHandler(Form1_FormClosed);
        }

        void providerTokenButton1_TileClicked(ProviderToken token)
        {
            MessageBox.Show(token.Name);
        }

        void eventTokenTile2_MouseLeave(object sender, EventArgs e)
        {
            eventTokenTile2.InnerText = "";
        }

        void eventTokenTile2_MouseMove(object sender, MouseEventArgs e)
        {
            eventTokenTile2.InnerText = "Drop";
        }

        void Form1_Load(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.eventProviderEditorControl1.Provider = per2;
            frm.eventProviderEditorControl1.ColorProvider = this;
            frm.Show();

            Form3 frm3 = new Form3();
            frm3.tokenBoard1.ColumnCount = 5;
            frm3.Show();
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

            _manager.Stop();
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
            get { return !_colors.ContainsKey(evt.Source) ? Color.Gray : _colors[evt.Source]; }
        }

        public Color this[IEventProvider prov]
        {
            get { return !_colors.ContainsKey(prov) ? Color.Gray : _colors[prov]; }
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
