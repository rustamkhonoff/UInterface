using System;
using System.Collections.Generic;

namespace UInterface.Implementation
{
    public interface IMiddlewareMap
    {
        Dictionary<Type, Func<bool>> Map { get; }
    }
}