
namespace Game.Client.Battle
{
    public interface IVehicleInputProvider
    {
        VehicleInput VehicleInput { get; }
        void UpdateInput();
    }
}