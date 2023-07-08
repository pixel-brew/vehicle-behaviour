using UnityEngine;

namespace Game.Client.Battle
{
    [RequireComponent(typeof(VehicleConfiguration))]
    [RequireComponent(typeof(VehicleDynamics))]
    [RequireComponent(typeof(VehicleMovement))]
    [RequireComponent(typeof(VehicleVisual))]
    public class Vehicle : MonoBehaviour
    {
        private IVehicleInputProvider _vehicleInputProvider;
        private VehicleMovement _vehicleMovement;
        private VehicleDynamics _vehicleDynamics;
        private VehicleConfiguration _vehicleConfiguration;
        private VehicleVisual _vehicleVisual;

        public void Awake()
        {
            Setup();
        }

        private void Setup()
        {
            _vehicleConfiguration = GetComponent<VehicleConfiguration>();
            
            _vehicleDynamics = GetComponent<VehicleDynamics>();
            _vehicleDynamics.Setup(_vehicleConfiguration );

            _vehicleMovement = GetComponent<VehicleMovement>();
            _vehicleMovement.Setup(_vehicleConfiguration, _vehicleDynamics);

            _vehicleVisual = GetComponent<VehicleVisual>();
            _vehicleVisual.Setup(_vehicleConfiguration, _vehicleDynamics, _vehicleMovement);

            _vehicleInputProvider = new VehicleKeyboardInputProvider(_vehicleDynamics);
        }

        public void Update()
        {
            _vehicleInputProvider.UpdateInput();
            var input = _vehicleInputProvider.VehicleInput;
            _vehicleMovement.SetInput(ref input);
            _vehicleVisual.Simulate(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            _vehicleMovement.Simulate(Time.deltaTime);
            _vehicleDynamics.Simulate(Time.deltaTime);
        }
    }
}