using System;
using System.Collections.Generic;
using System.Linq;
using DependencyInjectionService;
using UnityEngine;

namespace UInterface
{
    public class UIService : IUIService, IBootable
    {
        private readonly IUIFactory m_uiFactory;
        public event Action Initialized;

        private readonly List<WindowBase> m_currentCreatedWindows;

        private readonly Dictionary<Type, WindowBase> m_cachedModelWindowsTypes;
        private readonly Dictionary<Type, WindowBase> m_cachedWindowsTypes;
        private readonly Dictionary<Type, Func<bool>> m_middlewares;
        private readonly Dictionary<Type, Action<UIElement>> m_createActionHandlers, m_closeActionHandlers;

        private readonly UIStaticData m_uiStaticData;

        private UIRoot m_uiRoot;

        public UIService(
            IUIFactory uiFactory,
            UIServiceConfiguration configuration = null)
        {
            if (configuration == null)
                configuration = new UIServiceConfiguration();

            m_uiFactory = uiFactory;
            m_cachedModelWindowsTypes = new Dictionary<Type, WindowBase>();
            m_cachedWindowsTypes = new Dictionary<Type, WindowBase>();
            m_currentCreatedWindows = new List<WindowBase>();
            m_createActionHandlers = new Dictionary<Type, Action<UIElement>>();
            m_closeActionHandlers = new Dictionary<Type, Action<UIElement>>();

            m_middlewares = configuration.MiddlewareMap != null
                ? new Dictionary<Type, Func<bool>>(configuration.MiddlewareMap.Map)
                : new Dictionary<Type, Func<bool>>();

            m_uiStaticData = Resources.Load<UIStaticData>(configuration.StaticDataPath);
            if (m_uiStaticData == null)
                throw new NullReferenceException(nameof(UIStaticData));

            foreach (WindowBase window in m_uiStaticData.Windows)
            {
                Type type = window.GetType();
                if (IsModelWindow(type, out Type modelType))
                {
                    if (!m_cachedModelWindowsTypes.TryAdd(modelType, window))
                        Debug.LogWarning($"There is already window for model {modelType}");
                }
                else
                {
                    if (!m_cachedWindowsTypes.TryAdd(type, window))
                        Debug.LogWarning($"There is already window of type {type}");
                }
            }

            Initialized?.Invoke();
        }

        public void Boot()
        {
            CheckForUIRoot();
        }

        #region Create

        ///Creates new instance of Window
        /// <param name="onlyOneInstance">If true will find and destroy other instances of same window type</param>
        public TWindow CreateWindowOfType<TWindow>(bool onlyOneInstance = true)
            where TWindow : Window
        {
            CheckForUIRoot();

            Window prefab = (Window)m_cachedWindowsTypes[typeof(TWindow)];
            if (prefab is null)
                throw new ArgumentNullException($"There is no Window of type {typeof(TWindow)}");

            if (m_middlewares.TryGetValue(typeof(TWindow), out Func<bool> condition) && !condition())
            {
                Debug.LogWarning($"Can't create window of type {typeof(TWindow)}, blocked by middleware");
                return null;
            }

            TWindow instance =
                m_uiFactory.Create(prefab, m_uiRoot.RootTransform) as TWindow;

            if (instance == null)
                throw new NullReferenceException($"Error while instantiating Window of type {typeof(TWindow)}");

            instance.SetUIService(this);
            instance.Initialize();
            instance.Show();
            instance.Destroying += () => RemoveFromActive(instance);

            if (onlyOneInstance && TryGetActiveWindowsOfType<TWindow>(out IList<WindowBase> foundInstances))
                DestroyActiveWindows(foundInstances);

            m_currentCreatedWindows.Add(instance);

            if (m_createActionHandlers.TryGetValue(prefab.GetType(), out Action<UIElement> elementEvent))
                elementEvent?.Invoke(instance);

            return instance;
        }


        ///Creates new instance of Model Window
        /// <param name="model">Required model type for window</param>
        /// <param name="onlyOneInstance">If true will find and destroy other instances of same window type</param>
        public ModelWindow<TModel> CreateWindowForModel<TModel>(TModel model, bool onlyOneInstance = true)
        {
            CheckForUIRoot();

            ModelWindow<TModel> prefab = (ModelWindow<TModel>)m_cachedModelWindowsTypes[typeof(TModel)];
            if (prefab is null)
                throw new ArgumentNullException($"There is no Window for Model with type {model.GetType()}");

            if (m_middlewares.TryGetValue(prefab.GetType(), out Func<bool> condition) && !condition())
            {
                Debug.LogWarning($"Can't create window for model type {typeof(TModel)}, blocked by middleware");
                return null;
            }

            ModelWindow<TModel> instance =
                m_uiFactory.Create(prefab, m_uiRoot.RootTransform);

            if (instance == null)
                throw new NullReferenceException($"Error while instantiating ModelWindow for type {model.GetType()}");

            instance.SetUIService(this);
            instance.SetModel(model);
            instance.Initialize();
            instance.Show();
            instance.Destroying += () =>
            {
                if (m_closeActionHandlers.TryGetValue(prefab.GetType(), out Action<UIElement> closeElementEvent))
                    closeElementEvent?.Invoke(instance);
            };
            instance.Destroying += () => RemoveFromActive(instance);

            if (onlyOneInstance && TryGetActiveWindowsOfType<ModelWindow<TModel>>(out IList<WindowBase> foundInstances))
                DestroyActiveWindows(foundInstances);

            m_currentCreatedWindows.Add(instance);

            if (m_createActionHandlers.TryGetValue(prefab.GetType(), out Action<UIElement> openElementEvent))
                openElementEvent?.Invoke(instance);

            return instance;
        }

        ///Creates new instance of Model Window
        /// <param name="model">Required model type for window</param>
        /// <param name="onlyOneInstance">If true will find and destroy other instances of same window type</param>
        public TWindow CreateWindowOfTypeForModel<TWindow, TModel>(TModel model, bool onlyOneInstance = true)
            where TWindow : ModelWindow<TModel>
        {
            return (TWindow)CreateWindowForModel(model, onlyOneInstance);
        }

        #endregion

        #region Middleware

        public void AddMiddleware<TElement>(Func<bool> condition) where TElement : UIElement
        {
            m_middlewares.Add(typeof(TElement), condition);
        }

        public void RemoveMiddleware<TElement>() where TElement : UIElement
        {
            m_middlewares.Remove(typeof(TElement));
        }

        #endregion

        #region Actions

        public void AddCreateAction<TElement>(Action<UIElement> action) where TElement : UIElement
        {
            if (m_createActionHandlers.TryGetValue(typeof(TElement), out Action<UIElement> cachedElementEvent))
            {
                m_createActionHandlers[typeof(TElement)] = cachedElementEvent + action;
            }
            else
            {
                m_createActionHandlers.Add(typeof(TElement), action);
            }
        }

        public void RemoveCreateAction<TElement>(Action<UIElement> action) where TElement : UIElement
        {
            if (m_createActionHandlers.TryGetValue(typeof(TElement), out Action<UIElement> cachedElementEvent))
            {
                m_createActionHandlers[typeof(TElement)] = cachedElementEvent - action;
            }
        }

        public void AddCloseAction<TElement>(Action<UIElement> action) where TElement : UIElement
        {
            if (m_closeActionHandlers.TryGetValue(typeof(TElement), out Action<UIElement> cachedElementEvent))
            {
                m_closeActionHandlers[typeof(TElement)] = cachedElementEvent + action;
            }
            else
            {
                m_closeActionHandlers.Add(typeof(TElement), action);
            }
        }

        public void RemoveCloseAction<TElement>(Action<UIElement> action) where TElement : UIElement
        {
            if (m_closeActionHandlers.TryGetValue(typeof(TElement), out Action<UIElement> cachedElementEvent))
            {
                m_closeActionHandlers[typeof(TElement)] = cachedElementEvent - action;
            }
        }

        #endregion

        #region Active Windows Logic

        public IList<WindowBase> GetActiveWindowsOfType<TWindow>() where TWindow : WindowBase
        {
            return m_currentCreatedWindows.Where(a => a is TWindow).ToList();
        }

        public IList<WindowBase> GetActiveWindowsForModel<TModel>()
        {
            return m_currentCreatedWindows.Where(a => a is ModelWindow<TModel>).ToList();
        }


        public void CloseAllWindows()
        {
            m_currentCreatedWindows.ForEach(a => a.Hide());
        }

        public bool IsWindowActive<TWindow>() where TWindow : WindowBase
            => m_currentCreatedWindows.Exists(a => a.GetType() == typeof(TWindow));

        public bool IsWindowForModelActive<TModel>() =>
            m_currentCreatedWindows.Exists(a => a.GetType() == typeof(ModelWindow<TModel>));

        public bool TryGetActiveWindowsOfType<TWindow>(out IList<WindowBase> instances) where TWindow : WindowBase
        {
            instances = m_currentCreatedWindows.Where(a => a is TWindow).ToList();
            return true;
        }

        #endregion

        #region Destroy

        public void CloseWindowsOfModelType<TModel>()
        {
            TryGetActiveWindowsOfType<ModelWindow<TModel>>(out IList<WindowBase> foundInstances);
            DestroyActiveWindows(foundInstances);
        }

        public void CloseWindowsOfType<TWindow>() where TWindow : WindowBase
        {
            TryGetActiveWindowsOfType<TWindow>(out IList<WindowBase> foundInstances);
            DestroyActiveWindows(foundInstances);
        }

        private void DestroyActiveWindows(IList<WindowBase> windows)
        {
            for (int index = windows.Count - 1; index >= 0; index--)
            {
                WindowBase window = windows[index];
                window.Hide();
                m_currentCreatedWindows.Remove(window);
            }
        }

        public void DestroyAll()
        {
            for (int index = m_currentCreatedWindows.Count - 1; index >= 0; index--)
            {
                WindowBase window = m_currentCreatedWindows[index];
                window.Hide();
            }

            m_currentCreatedWindows.Clear();
        }

        #endregion

        #region Private

        private void RemoveFromActive(WindowBase windowBase)
        {
            if (windowBase == null)
                return;

            m_currentCreatedWindows.Remove(windowBase);
            m_currentCreatedWindows.RemoveAll(a => ReferenceEquals(a, null));
        }

        private void CheckForUIRoot()
        {
            if (!m_uiRoot)
                m_uiRoot = m_uiFactory.Create(m_uiStaticData.RootPrefab);
        }

        private bool IsModelWindow(Type t, out Type modelType)
        {
            while (t != null)
            {
                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ModelWindow<>))
                {
                    modelType = t.GetGenericArguments()[0];
                    return true;
                }

                t = t.BaseType;
            }

            modelType = null;
            return false;
        }

        #endregion
    }
}