using UInterface.Core;
using UnityEngine;

namespace UInterface.Extras.EventHandlers.Base
{
    [DefaultExecutionOrder(-10)] [RequireComponent(typeof(UIElement))]
    public class ElementTimeScale : MonoBehaviour
    {
        public enum TimeScaleType
        {
            Scaled = 0,
            UnScaled = 1
        }

        [SerializeField] private TimeScaleType _timeScaleType;

        private void Awake() => GetComponent<UIElement>().SetTimeScale(_timeScaleType);
    }
}