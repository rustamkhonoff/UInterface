using UnityEngine;

namespace Extras.EventHandlers.Types
{
    public class RectTransformAnchorElementHandler_Type : RectTransformAnchorElementHandler
    {
        [SerializeField] private AnchorTypes _from, _default, _to;
        
        public override AnchorData FromData => _from.GetAnchorDataFor();

        public override AnchorData DefaultData => _default.GetAnchorDataFor();

        public override AnchorData ToData => _to.GetAnchorDataFor();
    }
}