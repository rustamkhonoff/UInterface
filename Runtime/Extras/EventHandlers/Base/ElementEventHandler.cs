using System;
using UInterface.Core;
using UnityEngine;

namespace UInterface.Extras
{
    [RequireComponent(typeof(UIElement))]
    public abstract class ElementEventHandler : MonoBehaviour, IElementEventHandler
    {
        [Serializable]
        public enum Events
        {
            All = 0,
            Show = 1,
            Hide = 2
        }

        [SerializeField] private Events _eventsType;
        [SerializeField] private bool _autoRegister = true;
        [SerializeField] protected float _showDelay, _hideDelay;

        private UIElement m_element;

        public float RealShowCost => EventsType is Events.Show or Events.All ? ShowCost + _showDelay : 0f;
        public float RealHideCost => EventsType is Events.Hide or Events.All ? HideCost + _hideDelay : 0f;

        protected virtual float ShowCost => 0f;
        protected virtual float HideCost => 0f;

        public Events EventsType => _eventsType;

        private void Awake()
        {
            m_element = GetComponent<UIElement>();
            if (_autoRegister)
                m_element.ElementEventHandler = this;

            Initialize();
        }


        private void OnDestroy()
        {
            Dispose();
        }

        public void HandleShow(Action showAction)
        {
            if (EventsType is Events.All or Events.Show)
                OnHandleShow(showAction);
        }

        public void HandleHide(Action hideAction)
        {
            if (EventsType is Events.All or Events.Hide)
                OnHandleHide(hideAction);
        }

        public void SetAutoRegister(bool state) => _autoRegister = state;
        protected abstract void OnHandleShow(Action showAction);
        protected abstract void OnHandleHide(Action hideAction);

        public virtual void HandleNewHandlerAdded()
        {
        }

        private void Reset()
        {
            BroadcastMessage(nameof(HandleNewHandlerAdded));
        }

        public virtual void Dispose()
        {
        }

        public virtual void Initialize()
        {
        }
    }
}