using System;
using Core;
using UnityEngine;

namespace Extras.EventHandlers.Base
{
    [Serializable]
    public class TimingsData
    {
        [SerializeField] private float _delay;
        [SerializeField] private float _duration;

        public float Duration
        {
            get => _duration;
            set => _duration = value;
        }

        public float Delay
        {
            get => _delay;
            set => _delay = value;
        }
    }

    public interface ICompositeElementEventHandler { }

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
        [SerializeField] protected TimingsData _showData, _hideData;

        public float RealShowCost => EventsType is Events.Show or Events.All ? ShowCost + ShowData.Delay + ShowData.Duration : 0f;
        public float RealHideCost => EventsType is Events.Hide or Events.All ? HideCost + HideData.Delay + HideData.Duration : 0f;

        protected virtual float ShowCost => 0f;
        protected virtual float HideCost => 0f;

        public Events EventsType => _eventsType;

        public TimingsData ShowData => _showData;

        public TimingsData HideData => _hideData;

        private void Awake()
        {
            if (!TryGetComponent<ICompositeElementEventHandler>(out _))
                SetEventHandler(this);

            OnAwake();
        }

        protected void SetEventHandler(IElementEventHandler eventHandler)
        {
            if (TryGetComponent(out UIElement element))
                element.SetEventHandler(eventHandler);
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
        private void Reset() => OnReset();
        private void OnDestroy() => Dispose();
        protected abstract void OnHandleShow(Action showAction);
        protected abstract void OnHandleHide(Action hideAction);
        protected virtual void OnReset() { }
        protected virtual void Dispose() { }
        protected virtual void OnAwake() { }
    }
}