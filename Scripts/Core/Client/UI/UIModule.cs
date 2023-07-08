using System;
using System.Collections.Generic;
using Core.Client.Context;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Core.Client.UI
{
    public class UIModule : IClientContextModule
    {
        private int _counterShow;
        private Transform _canvasTransform;
        private IClientContext _clientContext;
        private readonly UIElementStack _elementsStack = new UIElementStack();
        private readonly List<UIElement> _createdElements = new List<UIElement>();
        private UIElement _activeElement;

        void IClientContextModule.Initialize(IClientContext clientContext)
        {
            _clientContext = clientContext;
        }

        async UniTask IClientContextModule.Load()
        {
            CreateUiEventSystem();
            await UniTask.Yield();
            var objectCanvasDynamic = CreateCanvas(UISettings.Instance.CanvasName, 3);
            objectCanvasDynamic.AddComponent<GraphicRaycaster>();
            _canvasTransform = objectCanvasDynamic.transform;
        }

        async UniTask IClientContextModule.Unload()
        {
            await UniTask.Yield();
        }

        private void CreateUiEventSystem()
        {
            var objectEvent = new GameObject(UISettings.Instance.EventSystemName);
            objectEvent.AddComponent<EventSystem>();
            objectEvent.AddComponent<StandaloneInputModule>();
        }

        private GameObject CreateCanvas(string canvasName, int sortingOrder)
        {
            var objectCanvas = new GameObject(canvasName);
            var canvas = objectCanvas.AddComponent<Canvas>();
            var scaler = objectCanvas.AddComponent<CanvasScaler>();

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = sortingOrder;

            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

            scaler.matchWidthOrHeight = UISettings.Instance.MatchWidthOrHeightFactor;
            scaler.referenceResolution = UISettings.Instance.ReferenceResolution;

            return objectCanvas;
        }

        public bool IsOpened<T>() where T : UIElement
        {
            var element = Get<T>();
            return element != null && element.IsOpened();
        }

        public bool IsExisted<T>() where T : UIElement
        {
            return Get<T>() != null;
        }

        public T Get<T>() where T : UIElement
        {   
            var targetType = typeof(T);
            foreach (var element in _createdElements)
            {
                if (element.GetType() == targetType)
                {
                    return (T)element;
                }
            }
            return null;
        }

        public void Preload<T>(UIElement.IArgs args = null) where T : UIElement
        {
            if (!IsExisted<T>())
            {
                var window = Cache<T>(_canvasTransform);
                window?.Setup(_clientContext, args);
            }
        }

        public void Up<T>(UIElement.IArgs args = null, Transform customCanvasTransform = null) where T : UIElement
        {
            var canvasTransform = (customCanvasTransform != null) ? customCanvasTransform : _canvasTransform;
            var itemToShow = Cache<T>(canvasTransform);

            if (itemToShow == null)
            {
                Debug.LogError($"ui module :: prefab for ui with type {typeof(T).Name} not found");
                return;
            }

            ++_counterShow;
            itemToShow.gameObject.transform.SetSiblingIndex(_counterShow);
        }

        public void Show<T>(UIElement.IArgs args = null, Transform customCanvasTransform = null) where T : UIElement
        {
            var canvasTransform = _canvasTransform;
            if (customCanvasTransform != null)
            {
                canvasTransform = customCanvasTransform;
            }

            var elementToShow = Cache<T>(canvasTransform);

            if (elementToShow == null)
            {
                Debug.LogError($"ui module :: prefab for ui with type {typeof(T).Name} not found");
                return;
            }

            ++_counterShow;
            elementToShow.gameObject.transform.SetSiblingIndex(_counterShow);

            switch (elementToShow.UIElementType)
            {
                case UIElement.WindowType.Window:
                    ShowWindow(elementToShow, args);
                    break;
                case UIElement.WindowType.Popup:
                    ShowPopup(elementToShow, args);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ShowWindow(UIElement element, UIElement.IArgs args)
        {
            var (prevElement, _) = _elementsStack.Peek();

            if (_elementsStack.Push(element, args))
            {
                _activeElement?.Hide();
                prevElement?.Hide();
                _activeElement = element;
                _activeElement.Setup(_clientContext, args);
                _activeElement.Show();
            }
            else
            {
                element.HideInstantly();
            }
        }

        private void ShowPopup(UIElement popup, UIElement.IArgs args = null)
        {
            var (prevElement, _) = _elementsStack.Peek();

            if (_elementsStack.Push(popup, args))
            {
                if (prevElement != null && prevElement.UIElementType == UIElement.WindowType.Popup && prevElement.ShowPriority > popup.ShowPriority)
                {
                    prevElement.Hide();
                }

                popup.gameObject.transform.SetSiblingIndex(short.MaxValue);
                popup.Setup(_clientContext, args);
                popup.Show();
            }
            else
            {
                popup.HideInstantly();
            }
        }

        public void Hide()
        {
            if (_elementsStack.IsEmpty)
            {
                return;
            }

            var elementToClose = _elementsStack.Pop();
            elementToClose.Hide();

            if (_activeElement == elementToClose)
            {
                _activeElement = null;
            }

            Debug.Log($"ui module :: hide {elementToClose.GetType().Name}");

            if (!_elementsStack.IsEmpty)
            {
                var (windowToOpen, argsToOpen) = _elementsStack.Peek();

                windowToOpen.Setup(_clientContext, argsToOpen);
                windowToOpen.Show(true);

                if (windowToOpen.UIElementType != UIElement.WindowType.Popup)
                {
                    _activeElement = windowToOpen;
                }
                else if (_activeElement == null)
                {
                    var lastWindowInStack = _elementsStack.GetLastWindowOfType(UIElement.WindowType.Window);

                    if (lastWindowInStack != null)
                    {
                        _activeElement = lastWindowInStack;
                        _activeElement.Show(true);
                    }
                }
            }
        }

        public void HideAll()
        {
            foreach (var element in _createdElements)
            {
                if (element.IsOpened() && !element.IsPermanent)
                {
                    element.Hide();
                }
            }
            _elementsStack.Clear();
            _activeElement = null;
        }

        public void Destroy(Predicate<UIElement> condition)
        {
            for (int i = _createdElements.Count - 1; i >= 0; )
            {
                var element = _createdElements[i];
                if (condition(element))
                {
                    Object.Destroy(element.gameObject);
                    _createdElements.RemoveAt(i);
                }
                else
                {
                    --i;
                }
            }
        }

        public void DirectlyHide<T>() where T : UIElement
        {
            var element = Get<T>();
            if (element != null)
            {
                _elementsStack.Remove(element);
                element.Hide();
            }
        }

        private T Create<T>(UIElement uiElement, Transform canvasTransform) where T : UIElement
        {
            return Object.Instantiate(uiElement, canvasTransform).GetComponent<T>();
        }

        private T Cache<T>(Transform canvasTransform) where T : UIElement
        {
            T result = Get<T>();

            if (result == null)
            {   
                var element = UIElementsRegistry.Instance.Get<T>();
                if (element != null)
                {
                    result = Create<T>(element, canvasTransform);
                    _createdElements.Add(result);
                }
            }

            return result;
        }
    }
}