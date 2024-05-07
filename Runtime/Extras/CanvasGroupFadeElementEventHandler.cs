using System;
using UnityEngine;

namespace UInterface.Extras
{
    public class CanvasGroupFadeElementEventHandler : ElementEventHandler
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _duration = 0.25f;

        private Coroutine m_coroutine;

        protected override void OnHandleShow(Action showAction)
        {
            if (m_coroutine != null) StopCoroutine(m_coroutine);
            m_coroutine = StartCoroutine(Utils.IETween(SetCanvasFadeValue, AnimationDuration(), showAction, false, Evaluate, delay: _showDelay));
        }

        protected override void OnHandleHide(Action hideAction)
        {
            if (m_coroutine != null) StopCoroutine(m_coroutine);
            m_coroutine = StartCoroutine(Utils.IETween(SetCanvasFadeValue, AnimationDuration(), hideAction, true, Evaluate, delay: _hideDelay));
        }

        public override void Dispose()
        {
            StopCoroutine(m_coroutine);
        }

        protected override float HideCost => _duration;
        protected override float ShowCost => _duration;
        protected virtual void SetCanvasFadeValue(float t) => _canvasGroup.alpha = t;
        protected virtual float Evaluate(float t) => t;
        protected virtual float AnimationDuration() => _duration;
        private void Reset() => _canvasGroup = GetComponent<CanvasGroup>();
    }
}