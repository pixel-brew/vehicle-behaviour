using UnityEngine;

namespace Game.Client.Location
{
    public abstract class TileStateBase : MonoBehaviour
    {
        // давай создавать функции по мере надобности
        // те которые тут пустую давай удалим
        // как понадобятся - допишем
        public abstract void Init(MapTile tile);
        public abstract void ActivateTile();
        public abstract bool TryEnable();
    }
}