using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Action = System.Action;

namespace Extras
{
    public enum AnchorTypes
    {
        Full,
        Left,
        Top,
        Right,
        Bottom
    }

    public static class Utils
    {
        public static AnchorData GetAnchorDataFor(this AnchorTypes anchorTypes)
        {
            return anchorTypes switch
            {
                AnchorTypes.Full => new AnchorData(),
                AnchorTypes.Left => new AnchorData(new Vector2(-1, 0), new Vector2(0, 1)),
                AnchorTypes.Right => new AnchorData(new Vector2(1, 0), new Vector2(2, 1)),
                AnchorTypes.Top => new AnchorData(new Vector2(0, 1), new Vector2(1, 2)),
                AnchorTypes.Bottom => new AnchorData(new Vector2(0, -1), new Vector2(1, 0)),
                _ => new AnchorData(),
            };
        }

        public static IEnumerator IETween(Action<float> action, float duration = 0.25f, Action callback = null, bool reverse = false,
            Func<float, float> evaluateFunc = null, float delay = 0f, bool unscaled = false)
        {
            evaluateFunc ??= a => a;
            Action<float> fixedAction = reverse ? a => action?.Invoke(1f - a) : action;

            fixedAction?.Invoke(0f);

            if (unscaled)
                yield return new WaitForSecondsRealtime(delay);
            else
                yield return new WaitForSeconds(delay);


            for (float t = 0; t < 1f; t += (unscaled ? Time.unscaledDeltaTime : Time.deltaTime) / duration)
            {
                fixedAction?.Invoke(evaluateFunc.Invoke(t));
                yield return null;
            }

            fixedAction?.Invoke(1f);
            callback?.Invoke();
        }

        public static IEnumerator IETween<TData>(Action<float, TData, TData> action, TData from, TData to, float duration = 0.25f,
            Action callback = null,
            bool reverse = false,
            Func<float, float> evaluateFunc = null, float delay = 0f, bool unscaled = false)
        {
            evaluateFunc ??= a => a;
            Action<float, TData, TData> fixedAction = reverse ? (a, b, c) => action?.Invoke(1f - a, b, c) : action;

            fixedAction?.Invoke(0f, from, to);

            if (unscaled)
                yield return new WaitForSecondsRealtime(delay);
            else
                yield return new WaitForSeconds(delay);

            for (float t = 0; t < 1f; t += (unscaled ? Time.unscaledDeltaTime : Time.deltaTime) / duration)
            {
                fixedAction?.Invoke(evaluateFunc.Invoke(t), from, to);
                yield return null;
            }

            fixedAction?.Invoke(1f, from, to);
            callback?.Invoke();
        }

        public static void Foreach<T>(this IEnumerable<T> array, Action<T> action)
        {
            foreach (T item in array)
                action?.Invoke(item);
        }

        public static Coroutine InvokeDelayed(this MonoBehaviour monoBehaviour, Action action, float delay, bool unscaled)
        {
            return monoBehaviour.StartCoroutine(IEInvokeDelayed(action, delay, unscaled));
        }

        public static IEnumerator IEInvokeDelayed(Action action, float delay, bool unscaled = false)
        {
            if (unscaled)
                yield return new WaitForSecondsRealtime(delay);
            else
                yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
    }
}