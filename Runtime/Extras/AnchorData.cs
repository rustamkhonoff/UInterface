using System;
using UnityEngine;

namespace UInterface.Extras
{
    [Serializable]
    public class AnchorData
    {
        [SerializeField] private Vector2 _minAnchor;
        [SerializeField] private Vector2 _maxAnchor;

        public AnchorData()
        {
            _minAnchor = Vector2.zero;
            _maxAnchor = Vector2.one;
        }
        public AnchorData(RectTransform rectTransform)
        {
            _minAnchor = rectTransform.anchorMin;
            _maxAnchor = rectTransform.anchorMax;
        }
        public AnchorData(Vector2 max, Vector2 min)
        {
            _maxAnchor = max;
            _minAnchor = min;
        }

        public Vector2 MinAnchor => _minAnchor;
        public Vector2 MaxAnchor => _maxAnchor;

        public override string ToString() => JsonUtility.ToJson(this, true);
    }
}