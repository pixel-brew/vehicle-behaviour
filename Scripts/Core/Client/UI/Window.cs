using UnityEngine;

namespace Core.Client.UI
{
    public class Window : UIElement
    {
        [SerializeField] private bool _isPermanent;
        public override WindowType UIElementType => WindowType.Window;
        public override bool IsPermanent => _isPermanent;
    }
}