using UnityEngine;

namespace UInterface.Extras
{
    [CreateAssetMenu(menuName = "Project/UI/ElementEventHandler/Create AnchorData", fileName = "AnchorData", order = 0)]
    public class AnchorDataSO : ScriptableObject
    {
        [SerializeField] private AnchorData _anchorData = new();

        public AnchorData Data => _anchorData;
    }
}