using UnityEngine;

namespace UInterface.Extras.EventHandlers
{
    public class RectTransformAnchorElementHandler_Reference : RectTransformAnchorElementHandler
    {
        [SerializeField] private AnchorDataSO _fromReference;
        [SerializeField] private AnchorDataSO _defaultReference;
        [SerializeField] private AnchorDataSO _toReference;
        public override AnchorData FromData => _fromReference.Data;
        public override AnchorData DefaultData => _defaultReference.Data;
        public override AnchorData ToData => _toReference.Data;
    }
}