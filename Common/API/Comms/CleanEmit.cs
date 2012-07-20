using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genesis.Common.API.Comms
{
    class Clean
    {
        public class Helper<D>
        {
            public readonly Action<D> Unsubscribe;
            private Func<MulticastDelegate> _eventGetter;
            public MulticastDelegate Event
            {
                get { return _eventGetter(); }
            }

            public Helper(Func<MulticastDelegate> d, Action<D> u)
            {
                _eventGetter = d;
                Unsubscribe = u;
            }
        }

        public static void Emit<D>(Helper<D> h) where D : class
        {
            CheckType<D>();
            foreach (Delegate f in h.Event.GetInvocationList())
            {
                D a = f as D;
                try
                {
                    if (f.Target is IGenesisObject && !(f.Target as IGenesisObject).IsConnected())
                        h.Unsubscribe(a);
                    else
                        f.DynamicInvoke();
                }
                catch (Exception)
                {
                    h.Unsubscribe(a);
                }
            }
        }

        public static void Emit<D, T>(Helper<D> h, T a1) where D : class
        {
            CheckType<D>();
            foreach (Delegate f in h.Event.GetInvocationList())
            {
                D a = f as D;
                try
                {
                    if (f.Target is IGenesisObject && !(f.Target as IGenesisObject).IsConnected())
                        h.Unsubscribe(a);
                    else
                        f.DynamicInvoke(a1);
                }
                catch (Exception)
                {
                    h.Unsubscribe(a);
                }
            }
        }

        public static void Emit<D, T1, T2>(Helper<D> h, T1 a1, T2 a2) where D : class
        {
            CheckType<D>();
            foreach (Delegate f in h.Event.GetInvocationList())
            {
                D a = f as D;
                try
                {
                    if (f.Target is IGenesisObject && !(f.Target as IGenesisObject).IsConnected())
                        h.Unsubscribe(a);
                    else
                        f.DynamicInvoke(a1, a2);
                }
                catch (Exception)
                {
                    h.Unsubscribe(a);
                }
            }
        }

        private static void CheckType<D>()
        {
            if (!(typeof(D).IsSubclassOf(typeof(Delegate))))
                throw new InvalidOperationException("Emit<D...> : D must be Delegate");
        }
    }
}
