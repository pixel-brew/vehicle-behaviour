using System.Collections.Generic;
using System.Linq;

namespace Core.Client.UI
{
    public class UIElementStack
    {
        private class Item
        {
            public readonly UIElement UIElement;
            public readonly UIElement.IArgs Args;

            public Item(UIElement uiElement, UIElement.IArgs args)
            {
                UIElement = uiElement;
                Args = args;
            }
        }

        private readonly List<Item> _items = new List<Item>();

        public bool Push(UIElement uiElement, UIElement.IArgs args)
        {
            var isPushedWindowOpened = true;

            var previousWindow = _items.LastOrDefault();
            var currentWindow = uiElement;

            _items.Add(new Item(uiElement, args));

            if (previousWindow != null)
            {
                isPushedWindowOpened = currentWindow.ShowPriority <= previousWindow.UIElement.ShowPriority;
                _items.Sort((item1, item2) => item2.UIElement.ShowPriority.CompareTo(item1.UIElement.ShowPriority));
            }

            return isPushedWindowOpened;
        }

        public UIElement Pop()
        {
            var lastIndex = _items.Count - 1;
            var elementToPop = _items[lastIndex];
            _items.RemoveAt(lastIndex);

            return elementToPop.UIElement;
        }

        public bool IsEmpty => _items.Count == 0;
        
        public (UIElement,UIElement.IArgs) Peek()
        {
            if (IsEmpty)
            {
                return (null, null);
            }

            var lastElement = _items[_items.Count - 1];
            return (lastElement.UIElement, lastElement.Args);
        }
        
        public void Remove(UIElement uiElement)
        {
            _items.RemoveAll(item => item.UIElement == uiElement);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public UIElement GetLastWindowOfType(UIElement.WindowType type)
        {
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                if (_items[i].UIElement.UIElementType == type)
                {
                    return _items[i].UIElement;
                }
            }

            return null;
        }
    }
}