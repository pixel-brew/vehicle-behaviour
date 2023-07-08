using Core;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Game.Client.Battle.UI
{
    public class UIElementAnimation
    {
        private readonly Transform _elementTransform;
        
        public UIElementAnimation(Transform uiElementTransform)
        {
            _elementTransform = uiElementTransform;
        }

        public async UniTask AnimateShow()
        {
            _elementTransform.SetLocalPositionY(1080f);
            await _elementTransform.DOLocalMoveY(0f, 0.4f).SetEase(Ease.OutCubic);
        }
        
        public  async UniTask AnimateHide()
        {
            await _elementTransform.DOLocalMoveY(-1080f, 0.4f).SetEase(Ease.InCubic);
        }
    }
}