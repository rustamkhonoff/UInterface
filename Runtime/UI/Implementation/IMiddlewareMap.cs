using System;
using System.Collections.Generic;

namespace UI.Implementation
{
    public interface IMiddlewareMap
    {
        Dictionary<Type, Func<bool>> Map { get; }
    }
}