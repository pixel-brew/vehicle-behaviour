using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Game.Client.Battle
{
    public class VehicleKeyboardInputProvider : IVehicleInputProvider
    {
        private VehicleInput _vehicleInput = new VehicleInput();
        private float SpeedThresholdToSwitchDirection = 5f;
        private bool _isMovingForward = true;
        private readonly VehicleDynamics _vehicleDynamics;

        private const string AccelerationAxis = "acceleration";
        private const string BrakingAxis = "braking";
        private const string SteeringAxis = "steering";
        private const string HandbrakeAxis = "handbrake";

        public VehicleKeyboardInputProvider(VehicleDynamics vehicleDynamics)
        {
            _vehicleDynamics = vehicleDynamics;
        }

        VehicleInput IVehicleInputProvider.VehicleInput => _vehicleInput;

        void IVehicleInputProvider.UpdateInput()
        {
            var accelerationAxis = Input.GetAxis(AccelerationAxis);
            var brakingAxis = Input.GetAxis(BrakingAxis);

            bool isBraking = brakingAxis > float.Epsilon;
            bool isAccelerating = accelerationAxis > float.Epsilon;

            if (_isMovingForward && !isAccelerating && isBraking && _vehicleDynamics.ForwardSpeed < SpeedThresholdToSwitchDirection)
            {
                _isMovingForward = false;
            }

            if (!_isMovingForward && !isBraking && isAccelerating && _vehicleDynamics.ForwardSpeed > -SpeedThresholdToSwitchDirection && accelerationAxis > float.Epsilon)
            {
                _isMovingForward = true;
            }


            if (_isMovingForward)
            {
                _vehicleInput.Acceleration = accelerationAxis;
                _vehicleInput.Braking = brakingAxis;
                if (_vehicleDynamics.ForwardSpeed < 0f && _vehicleInput.Acceleration > 0f)
                {
                    _vehicleInput.Braking = Mathf.Max(_vehicleInput.Acceleration, _vehicleInput.Braking);
                }
            }
            else
            {
                _vehicleInput.Acceleration = brakingAxis;
                _vehicleInput.Braking = accelerationAxis;
                
                if (_vehicleDynamics.ForwardSpeed > 0f && _vehicleInput.Acceleration > 0f)
                {
                    _vehicleInput.Braking = Mathf.Max(_vehicleInput.Acceleration, _vehicleInput.Braking);
                }
            }

            _vehicleInput.Steering = Input.GetAxis(SteeringAxis);
            _vehicleInput.HandBraking = Input.GetAxis(HandbrakeAxis);
            _vehicleInput.IsForwardMoving = _isMovingForward;
        }
    }
}