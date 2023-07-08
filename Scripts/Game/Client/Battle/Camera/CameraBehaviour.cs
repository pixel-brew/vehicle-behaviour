using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Game.Client.Battle
{
    public class CameraBehaviour : MonoBehaviour
    {
        [Range(1, 10)] [SerializeField] private float _followSpeed = 2;
        [Range(1, 10)] [SerializeField] private float _lookSpeed = 5;

        [SerializeField] private Vector3 _cameraOffset;

        public Transform Target;

        void Start()
        {
            // UpdateOffset();
        }

        void FixedUpdate()
        {
            var lookDirection = Target.position - transform.position;
            var toTargetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toTargetRotation, _lookSpeed * Time.deltaTime);

            var targetPosition = _cameraOffset + Target.transform.position;
            transform.position = Vector3.Lerp(transform.position, targetPosition, _followSpeed * Time.deltaTime);
        }

        [Button()]
        public void UpdateOffset()
        {
            _cameraOffset = gameObject.transform.position - Target.position;
        }
    }
}