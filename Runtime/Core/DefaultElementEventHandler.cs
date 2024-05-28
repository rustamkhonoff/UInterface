using System;

namespace UInterface
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