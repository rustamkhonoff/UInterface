namespace UInterface
{
    public class UIServiceConfiguration
    {
        public IMiddlewareMap MiddlewareMap { get; } = new MiddlewareMap();
        public string StaticDataPath { get; set; } = "StaticData/UI/UIStaticData";
    }
}