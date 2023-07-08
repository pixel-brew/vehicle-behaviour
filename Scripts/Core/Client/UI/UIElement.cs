using Core.Client.Context;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Client.UI
{
    public abstract class UIElement : MonoBehaviour
    {   
        public interface IArgs
        {
        
        }
        
        private enum State
        {
            Showing,
            Shown,
            Hiding,
            Hided,
        }

        public enum WindowType
        {
            Window,
            Popup
        }
        
        private IClientContext _context;
        private State _currentState = State.Hided;
        
        [SerializeField] private int _showPriority;

        public abstract WindowType UIElementType { get; }
        public abstract bool IsPermanent { get; }
        public IArgs Arguments { get; private set; }
        public bool IsReturnedFromStack { get; private set; }
        public int ShowPriority => _showPriority;
        public IClientContext Context => _context;
        public UIModule UIModule => _context.GetModule<UIModule>();
        
        public void Setup(IClientContext context, IArgs args = null)
        {
            _context = context;
            Arguments = args;
            OnSetup();
        }

        protected virtual void OnSetup()
        {
            
        }

        private void Validate()
        {
            bool isSetupCompleted = (_context != null);
            if (!isSetupCompleted)
            {
                Debug.LogError("ui element :: call setup before use");
            }
        }
        
        public async void Show(bool isFromStack = false)
        {
            Validate();
            if (_currentState == State.Shown || _currentState == State.Showing)
            {
                return;
            }
            if (_currentState == State.Hiding)
            {
                StopHiding();
            }
            IsReturnedFromStack = isFromStack;
            gameObject.SetActive(true);
            _currentState = State.Showing;
            await AnimateShow();
            _currentState = State.Shown;
            OnShow();
            new UIElementEvent(UIElementEvent.ActionType.Shown, this).Launch();
        }

        public async void Hide(bool instantly = false)
        {
            Validate();
            if (_currentState == State.Hided || _currentState == State.Hiding)
            {
                return;
            }
            if (_currentState == State.Showing)
            {
                StopShowing();
            }
            _currentState = State.Hiding;
            if (!instantly)
            {
                await AnimateHide();    
            }
            _currentState = State.Hided;
            OnHide();
            new UIElementEvent(UIElementEvent.ActionType.Hided, this).Launch();
            gameObject.SetActive(false);
        }

        public void HideInstantly()
        {
            Hide(instantly:true);
        }

        private void StopShowing()
        {
            
        }

        private void StopHiding()
        {
            
        }
        
        protected virtual void OnShow()
        {   
        }

        protected virtual void OnHide()
        {
        }

        protected virtual void OnDestroy()
        {
            if (IsOpened())
            {
                HideInstantly();
            }
        }

        protected virtual async UniTask AnimateShow()
        {
            await UniTask.Yield();
        }

        protected virtual async UniTask AnimateHide()
        {
            await UniTask.Yield();
        }
        
        public bool IsOpened()
        {
            return gameObject.activeInHierarchy && _currentState == State.Showing || _currentState == State.Shown;
        }

    }
}