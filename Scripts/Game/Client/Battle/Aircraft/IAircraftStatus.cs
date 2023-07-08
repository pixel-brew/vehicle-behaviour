namespace Game.Client.Battle.Aircraft
{
    public interface IAircraftStatus
    {
        int WeaponsCount { get; }
        IAircraftWeaponStatus GetWeaponStatus(int weaponIndex);
    }
}