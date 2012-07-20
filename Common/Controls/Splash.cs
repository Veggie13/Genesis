using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Genesis.Common.Controls
{
    public interface ISplashMessenger
    {
        string Message { set; }
    }

    public partial class Splash : Form, ISplashMessenger
    {
        public interface Worker
        {
            void DoWork(ISplashMessenger msgr);
        }

        private class DelayWorker : Worker
        {
            private int _msDelay = 3000;
            public DelayWorker(int delay)
            {
                _msDelay = delay;
            }

            public void DoWork(ISplashMessenger msgr)
            {
                msgr.Message = "Loading...";
                Thread.Sleep(_msDelay);
            }
        }

        public static Worker BasicDelay(int delay)
        {
            return new DelayWorker(delay);
        }

        public static void DoSplash(Image bg, Worker worker)
        {
            Splash _this = new Splash(worker);
            if (bg != null)
            {
                _this.BackgroundImage = bg;
                _this.BackgroundImageLayout = ImageLayout.Stretch;
            }
            _this.ShowDialog();
        }

        private Worker _worker;
        private Thread _work;
        private Splash(Worker worker)
        {
            InitializeComponent();
            _worker = worker;
        }

        private delegate void CloseDelegate();
        private void DoWork()
        {
            _worker.DoWork(this);
            Invoke(new CloseDelegate(Close));
        }

        public string Message
        {
            set
            {
                if (InvokeRequired)
                    Invoke(new UpdateMessageDelegate(UpdateMessage), value);
                else
                    UpdateMessage(value);
            }
        }

        private delegate void UpdateMessageDelegate(string msg);
        private void UpdateMessage(string msg)
        {
            _message.Text = msg;
        }

        private void Splash_Load(object sender, EventArgs e)
        {
            _message.Text = "";

            _work = new Thread(new ThreadStart(this.DoWork));
            _work.Start();
        }
    }
}
