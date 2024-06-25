using System;

namespace UInterface
{
    public interface IElementEventHandler
    {
        void HandleShow(Action showAction);
        void HandleHide(Action hideAction);
    }
}