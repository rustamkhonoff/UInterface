using System;
using Extras.EventHandlers.Base;
using UnityEngine;

namespace Extras.EventHandlers.Types
{
    public class CanvasGroupFadeElementEventHandler : ElementEventHandler
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        private Coroutine m_coroutine;

        protected override void OnHandleShow(Action showAction)
        {
            if (m_coroutine != null) StopCoroutine(m_coroutine);
            m_coroutine = StartCoroutine(Utils.IETween(SetCanvasFadeValue, ShowData.Duration, showAction, false, Evaluate, delay: ShowData.Delay,
                unscaled: IsUnscaled));
        }

        protected override void OnHandleHide(Action hideAction)
        {
            if (m_coroutine != null) StopCoroutine(m_coroutine);
            m_coroutine = StartCoroutine(Utils.IETween(SetCanvasFadeValue, HideData.Duration, hideAction, true, Evaluate, delay: HideData.Delay,
                unscaled: IsUnscaled));
        }

        protected override void Dispose()
        {
            if (m_coroutine != null) StopCoroutine(m_coroutine);
        }

        protected virtual void SetCanvasFadeValue(float t) => _canvasGroup.alpha = t;
        protected virtual float Evaluate(float t) => t;
        protected override void OnReset() => _canvasGroup = GetComponent<CanvasGroup>();
    }
}