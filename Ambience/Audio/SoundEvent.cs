using System;
using System.Collections.Generic;
using System.Text;
using Genesis.Ambience.Scheduler;
using NAudio;
using NAudio.Wave;
using NAudio.CoreAudioApi;

namespace Genesis.Ambience.Audio
{
    public class SoundEvent : IScheduleEvent
    {
        public class Provider : AEventProvider
        {
            public class Instance : AEventProviderInstance<Provider>
            {
                public Instance(Provider parent)
                    : base(parent, parent)
                {
                }

                public Instance(Provider parent, IEventProvider src)
                    : base(parent, src)
                {
                }

                public override bool Next(IEventScheduler sched, ulong currTimeCode, ulong span)
                {
                    sched.ScheduleEvent(_parent.CreateEvent(sched.TicksPerSec), currTimeCode);
                    return false;
                }
            }

            public Provider(string name, string filename)
                : base(name)
            {
                _filename = filename;
            }

            private string _filename;
            public string Filename
            {
                get { return _filename; }
            }

            #region IEventProvider Members

            public override bool DependsOn(IEventProvider dependent)
            {
                throw new NotImplementedException();
            }

            public override IEventProviderInstance CreateInstance()
            {
                return new Instance(this);
            }

            public override IEventProviderInstance CreateInstance(IEventProvider src)
            {
                return new Instance(this, src);
            }

            #endregion

            private List<SoundEvent> _all = new List<SoundEvent>();
            private SoundEvent CreateEvent(uint timePerSecond)
            {
                WaveStream reader;
                if (_filename.EndsWith(".mp3"))
                {
                    Mp3FileReader mp3 = new Mp3FileReader(_filename);
                    reader = mp3;
                }
                else if (_filename.EndsWith(".wav"))
                {
                    WaveFileReader wav = new WaveFileReader(_filename);
                    reader = wav;
                }
                else
                {
                    throw new InvalidOperationException("Unsupported extension.");
                }

                SoundEvent evt = new SoundEvent(this, reader, timePerSecond);
                _all.Add(evt);
                return evt;
            }
        }

        private WaveStream _reader;
        private WaveChannel32 _stream;
        private IWavePlayer _wavDevice = new WaveOut();
        private static int s_next = 1;
        private int _id = s_next++;
        
        public SoundEvent(Provider src, WaveStream wav, uint timePerSecond)
        {
            Console.WriteLine("{0} created", _id);
            _source = src;
            _reader = wav;
            _stream = new WaveChannel32(wav);
            _stream.PadWithZeroes = false;
            _length = (ulong)Math.Ceiling(_stream.TotalTime.TotalSeconds * (double)timePerSecond);
            if (_length < 1)
                _length = 1;
            _wavDevice.Init(_stream);
        }

        private void PlaybackFinished(object sender, EventArgs e)
        {
            _active = false;
        }

        #region IScheduleEvent Members

        private ulong _length;
        public ulong Length
        {
            get { return _length; }
        }

        private bool _active = false;
        public bool Active
        {
            get { return _active; }
        }

        private string _name = "";
        public string Name
        {
            get { return _name; }
        }

        private Provider _source;
        public IEventProvider Source
        {
            get { return _source; }
        }

        public void Start()
        {
            //Console.WriteLine("Event started");
            _active = true;
            _wavDevice.PlaybackStopped += new EventHandler(PlaybackFinished);
            _wavDevice.Play();
        }

        public void Update()
        {
            //throw new NotImplementedException();
        }

        public void Stop()
        {
            //Console.WriteLine("Event Stopped");
            _wavDevice.Stop();
            Console.WriteLine("{0} disposed", _id);
            _wavDevice.Dispose();
            _stream.Close();
            _reader.Close();
        }

        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            _wavDevice.Dispose();
            _stream.Close();
            _reader.Close();
        }
        #endregion
    }
}
