using System;

namespace UInterface.Core
{
    public interface IElementEventHandler
    {
        void HandleShow(Action showAction);
        void HandleHide(Action hideAction);
    }
}