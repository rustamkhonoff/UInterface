namespace Types
{
    public abstract class ModelWindow<T> : WindowBase
    {
        public T Model { get; private set; }
        internal void SetModel(T model) => Model = model;
    }
}