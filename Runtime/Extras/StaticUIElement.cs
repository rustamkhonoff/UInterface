using System;

namespace UInterface.Extras
{
    public sealed class StaticUIElement : UIElement
    {
        private void OnEnable() => Show();
        private void Start() => Initialize();
    }
}