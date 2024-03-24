using System;
using System.Collections.Generic;

namespace UI.Implementation
{
    public class MiddlewareMap : IMiddlewareMap
    {
        public Dictionary<Type, Func<bool>> Map { get; }

        public MiddlewareMap()
        {
            Map = new Dictionary<Type, Func<bool>>();
        }

        public MiddlewareMap(Dictionary<Type, Func<bool>> map)
        {
            Map = map;
        }

        public void Add<T>(Func<bool> condition) where T : UIElement
        {
            Map.TryAdd(typeof(T), condition);
        }

        public void Remove<T>() where T : UIElement
        {
            Map.Remove(typeof(T));
        }
    }
}