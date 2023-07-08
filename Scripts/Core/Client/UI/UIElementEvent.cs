namespace Core.Client.UI
{
    public class UIElementEvent : Event
    {
        public enum ActionType
        {
            Shown,
            Hided
        }

        public readonly ActionType Type;
        public readonly UIElement Element;

        public UIElementEvent(ActionType actionType, UIElement element)
        {
            Type = actionType;
            Element = element;
        }
    }
}