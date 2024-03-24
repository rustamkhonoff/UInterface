using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("UI.Zenject")]

namespace UI
{
    internal class UIRoot : MonoBehaviour
    {
        [SerializeField] private RectTransform _rootTransform;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Reset()
        {
            _rootTransform = GetComponent<RectTransform>();
        }

        public Transform RootTransform => _rootTransform;
    }
}