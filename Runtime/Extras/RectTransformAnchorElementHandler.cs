using System;
using UnityEngine;

namespace UInterface.Extras
{
    public abstract class RectTransformAnchorElementHandler : ElementEventHandler
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private float _showDuration = 0.25f;
        [SerializeField] private float _hideDuration = 0.25f;

        private Coroutine m_coroutine;
        public abstract AnchorData FromData { get; }
        public abstract AnchorData DefaultData { get; }
        public abstract AnchorData ToData { get; }

        protected override void OnHandleShow(Action showAction)
        {
            if (m_coroutine != null) StopCoroutine(m_coroutine);
            m_coroutine = StartCoroutine(Utils.IETween(AnimateAnchor, FromData, DefaultData, _showDuration, showAction));
        }

        protected override void OnHandleHide(Action hideAction)
        {
            if (m_coroutine != null) StopCoroutine(m_coroutine);
            m_coroutine = StartCoroutine(Utils.IETween(AnimateAnchor, GetCurrentData, ToData, _hideDuration, hideAction));
        }

        private void AnimateAnchor(float t, AnchorData from, AnchorData to)
        {
            _rectTransform.anchorMin = Vector2.Lerp(from.MinAnchor, to.MinAnchor, t);
            _rectTransform.anchorMax = Vector2.Lerp(from.MaxAnchor, to.MaxAnchor, t);
        }

        private AnchorData GetCurrentData => new(_rectTransform);

        protected override float ShowCost => _showDuration;
        protected override float HideCost => _hideDuration;

        private void Reset() => _rectTransform = GetComponent<RectTransform>();
    }
}