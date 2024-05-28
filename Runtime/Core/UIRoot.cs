using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("UI.Zenject")]

namespace UInterface
{
    internal class UIRoot : UIElement
    {
        [SerializeField] private bool _dontDestroyOnLoad;
        [SerializeField] private RectTransform _rootTransform;

        private void Awake()
        {
            if (_dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }

        private void Reset()
        {
            _rootTransform = GetComponent<RectTransform>();
        }

        public Transform RootTransform => _rootTransform;
    }
}