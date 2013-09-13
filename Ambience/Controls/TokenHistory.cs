using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genesis.Ambience.Scheduler;
using Genesis.Common.Tools;

namespace Genesis.Ambience.Controls
{
    class TokenHistory
    {
        #region Class Members
        private int _rowCount = 4;
        private Dictionary<ulong, EventToken[,]> _history = new Dictionary<ulong, EventToken[,]>();
        #endregion

        public TokenHistory()
        {
            BlockWidth = 1;
            ColumnCount = 1;
        }

        #region Properties
        public int BlockWidth
        {
            get;
            set;
        }

        public IEventColorProvider ColorProvider
        {
            get;
            set;
        }

        public ulong ColumnCount
        {
            get;
            private set;
        }

        public EventToken this[int row, ulong time]
        {
            get
            {
                int timeIndex;
                EventToken[,] block;
                getBaseTime(time, out block, out timeIndex);
                if (row >= block.GetLength(0))
                    return null;

                return block[row, timeIndex];
            }
        }

        public EventToken[] this[ulong time]
        {
            get
            {
                EventToken[,] block;
                int index;
                getBaseTime(time, out block, out index);

                if (block == null)
                    return new EventToken[0];

                var result = new List<EventToken>();
                for (int row = 0; row < block.GetLength(0); row++)
                {
                    result.Add(block[row, index]);
                }

                return result.Take(result.FindLastIndex(t => (t != null)) + 1).ToArray();
            }
        }
        #endregion

        #region Public Operations
        public void Clear()
        {
            _history.Clear();
        }
        
        public void UpdateFuture(EventSchedule sched, ulong fromTime)
        {
            var future = sched.GetActualFuture(fromTime);

            acrossTime(fromTime, ColumnCount, (ref EventToken[,] block) =>
            {
                return (block != null);
            }, (ref EventToken[,] block, ulong t, int j) =>
            {
                for (int r = 0; r < block.GetLength(0); r++)
                {
                    if (block[r, j] != null && (!block[r, j].Event.Started || !block[r, j].Event.Active))
                    {
                        block[r, j] = null;
                    }
                }
            });

            acrossTime(fromTime, (ulong)future.Length, (ref EventToken[,] block) =>
            {
                if (block == null)
                    block = new EventToken[_rowCount, BlockWidth];
                return true;
            }, (ref EventToken[,] block, ulong t, int j) =>
            {
                foreach (var evt in future[(int)t])
                {
                    int r = findEmptyRow(ref block, j);
                    insertEvent(r, fromTime + t, evt);
                }
            });
        }
        #endregion

        #region Private Helpers
        private void getBaseTime(ulong time, out EventToken[,] block, out int index)
        {
            ulong baseTime = _history.Keys.Where(t => (t <= time)).DefaultIfEmpty(0UL).Max();
            if (!_history.ContainsKey(baseTime))
            {
                block = null;
                index = 0;
                return;
            }

            block = _history[baseTime];
            index = (int)(time - baseTime);
            if (index >= block.GetLength(1))
            {
                block = null;
                index = 0;
            }
        }

        private void insertEvent(int row, ulong time, IScheduleEvent evt)
        {
            var token = new EventToken((int)time, evt, ColorProvider);

            acrossTime(time, (ulong)evt.Length, (ref EventToken[,] block) =>
            {
                if (block == null)
                    block = new EventToken[_rowCount, BlockWidth];
                if (block.GetLength(0) <= row)
                    block = block.Resize(_rowCount, block.GetLength(1));
                return true;
            }, (ref EventToken[,] block, ulong t, int i) =>
            {
                block[row, i] = token;
            });

            if (time + (ulong)evt.Length > ColumnCount)
                ColumnCount = time + (ulong)evt.Length;
        }

        private delegate bool BlockValidator(ref EventToken[,] block);
        private delegate void TimeOperation(ref EventToken[,] block, ulong dt, int blockCol);
        private void acrossTime(ulong startTime, ulong timeLen, BlockValidator validator, TimeOperation op)
        {
            for (ulong dt = 0; dt < timeLen; )
            {
                int begin;
                EventToken[,] block;
                ulong time = startTime + dt;
                getBaseTime(time, out block, out begin);
                time -= (ulong)begin;

                if (!validator(ref block) || block == null)
                {
                    dt = _history.Keys.Where(t => (t > time))
                        .DefaultIfEmpty(startTime + timeLen)
                        .Min() - startTime;
                    continue;
                }

                _history[time] = block;

                for (int blockIndex = begin; blockIndex < block.GetLength(1) && dt < timeLen; blockIndex++, dt++)
                {
                    op(ref block, dt, blockIndex);
                }
            }
        }

        private int findEmptyRow(ref EventToken[,] block, int col)
        {
            bool found = false;
            int row;
            for (row = 0; row < block.GetLength(0); row++)
            {
                if (block[row, col] == null)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                row = (++_rowCount) - 1;
                block = block.Resize(_rowCount, block.GetLength(1));
            }

            return row;
        }
        #endregion
    }
}
