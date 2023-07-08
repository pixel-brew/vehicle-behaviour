using UnityEngine;
using DG.Tweening;

namespace Game.Client.Battle.UI
{
    // rename spread indicator
    public class AimBehaviour : MonoBehaviour
    {
        [SerializeField] private RectTransform _aimTargets;
        [SerializeField] private float _minSpread;
        [SerializeField] private float _maxSpread;
        
        private float _minScale = 1;
        private float _maxScale = 2;

        //temporarily disabled the method and serialized the fields
        public void SetupSpread(float minValue, float maxValue)
        {
            _minSpread = minValue;
            _maxSpread = maxValue;
        }
        
        public void SetSpread(float value)
        {
            _aimTargets.sizeDelta += Vector2.one * value;
            _aimTargets.sizeDelta = new Vector2(Mathf.Clamp(_aimTargets.sizeDelta.x, _minSpread, _maxSpread), 
                Mathf.Clamp(_aimTargets.sizeDelta.y, _minSpread, _maxSpread));
        }
        
        public void ResetSpread()
        {
            Tween resetTween = DOVirtual.Float(_aimTargets.sizeDelta.x, _minSpread, 1f, 
                (progressValue) =>
                {
                    _aimTargets.sizeDelta = Vector2.one * progressValue;
                });

            resetTween.SetEase(Ease.OutQuint);
            resetTween.Play();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.E))
            {
                SetScaleAim(1 * Time.deltaTime); //test
            }
            
            if (Input.GetKey(KeyCode.Q))
            {
                SetScaleAim(-1 * Time.deltaTime); //test
            }
        }

        private void SetScaleAim(float scale)
        {
            var newScale= Mathf.Clamp(transform.localScale.x, _minScale, _maxScale);
            transform.localScale = new Vector3(newScale + scale, newScale + scale, transform.localScale.z);
        }

        public void EnableAim()
        {
            gameObject.SetActive(true); //test
        }

        public void DisableAim()
        {
            gameObject.SetActive(false); //test
        }
    }
}