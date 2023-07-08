using Core.Client.UI;
using Cysharp.Threading.Tasks;

namespace Game.Client.Battle.UI
{
    public class NotificationPopup : Popup
    {
        private UIElementAnimation _elementAnimation;

        private void Awake()
        {
            _elementAnimation = new UIElementAnimation(transform);
        }

        protected override UniTask AnimateShow()
        {
            return _elementAnimation.AnimateShow();
        }
        
        protected override UniTask AnimateHide()
        {
            return _elementAnimation.AnimateHide();
        }
    }
}