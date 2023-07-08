using Core;
using UnityEngine;

namespace Game.Client.Battle
{
    public class VehicleDynamics : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        private const float _speedThreshold = 0.05f;
        private const float _accelerationThreshold = 0.1f;
        
        private float _previousForwardSpeed;
        private float _previousSidewaysSpeed;

        public Vector3 InstantGlobalVelocity { get; private set; }
        public Vector3 GlobalVelocity { get; private set; }
        public float ForwardSpeed { get; private set; }
        public float AbsForwardSpeed { get; private set; }
        public float SidewaysSpeed { get; private set; } // right - positive, left - negative value
        public float AbsSidewaysSpeed { get; private set; }
        
        public float AngularSpeed { get; private set; }
        public float AbsAngularSpeed { get; private set; }

        public float ForwardAcceleration { get; private set; }
        public float SidewaysAcceleration { get; private set; }

        private Vector3 _horizontalForwardDirection
        {
            get
            {
                var result = transform.forward;
                result.y = 0f;
                return result;
            }
        }

        private Vector3 _horizontalRightDirection
        {
            get
            {
                var result = transform.right;
                result.y = 0f;
                return result;
            }
        }

        public void Setup(VehicleConfiguration vehicleConfiguration)
        {
            _rigidbody = vehicleConfiguration.Rigidbody;
        }

        public void Simulate(float deltaTime)
        {
            var horizontalForwardDirection = _horizontalForwardDirection;

            InstantGlobalVelocity = _rigidbody.velocity;
            GlobalVelocity = Vector3.Lerp(GlobalVelocity, InstantGlobalVelocity, Time.deltaTime * 25f);

            var forwardVelocity = Vector3.Project(GlobalVelocity, horizontalForwardDirection);
            AbsForwardSpeed = forwardVelocity.magnitude.Threshold(_speedThreshold);
            bool isMovingForward = Vector3.Dot(GlobalVelocity, horizontalForwardDirection) > 0f;
            ForwardSpeed = AbsForwardSpeed * (isMovingForward ? 1f : -1f);

            var sideVelocity = (GlobalVelocity - forwardVelocity);
            sideVelocity.y = 0f;
            AbsSidewaysSpeed = sideVelocity.magnitude.Threshold(_speedThreshold);
            bool isMovingRight = Vector3.Dot(GlobalVelocity, _horizontalRightDirection) > 0f;
            SidewaysSpeed = AbsSidewaysSpeed * (isMovingRight ? 1f : -1f);

            var angularSpeed = _rigidbody.angularVelocity;
            AngularSpeed= angularSpeed.y;
            AbsAngularSpeed = Mathf.Abs(angularSpeed.y);

            var instantForwardAcceleration = (ForwardSpeed - _previousForwardSpeed) / deltaTime;
            var instantSidewaysAcceleration = (SidewaysSpeed - _previousSidewaysSpeed) / deltaTime;
            _previousForwardSpeed = ForwardSpeed;
            _previousSidewaysSpeed = SidewaysSpeed;

            ForwardAcceleration = Mathf.Lerp(ForwardAcceleration, instantForwardAcceleration, deltaTime * 20f).Threshold(_accelerationThreshold);
            SidewaysAcceleration = Mathf.Lerp(SidewaysAcceleration, instantSidewaysAcceleration, deltaTime * 20f).Threshold(_accelerationThreshold);
        }

        public void GetSpeedAtPosition(Vector3 worldPosition, out float forwardSpeed, out float sideSpeed)
        {
            var horizontalForwardDirection = _horizontalForwardDirection;
            var globalVelocity = _rigidbody.GetPointVelocity(worldPosition);
            var forwardVelocity = Vector3.Project(globalVelocity, horizontalForwardDirection);
            bool isMovingForward = Vector3.Dot(forwardVelocity, horizontalForwardDirection) > 0f;
            forwardSpeed = forwardVelocity.magnitude * (isMovingForward ? 1f : -1f);
            var sideVelocity = globalVelocity - forwardVelocity;
            sideVelocity.y = 0;
            sideSpeed = sideVelocity.magnitude;
        }

        public float GetSidewaysSpeedAtPosition(Vector3 worldPosition)
        {
            var horizontalForwardDirection = _horizontalForwardDirection;
            var globalVelocity = _rigidbody.GetPointVelocity(worldPosition);
            var forwardVelocity = Vector3.Project(globalVelocity, horizontalForwardDirection);
            var sideVelocity = globalVelocity - forwardVelocity;
            sideVelocity.y = 0;
            return sideVelocity.magnitude;
        }
    }
}