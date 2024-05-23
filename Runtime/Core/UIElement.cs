using System;
using Extras.EventHandlers.Base;
using Interfaces;
using UnityEngine;

namespace Core
{
    public abstract class UIElement : MonoBehaviour
    {
        public IElementEventHandler ElementEventHandler { get; private set; } = new DefaultElementEventHandler();
        public ElementTimeScale.TimeScaleType TimeScaleType { get; private set; } = ElementTimeScale.TimeScaleType.Scaled;

        protected IUIService UIService { get; private set; }

        internal void SetUIService(IUIService uiService) => UIService = uiService;

        public event Action Destroying;

        public virtual void Initialize() { }

        public virtual void Dispose() { }

        public virtual void Hide()
        {
            ElementEventHandler.HandleHide(DestroySelf);
        }

        public void SetEventHandler(IElementEventHandler elementEventHandler) => ElementEventHandler = elementEventHandler;
        public void SetTimeScale(ElementTimeScale.TimeScaleType timeScaleType) => TimeScaleType = timeScaleType;

        public virtual void Hide(Action onHided)
        {
            ElementEventHandler.HandleHide(() =>
            {
                gameObject.SetActive(false);
                onHided?.Invoke();
            });
        }

        public virtual void Show()
        {
            ElementEventHandler.HandleShow(() => gameObject.SetActive(true));
        }

        private void OnDestroy()
        {
            Destroying?.Invoke();
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}