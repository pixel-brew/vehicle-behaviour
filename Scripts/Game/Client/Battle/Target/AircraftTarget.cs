using System.Collections.Generic;
using UnityEngine;

namespace Game.Client.Battle
{
    public class AircraftTarget : MonoBehaviour, IAircraftTarget
    {
        [SerializeField] private AircraftTargetType _aircraftTargetType = AircraftTargetType.Small;
        private Bounds _bounds;
        private Vector3 _position;
        private Vector2 _displayOffset;

        private Dictionary<AircraftTargetType, Vector2> _offsets; //for test

        Bounds IAircraftTarget.Bounds => _bounds;
        Vector3 IAircraftTarget.Position => _position;
        Vector2 IAircraftTarget.DisplayOffset => _displayOffset;
        AircraftTargetType IAircraftTarget.AircraftTargetType => _aircraftTargetType;

        private void Start()
        {
            _offsets = new Dictionary<AircraftTargetType, Vector2>
            {
                {AircraftTargetType.Small, new Vector2(0f, 60f)},
                {AircraftTargetType.Medium, new Vector2(0f, 60f)}, 
                {AircraftTargetType.Large, new Vector2(0f, 0f)} 
            };
            
            _displayOffset = _offsets[_aircraftTargetType];
            _bounds = GetComponentInChildren<SkinnedMeshRenderer>().bounds;
        }

        private void Update()
        {
            _position = transform.position;
        }
    }
}