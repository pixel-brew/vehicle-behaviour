using UnityEngine;
using DG.Tweening;
using Lean.Touch;

namespace Game.Client.Location
{
    public class MapTile : MonoBehaviour
    {
        [SerializeField] protected LeanSelectableByFinger _selectable;
        [SerializeField] protected SpriteRenderer _renderer;
        [SerializeField] private TileStateBase _hidenState; 
        
        private Vector3 _initialPosition;
        private bool _isClicked;

        public void SetupTile(Vector3 newPosition)
        {
            _hidenState.Init(this); // send data if the tile is hidden 

            _initialPosition = newPosition;
            transform.position = _initialPosition;
            _isClicked = false;

            _selectable.OnSelectedFinger.AddListener(OnMapTileClicked);
        }

        private void OnMapTileClicked(LeanFinger finger)
        {
            if (_isClicked)
            {
                return;
            }

            PlayAnimation();
            
            if (_hidenState.TryEnable())
            {
                _hidenState.ActivateTile();
                return;
            }
            
            OnClicked();
        }

        protected virtual void OnClicked()
        {
            //TODO: different behavior of child classes
        }

        private void PlayAnimation()
        {
            var duration = 0.2f;
            _isClicked = true;
            var sequence = DOTween.Sequence();
            _renderer.sortingOrder = 0;
            
            sequence.Append(transform.DOMove(transform.position - Vector3.up * duration, duration));
            sequence.Append(transform.DOMove(_initialPosition, duration)).OnComplete(() =>
            {
                _renderer.sortingOrder = 1;
                _isClicked = false;
            });

            sequence.Play();
        }

        private void OnDestroy()
        {
            _selectable.OnSelectedFinger.RemoveListener(OnMapTileClicked);
        }
    }
}