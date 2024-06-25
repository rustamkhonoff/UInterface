using System;
using System.Collections.Generic;

namespace UInterface
{
    public interface IMiddlewareMap
    {
        Dictionary<Type, Func<bool>> Map { get; }
    }
}