using UnityEngine;

namespace UInterface.Extras
{
    public class RectTransformAnchorElementHandler_Value : RectTransformAnchorElementHandler
    {
        [SerializeField] private AnchorData _fromData = new();
        [SerializeField] private AnchorData _defaultData = new();
        [SerializeField] private AnchorData _toData = new();

        public override AnchorData FromData => _fromData;
        public override AnchorData DefaultData => _defaultData;
        public override AnchorData ToData => _toData;
    }
}