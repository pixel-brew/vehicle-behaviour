namespace Game.Client.Location
{
    public class TileStateHidden : TileStateBase
    {
        public override void Init(MapTile tile)
        {
            
        }

        public override void ActivateTile()
        {
            //TODO: feedback that the tile is hidden
        }

        public override bool TryEnable()
        {
            return false;
        }
    }
}