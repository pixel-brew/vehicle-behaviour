using Core.Client;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace Game.Client.Battle.UI
{
    [AssetSettings("settings_aircraft_screen", "Resources/Settings")]
    public class AircraftScreenSettings : AssetSettings<AircraftScreenSettings>
    {   
        [SerializeField] private SerializableDictionaryBase<AircraftTargetType, AircraftTargetWidget> _aircraftTargetWidgets;
#if UNITY_EDITOR
        [UnityEditor.MenuItem("Settings/Aircraft Screen Settings")]
        public static void Edit()
        {
            Instance = null;
            UnityEditor.Selection.activeObject = Instance;
            DirtyEditor();
        }
#endif

        public AircraftTargetWidget GetAircraftTargetWidget(AircraftTargetType aircraftTargetType)
        {
            if (_aircraftTargetWidgets.ContainsKey(aircraftTargetType))
            {
                return _aircraftTargetWidgets[aircraftTargetType];
            }
            Debug.LogError($"aircraft settings :: no aircraft target widget for type = {aircraftTargetType}");
            return null;
        }
    }
}