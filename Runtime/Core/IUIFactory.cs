using UnityEngine;

namespace UInterface
{
    public interface IUIFactory
    {
        public T Create<T>(T prefab, Transform parent) where T : UIElement;
        public T Create<T>(T prefab) where T : UIElement;
    }
}