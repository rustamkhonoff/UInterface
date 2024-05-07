using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UInterface.Extras
{
    public class CompositeElementEventHandler : ElementEventHandler
    {
        [SerializeField] private List<ElementEventHandler> _elementAnimations = new();

        protected override void OnHandleShow(Action showAction)
        {
            bool Validation(ElementEventHandler a) => a.EventsType is Events.Show or Events.All;

            if (!_elementAnimations.Any(Validation))
            {
                showAction?.Invoke();
            }
            else
            {
                switch (_elementAnimations.Count)
                {
                    case 0:
                        showAction?.Invoke();
                        break;
                    case 1:
                        _elementAnimations[0].HandleShow(showAction);
                        break;
                    default:
                    {
                        List<ElementEventHandler> ordered = _elementAnimations.OrderByDescending(a => a.RealShowCost).ToList();
                        foreach (ElementEventHandler item in ordered.Skip(1))
                            item.HandleShow(null);
                        ordered[0].HandleShow(showAction);
                        break;
                    }
                }
            }
        }

        protected override void OnHandleHide(Action hideAction)
        {
            bool Validation(ElementEventHandler a) => a.EventsType is Events.Hide or Events.All;
            if (!_elementAnimations.Any(Validation))
            {
                hideAction?.Invoke();
            }
            else
            {
                switch (_elementAnimations.Count)
                {
                    case 0:
                        hideAction?.Invoke();
                        break;
                    case 1:
                        _elementAnimations[0].HandleHide(hideAction);
                        break;
                    default:
                    {
                        List<ElementEventHandler> ordered = _elementAnimations.OrderByDescending(a => a.RealHideCost).ToList();
                        foreach (ElementEventHandler item in ordered.Skip(1))
                            item.HandleHide(null);
                        ordered[0].HandleHide(hideAction);
                        break;
                    }
                }
            }
        }

        public override void HandleNewHandlerAdded() => UpdateDatas();

        private void Reset() => UpdateDatas();

        public override void Dispose() => _elementAnimations.ForEach(a => a.SetAutoRegister(true));

        private void UpdateDatas()
        {
            _elementAnimations.Clear();
            _elementAnimations.AddRange(GetComponentsInChildren<ElementEventHandler>());
            _elementAnimations.RemoveAll(a => a == this);
            _elementAnimations.ForEach(a => a.SetAutoRegister(false));
        }

        private void OnValidate() => UpdateDatas();
    }
}