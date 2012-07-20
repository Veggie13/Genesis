using System;
using System.Collections.Generic;
using System.Text;
using Genesis.Ambience.Scheduler;
using Genesis.Ambience.Audio;
using System.Threading;
using Genesis.Common.API.Comms;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Remoting.Channels.Tcp;

namespace SchedulerTest
{
    class Program : AGenesisApplication, IClientChannelDetailProvider
    {
        static Program _this = new Program();

        static ulong _maxTime = 40;

        static void Ticker(EventSchedule sched, ulong newTimeCode)
        {
            Console.WriteLine("Time {0}:", newTimeCode);
            //if (newTimeCode >= _maxTime)
            //    sched.Stop();
        }

        static void Main(string[] args)
        {
            _this.giver();
        }

        void giver()
        {
            EventSchedule sched = new EventSchedule(32);
            sched.TicksPerSec = 1;

            //BasicEvent.Provider prov = new BasicEvent.Provider();
            SoundEvent.Provider prov1 = new SoundEvent.Provider("Chord", @"C:\Windows\Media\chord.wav");
            SoundEvent.Provider prov2 = new SoundEvent.Provider("Ding", @"C:\Windows\Media\ding.wav");
            /*RandomEventSelector rnd = new RandomEventSelector();
            rnd.Selection.Add(prov1);
            rnd.Selection.Add(prov2);
            SimultaneousEventProvider sim = new SimultaneousEventProvider();
            sim.Group.Add(prov1);
            sim.Group.Add(prov2);
            PeriodicEventProvider per = new PeriodicEventProvider();
            per.Period = 8;
            per.Variance = 0;
            per.Subordinate = sim;
             */

            _hub.SetChannel(this);
            _hub.Initialize(_this);

            var obs = new EventProviderActivator(AppID);
            obs.Provider = prov1;
            obs.Schedule = sched;
            var id = new TriggerID("Test", 1);
            bool result = _hub.Hub.ConnectObserver(id, obs);

            //sched.Model.Add(per);
            sched.Tick += new EventSchedule.TickEvent(Ticker);
            sched.Start(false);

            //AutoResetEvent auto = new AutoResetEvent(false);
            //while (sched.IsRunning)
            //    auto.WaitOne(1000);

            Console.Write("Press any key...");
            Console.ReadLine();

            sched.Stop();
            _hub.Finish(_this);

            Console.WriteLine("Finished.");
            Console.ReadLine();

            return;
        }

        #region IGenesisApplication Members

        private ClientHubProvider _hub = new ClientHubProvider();
        protected override IHubProvider HubProvider
        {
            get { return _hub; }
        }

        public override string AppName
        {
            get { return "MyClient"; }
        }

        public override ChannelMode Channel
        {
            get { return ChannelMode.TCPIP; }
        }

        public override void OnDisconnect()
        {
            
        }

        #endregion

        #region IClientChannelDetailProvider Members
        public ChannelMode Mode
        {
            get { return Channel; }
        }

        public string URL
        {
            get { return "tcp://localhost:13584"; }
        }
        #endregion
    }
}
