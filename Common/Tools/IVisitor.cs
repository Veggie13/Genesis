using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genesis.Common.Tools
{
    public interface IVisitable<V, T>
        where V : IVisitor<V, T>
        where T : IVisitable<V, T>
    {
        void Accept(V visitor);
    }
    
    public interface IVisitor<V, T>
        where V : IVisitor<V, T>
        where T : IVisitable<V, T>
    {
        void Visit(IVisitable<V, T> item);
    }

    public static class VisitorExtensions
    {
        public static void TryVisit<V, T>(this V me, IVisitable<V, T> item)
            where V : IVisitor<V, T>
            where T : IVisitable<V, T>
        {
            item.Accept(me);
        }
    }
}
