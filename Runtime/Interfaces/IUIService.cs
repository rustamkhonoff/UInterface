using System;
using System.Collections.Generic;
using UInterface.Core;
using UInterface.Window;

namespace UInterface.Interfaces
{
    public interface IUIService
    {
        event Action Initialized;

        void CreateWindowOfType<TWindow>(bool onlyOneInstance = true)
            where TWindow : Window.Window;

        void CreateWindowOfTypeForModel<TWindow, TModel>(TModel model, bool onlyOneInstance = true)
            where TWindow : ModelWindow<TModel>;

        void CreateWindowForModel<TModel>(TModel model, bool onlyOneInstance = true);

        void AddMiddleware<TElement>(Func<bool> condition) where TElement : UIElement;
        void RemoveMiddleware<TElement>() where TElement : UIElement;
        void AddCreateAction<TElement>(Action<UIElement> action) where TElement : UIElement;
        void RemoveCreateAction<TElement>(Action<UIElement> action) where TElement : UIElement;
        IList<WindowBase> GetActiveWindowsOfType<TWindow>() where TWindow : WindowBase;
        IList<WindowBase> GetActiveWindowsForModel<TModel>();
        bool TryGetActiveWindowsOfType<TWindow>(out IList<WindowBase> instances) where TWindow : WindowBase;
        void CloseWindowsOfModelType<TModel>();
        void CloseWindowsOfType<TWindow>() where TWindow : WindowBase;
        void CloseAllWindows();
        bool IsWindowActive<TWindow>() where TWindow : WindowBase;
        bool IsWindowForModelActive<TModel>();
    }
}