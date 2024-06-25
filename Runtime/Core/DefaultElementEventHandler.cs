using System;

namespace UInterface.Core
{
    public class DefaultElementEventHandler : IElementEventHandler
    {
        public void HandleShow(Action showAction)
        {
            showAction?.Invoke();
        }

        public void HandleHide(Action hideAction)
        {
            hideAction?.Invoke();
        }
    }
}