using UnityEngine;

namespace UInterface.Core
{
    public class DefaultUIFactory : IUIFactory
    {
        public T Create<T>(T prefab, Transform parent) where T : UIElement
        {
            return Object.Instantiate(prefab, parent);
        }

        public T Create<T>(T prefab) where T : UIElement
        {
            return Object.Instantiate(prefab);
        }
    }
}