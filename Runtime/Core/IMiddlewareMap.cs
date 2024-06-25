using System;
using System.Collections.Generic;

namespace UInterface.Core
{
    public interface IMiddlewareMap
    {
        Dictionary<Type, Func<bool>> Map { get; }
    }
}