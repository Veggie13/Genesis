using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Wave;
using System.Threading;
using System.IO;
using Genesis.Ambience.Scheduler;

namespace Genesis.Ambience.Audio
{
    public class SoundResource : IDisposable, SoundEvent.IResource
    {
        private ResourceManager _manager;
        private WaveStream _reader;
        private WaveChannel32 _stream;
        private IWavePlayer _wavDevice = new WaveOut(WaveCallbackInfo.FunctionCallback());

        public SoundResource(ResourceManager mgr, string fullName, Stream stream, Format fmt)
        {
            _manager = mgr;
            FullName = fullName;

            switch (fmt)
            {
                case Format.MP3:
                {
                    Mp3FileReader mp3 = new Mp3FileReader(stream);
                    _reader = mp3;
                    break;
                }
                case Format.WAV:
                {
                    WaveFileReader wav = new WaveFileReader(stream);
                    _reader = wav;
                    break;
                }
                default:
                    throw new InvalidOperationException("Unsupported extension.");
            }

            _stream = new WaveChannel32(_reader);
            _stream.PadWithZeroes = false;

            _wavDevice.PlaybackStopped += _wavDevice_PlaybackStopped;
        }

        public event Action PlaybackStopped = () => { };

        private void _wavDevice_PlaybackStopped(object sender, EventArgs e)
        {
            PlaybackStopped();
        }

        public double Length
        {
            get
            {
                return _stream.TotalTime.TotalSeconds;
            }
        }

        public string FullName
        {
            get;
            private set;
        }

        internal void init()
        {
            _manager.syncAction(() =>
            {
                _wavDevice.Init(_stream);
            });
        }

        public void Play()
        {
            _manager.syncAction(() =>
            {
                if (_wavDevice.PlaybackState != PlaybackState.Playing)
                {
                    _reader.Seek(0, SeekOrigin.Begin);
                    _wavDevice.Play();
                }
            });
        }

        public void Stop()
        {
            _manager.syncAction(() =>
            {
                _wavDevice.Stop();
            });
        }

        public void Dispose()
        {
            AutoResetEvent signal = new AutoResetEvent(false);
            _manager.syncAction(() =>
            {
                _wavDevice.Stop();
                _wavDevice.Dispose();
                _stream.Close();
                _reader.Close();
                signal.Set();
            });
            signal.WaitOne();
        }
    }
}
