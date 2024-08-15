namespace UInterface.Extras
{
    public class StaticUIElement : UIElement
    {
        private void OnEnable() => Show();
        private void Start() => Initialize();
    }
}