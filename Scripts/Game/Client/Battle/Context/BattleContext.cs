using Core.Client.Context;
using Core.Client.UI;
using Game.Client.Battle.Aircraft;

namespace Game.Client.Battle
{
    public class BattleContext : ClientContext
    {
        private UIModule _uiModule = new UIModule();
        private AircraftModule _aircraftModule = new AircraftModule();
        private AircraftTargetsModule _aircraftTargetsModule = new AircraftTargetsModule();
    }
}