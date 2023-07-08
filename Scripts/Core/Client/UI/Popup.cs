namespace Core.Client.UI
{
    public class Popup : UIElement
    {
        public override WindowType UIElementType => WindowType.Popup;
        public override bool IsPermanent => false;
    }
}