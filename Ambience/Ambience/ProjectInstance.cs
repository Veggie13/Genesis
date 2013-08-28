using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genesis.Ambience.Audio;
using Genesis.Ambience.Scheduler;
using Genesis.Ambience.Controls;
using Genesis.Common.Tools;

namespace Genesis.Ambience
{
    class ProjectInstance : TokenBoard.ITokenBoardProvider
    {
        #region Class Members
        private const int DefaultRowCount = 10;
        private const int DefaultColumnCount = 10;

        private ResourceManager _resourceMgr = new ResourceManager();
        private EventSchedule _sched = new EventSchedule(256);
        private IEventProvider[,] _providers = new IEventProvider[DefaultRowCount, DefaultColumnCount];
        #endregion

        public ProjectInstance()
        {
            init();
        }

        #region Public Operations
        public List<SoundEvent.Provider> GetSounds()
        {
            return _resourceMgr.GetAllSounds()
                .Select(s => new SoundEvent.Provider(s, _resourceMgr, s))
                .ToList();
        }

        public void Close()
        {
            _sched.Dispose();
            _resourceMgr.Stop();
        }
        #endregion

        #region Properties
        public EventSchedule Schedule
        {
            get { return _sched; }
        }

        public ResourceManager Resources
        {
            get { return _resourceMgr; }
        }
        #endregion

        #region ITokenBoardProvider
        public IEventProvider this[int row, int col]
        {
            get
            {
                return _providers[row, col];
            }
            set
            {
                _providers[row, col] = value;
            }
        }

        public int RowCount
        {
            get
            {
                return _providers.GetLength(0);
            }
            set
            {
                _providers = _providers.Resize(value, ColumnCount);
            }
        }

        public int ColumnCount
        {
            get
            {
                return _providers.GetLength(1);
            }
            set
            {
                _providers = _providers.Resize(RowCount, value);
            }
        }
        #endregion

        #region Private Helpers
        private void init()
        {
            _sched.TicksPerSec = 4;
            _sched.Initialize();

            _resourceMgr.Start();
        }
        #endregion
    }
}
