using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("UI.Zenject")]

namespace UInterface
{
    [CreateAssetMenu(menuName = "Project/UI/Create UIStaticData", fileName = "UIStaticData", order = 0)]
    internal class UIStaticData : ScriptableObject
    {
        [SerializeField] private UIRoot _uiRootPrefab;
        [SerializeField] private List<WindowBase> _windows;

        public UIRoot RootPrefab => _uiRootPrefab;
        public IEnumerable<WindowBase> Windows => _windows;
    }
}