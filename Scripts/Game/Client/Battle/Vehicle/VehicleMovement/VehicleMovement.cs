using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Game.Client.Battle
{
    public class VehicleMovement : MonoBehaviour
    {
        private class WheelMovement
        {
            public VehicleConfiguration.WheelLocation WheelLocation;
            public WheelCollider WheelCollider;
            public VehicleMovementConfig.WheelMovementConfig WheelMovementConfig;
        }

        private readonly List<WheelMovement> _wheels = new List<WheelMovement>();
        private VehicleMovementConfig _config;
        private Rigidbody _rigidbody;

        private VehicleDynamics _vehicleDynamics;
        private float _slipFactor;
        private float _stiffnessFactor;

        public bool IsForwardMoving { get; private set; }
        public float Acceleration { get; private set; }
        public float Braking { get; private set; }
        public float HandBraking { get; private set; }
        public float Steering { get; private set; }
        public bool IsAccelerating => Acceleration > float.Epsilon;
        public bool IsBraking => Braking > float.Epsilon;
        public bool IsHandBraking => HandBraking > float.Epsilon;
        public float MaxSpeed => IsForwardMoving ? _config.MaxSpeedForward : _config.MaxSpeedBackward;

        public void Setup(VehicleConfiguration configuration, VehicleDynamics vehicleDynamics)
        {
            _vehicleDynamics = vehicleDynamics;

            _config = configuration.MovementConfig;

            _rigidbody = configuration.Rigidbody;
            _rigidbody.mass = _config.BodyMass;
            _rigidbody.centerOfMass = new Vector3(0f, 0f, _config.BodyMassOffset);

            foreach (var wheel in configuration.Wheels)
            {
                _wheels.Add(new WheelMovement()
                {
                    WheelLocation = wheel.WheelLocation,
                    WheelCollider = wheel.WheelCollider,
                    WheelMovementConfig = configuration.GetWheelMovementConfig(wheel.WheelLocation)
                });
            }

            // Debug.Assert(_);

            foreach (var w in _wheels)
            {
                w.WheelCollider.forwardFriction = w.WheelMovementConfig.ForwardFriction.ToWheelFrictionCurve();
                w.WheelCollider.sidewaysFriction = w.WheelMovementConfig.SidewaysFriction.ToWheelFrictionCurve();
                // w.ConfigureVehicleSubsteps(5, 12, 15);
            }
        }

        public void SetInput(ref VehicleInput input)
        {
            IsForwardMoving = input.IsForwardMoving;
            bool isMaxSpeedReached = _vehicleDynamics.AbsForwardSpeed > MaxSpeed;
            Acceleration = isMaxSpeedReached ? 0f : input.Acceleration;
            Braking = input.Braking;
            HandBraking = input.HandBraking;
            Steering = input.Steering;

            if (_config.TrackedSteering.IsEnabled)
            {
                Acceleration *= 1f - _config.TrackedSteering.DecelerationRatio * Steering;
            }
        }

        public void Simulate(float deltaTime)
        {
            foreach (var wheel in _wheels)
            {
                SimulateWheel(wheel, deltaTime);
            }
        }

        private void SimulateWheel(WheelMovement wheelMovement, float deltaTime)
        {
            var wheelMovementConfig = wheelMovement.WheelMovementConfig;
            var wheelCollider = wheelMovement.WheelCollider;

            float directionSign = IsForwardMoving ? 1f : -1f;

            SimulateSteeringOnTracks(directionSign);
            SimulateSteeringOnWheels(wheelCollider, wheelMovementConfig.SteeringFactor);

            wheelCollider.motorTorque = wheelMovementConfig.MotorFactor * directionSign * Acceleration * _config.AccelerationForce;

            var handbrakeForce = wheelMovementConfig.HandBrakeFactor * HandBraking * _config.HandBrakeForce;
            var brakeForce = wheelMovementConfig.BrakeFactor * Braking * _config.BrakeForce;
            wheelCollider.brakeTorque = handbrakeForce + brakeForce;

            if (IsHandBraking)
            {
                _slipFactor = (_slipFactor + deltaTime * _config.HandbrakeUsage.SlipFactorIncrease).Clamp01();
                _stiffnessFactor = Mathf.Lerp(_stiffnessFactor, 1f, deltaTime * _config.HandbrakeUsage.StiffnessFactorIncrease);
            }
            else
            {
                _slipFactor = (_slipFactor - deltaTime * _config.HandbrakeUsage.SlipFactorDecrease).Clamp01();
                _stiffnessFactor = (_stiffnessFactor - deltaTime * _config.HandbrakeUsage.StiffnessFactorDecrease).Clamp01();
            }

            wheelCollider.SetSidewaysExtremumSlip(Mathf.Lerp(wheelMovementConfig.SidewaysFriction.ExtremumSlip, _config.HandbrakeUsage.ExtremumSlipDuringBraking, _slipFactor));
            wheelCollider.SetSidewaysStiffness(Mathf.Lerp(wheelMovementConfig.SidewaysFriction.Stiffness, _config.HandbrakeUsage.StiffnessDuringBraking, _stiffnessFactor * wheelMovementConfig.HandBrakeFactor));
        }

        private void SimulateSteeringOnWheels(WheelCollider wheel, float steeringFactor)
        {
            if (_config.WheeledSteering.IsEnabled)
            {
                var steeringAngle = Steering * _config.WheeledSteering.MaxSteeringAngle * steeringFactor;
                wheel.steerAngle = Mathf.Lerp(wheel.steerAngle, steeringAngle, _config.WheeledSteering.SteeringSpeed);
            }
        }

        private void SimulateSteeringOnTracks(float dir)
        {
            if (_config.TrackedSteering.IsEnabled)
            {
                bool tooFastSteering = _vehicleDynamics.AbsAngularSpeed > _config.TrackedSteering.MaxAngularSpeed;
                if (!tooFastSteering)
                {
                    _rigidbody.AddTorque(new Vector3(0f, _config.TrackedSteering.TrackedSteeringForce * Steering * dir, 0f), ForceMode.Force);
                }
            }
        }
    }
}