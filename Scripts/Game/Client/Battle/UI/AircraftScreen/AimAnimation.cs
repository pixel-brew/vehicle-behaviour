using UnityEngine;
using DG.Tweening;

namespace Game.Client.Battle.UI
{
    public class AimAnimation : MonoBehaviour
    {
        [SerializeField] private float _rotateDuration;
        [SerializeField] private Vector3 _rotateAmount = new Vector3(0f, 0f, 360f);
        [SerializeField] private RectTransform _targetRotation;
        
        private void Start()
        {
            _targetRotation.DOLocalRotate(_rotateAmount, _rotateDuration, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1);
        }
    }   
}