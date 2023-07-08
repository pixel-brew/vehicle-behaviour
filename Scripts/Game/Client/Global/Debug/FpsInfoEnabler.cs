using UnityEngine;

namespace Game.Client
{
    public class FpsInfoEnabler : MonoBehaviour
    {
        [SerializeField] private GameObject _fpsInfoObject;
        void Awake()
        {
            if (BuildSettings.Instance.ShowFpsInfo)
            {
                _fpsInfoObject.SetActive(true);
            }
            
        }
    }
}
