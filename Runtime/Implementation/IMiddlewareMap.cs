using System;
using System.Collections.Generic;

namespace Implementation
{
    public interface IMiddlewareMap
    {
        Dictionary<Type, Func<bool>> Map { get; }
    }
}