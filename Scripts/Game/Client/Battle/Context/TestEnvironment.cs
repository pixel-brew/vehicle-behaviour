using Core.Client.Context;
using Core.Client.UI;
using Game.Client.Battle.UI;
using UnityEngine;

namespace Game.Client.Battle
{
    public class TestEnvironment : MonoBehaviour
    {
        private BattleContext _battleContext;

        private async void Awake()
        {
            _battleContext = new BattleContext();
            IClientContext battleContext = _battleContext;
            await battleContext.Load();
            var uiModule = battleContext.GetModule<UIModule>();
            uiModule.Show<AircraftTargetsScreenWindow>();
            // uiModule.Show<AircraftScreenWindow>();
            
            // uiModule.Show<NotificationPopup>();
            // await UniTask.Delay(TimeSpan.FromSeconds(3.5f), ignoreTimeScale: true);
            // uiModule.Hide();
            // await UniTask.Delay(TimeSpan.FromSeconds(1.5f), ignoreTimeScale: true);
            // uiModule.Show<NotificationPopup>();
            // await UniTask.Delay(TimeSpan.FromSeconds(3.5f), ignoreTimeScale: true);
            // uiModule.Hide();
            // await UniTask.Delay(TimeSpan.FromSeconds(1.5f), ignoreTimeScale: true);
            // uiModule.Show<NotificationPopup>();
            
        }
    }
}