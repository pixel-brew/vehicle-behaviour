using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Client.Battle.UI
{
    public class OverheatingIndicator : MonoBehaviour
    {
        [SerializeField] private Image _overheatingImage;
        [SerializeField] private Gradient _gradient;
        private bool _isPressedButton;

        public bool IsOverhetead { get; private set; }
        
        private void Awake()
        {
            _overheatingImage.fillAmount = 0f;
            _overheatingImage.color = Color.white;
        }

        public void SetHeating(float value, float time)
        {
            var fillAmount = _overheatingImage.fillAmount += value;
            _overheatingImage.DOFillAmount(fillAmount, time).SetEase(Ease.OutCirc);
            _overheatingImage.DOColor(_gradient.Evaluate(fillAmount), time).SetEase(Ease.InOutCirc);
            
            SetIsOverhetead(fillAmount >= 1f);
        }

        private void CoolingProcess()
        {
            _overheatingImage.fillAmount -= Time.deltaTime;
            _overheatingImage.color = _gradient.Evaluate(_overheatingImage.fillAmount);
                
            if (_overheatingImage.fillAmount.Equals(0f))
            {
                SetIsOverhetead(false);
            }
        }

        public void SetPressButton(bool isPressed)
        {
            _isPressedButton = isPressed;
        }

        private void SetIsOverhetead(bool isOverheated)
        {
            IsOverhetead = isOverheated;
        }

        private void Update()
        {
            if (IsOverhetead || !_isPressedButton)
            {
                CoolingProcess();
            }
        }
    }
}