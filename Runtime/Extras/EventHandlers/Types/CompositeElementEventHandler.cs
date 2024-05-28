using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UInterface.Extras.EventHandlers
{
    public class CompositeElementEventHandler : ElementEventHandler, ICompositeElementEventHandler
    {
        [SerializeField] private List<ElementEventHandler> _handlers = new();

        protected override void OnAwake()
        {
            _handlers.ForEach(a => a.ShowData.Delay += _showData.Delay);
            _handlers.ForEach(a => a.HideData.Delay += _hideData.Delay);
            SetEventHandler(this);
        }

        protected override void OnHandleShow(Action showAction)
        {
            bool Validation(ElementEventHandler a) => a.EventsType is Events.Show or Events.All;
            IterateAndInvokeByCost(
                Validation,
                showAction,
                (handler, action) => handler.HandleShow(action),
                handler => handler.TotalShowCost);
        }


        protected override void OnHandleHide(Action hideAction)
        {
            bool Validation(ElementEventHandler a) => a.EventsType is Events.Hide or Events.All;
            IterateAndInvokeByCost(
                Validation,
                hideAction,
                (handler, action) => handler.HandleHide(action),
                handler => handler.TotalHideCost);
        }

        private void IterateAndInvokeByCost(
            Func<ElementEventHandler, bool> validation,
            Action action,
            Action<ElementEventHandler, Action> handlerAction,
            Func<ElementEventHandler, float> costFunc)
        {
            if (!_handlers.Any(validation))
            {
                action?.Invoke();
            }
            else
            {
                switch (_handlers.Count)
                {
                    case 0:
                        action?.Invoke();
                        break;
                    case 1:
                        handlerAction?.Invoke(_handlers[0], action);
                        break;
                    default:
                    {
                        List<ElementEventHandler> ordered = _handlers.OrderByDescending(costFunc).ToList();
                        foreach (ElementEventHandler item in ordered.Skip(1))
                            handlerAction?.Invoke(item, null);
                        handlerAction?.Invoke(ordered[0], action);
                        break;
                    }
                }
            }
        }

        protected override void OnReset()
        {
            _handlers.Clear();
            _handlers.AddRange(GetComponents<ElementEventHandler>());
            _handlers.RemoveAll(a => a == this);
        }
    }
}