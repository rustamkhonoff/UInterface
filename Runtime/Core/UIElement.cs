using System;
using UInterface.Extras.EventHandlers;
using UnityEngine;

namespace UInterface
{
    public abstract class UIElement : MonoBehaviour
    {
        public IElementEventHandler ElementEventHandler { get; private set; } = new DefaultElementEventHandler();
        public ElementTimeScale.TimeScaleType TimeScaleType { get; private set; } = ElementTimeScale.TimeScaleType.Scaled;

        protected IUIService UIService { get; private set; }

        internal void SetUIService(IUIService uiService) => UIService = uiService;

        public event Action Destroying;

        public virtual void Initialize() { }
        public void Hide() => Hide(null);

        public void SetEventHandler(IElementEventHandler elementEventHandler) => ElementEventHandler = elementEventHandler;
        public void SetTimeScale(ElementTimeScale.TimeScaleType timeScaleType) => TimeScaleType = timeScaleType;

        public void Hide(Action onHided)
        {
            OnHiding();
            onHided += OnHided;
            onHided += DestroySelf;
            ElementEventHandler.HandleHide(onHided);
        }

        public void Show() => Show(null);

        public void Show(Action action)
        {
            OnShowing();
            action += OnShown;
            ElementEventHandler.HandleShow(action);
        }

        protected virtual void OnHiding() { }
        protected virtual void OnHided() { }
        protected virtual void OnShowing() { }
        protected virtual void OnShown() { }
        public virtual void Dispose() { }

        private void OnDisable()
        {
            Dispose();
            Destroying?.Invoke();
        }

        public void DestroySelf() => Destroy(gameObject);
    }
}