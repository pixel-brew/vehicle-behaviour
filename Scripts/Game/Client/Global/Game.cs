using UnityEngine;

namespace Game.Client
{
    public class Game : MonoBehaviour
    {
        void Awake()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }
    }

}
