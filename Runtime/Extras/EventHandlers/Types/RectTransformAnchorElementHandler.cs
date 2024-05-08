using System;
using Extras.EventHandlers.Base;
using UnityEngine;

namespace Extras.EventHandlers.Types
{
    public abstract class RectTransformAnchorElementHandler : ElementEventHandler
    {
        [SerializeField] private RectTransform _rectTransform;

        private Coroutine m_coroutine;
        public abstract AnchorData FromData { get; }
        public abstract AnchorData DefaultData { get; }
        public abstract AnchorData ToData { get; }

        protected override void OnHandleShow(Action showAction)
        {
            if (m_coroutine != null) StopCoroutine(m_coroutine);
            m_coroutine = StartCoroutine(Utils.IETween(AnimateAnchor, FromData, DefaultData, ShowData.Duration, showAction, delay: ShowData.Delay));
        }

        protected override void OnHandleHide(Action hideAction)
        {
            if (m_coroutine != null) StopCoroutine(m_coroutine);
            m_coroutine = StartCoroutine(Utils.IETween(AnimateAnchor, GetCurrentData, ToData, HideData.Duration, hideAction,
                delay: HideData.Delay));
        }

        private void AnimateAnchor(float t, AnchorData from, AnchorData to)
        {
            _rectTransform.anchorMin = Vector2.Lerp(from.MinAnchor, to.MinAnchor, t);
            _rectTransform.anchorMax = Vector2.Lerp(from.MaxAnchor, to.MaxAnchor, t);
        }

        private AnchorData GetCurrentData => new(_rectTransform);

        protected override void OnReset() => _rectTransform = GetComponent<RectTransform>();
    }
}