using System;
using Interfaces;
using UnityEngine;

namespace Core
{
    public abstract class UIElement : MonoBehaviour
    {
        public IElementEventHandler ElementEventHandler { get; private set; } = new DefaultElementEventHandler();

        protected IUIService UIService { get; private set; }

        internal void SetUIService(IUIService uiService) => UIService = uiService;

        public event Action Destroying;

        public virtual void Initialize() { }

        public virtual void Dispose() { }

        public virtual void Hide()
        {
            ElementEventHandler.HandleHide(() => gameObject.SetActive(false));
        }

        public void SetEventHandler(IElementEventHandler elementEventHandler)
        {
            Debug.Log($"Set to {elementEventHandler}");
            ElementEventHandler = elementEventHandler;
        }

        public virtual void Hide(Action onHided)
        {
            ElementEventHandler.HandleHide(() =>
            {
                gameObject.SetActive(false);
                onHided?.Invoke();
            });
        }

        public void HideAndDestroy()
        {
            Hide(DestroySelf);
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