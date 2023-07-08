using System.Collections.Generic;
using Core.Client;
using Game.Client.Battle.Aircraft;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Client.Battle.UI
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(CanvasRenderer))]
    public class AircraftControlWidget : MonoBehaviour,  IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        private IAircraftControl _aircraftControl;

        private class PointerData
        {
            public readonly int PointerId;
            public Vector2 Position;

            public PointerData(int pointerId, Vector2 position)
            {
                PointerId = pointerId;
                Position = position;
            }
        }

        private readonly List<PointerData> _activePointers = new List<PointerData>();
        public void Setup(IAircraftControl aircraftControl)
        {
            _aircraftControl = aircraftControl;
        }

        private PointerData GetPointerData(int pointerId)
        {
            return _activePointers.Find(p => p.PointerId == pointerId);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            var pointerId = eventData.pointerId;
            var position = eventData.position;

            var pointerData = new PointerData(pointerId, position);
            bool isContained = _activePointers.Contains(p => p.PointerId == pointerId);
            if (isContained)
            {
                Debug.LogError($"aircraft control widget :: already has pointer with pointerId = {pointerId}");
                pointerData = GetPointerData(pointerId);
                pointerData.Position = position;
            }
            
            _activePointers.Add(pointerData);
            
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            var pointerId = eventData.pointerId;
            bool isDeleted = _activePointers.RemoveFirst(p => p.PointerId == pointerId);
            if (!isDeleted)
            {
                Debug.LogError($"aircraft control widget :: cannot find to delete pointer data with pointerId = {pointerId}");
            }
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            var pointerId = eventData.pointerId;
            var position = eventData.position;
            
            var pointerData = GetPointerData(pointerId);
            if(pointerData == null)
            {
                Debug.LogError($"aircraft control widget :: cannot find pointer data with pointerId = {pointerId}");
            }
            else
            {
                var deltaPosition = position - pointerData.Position;
                pointerData.Position = position;    
            }
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            var image = GetComponent<Image>();
            image.color = new Color(1f, 1f, 1f, 0f);
            image.sprite = null;
            GetComponent<CanvasRenderer>().cullTransparentMesh = true;
        }
        #endif
    }
}