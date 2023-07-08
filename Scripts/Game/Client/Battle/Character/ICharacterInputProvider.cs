
namespace Game.Client.Battle
{
    public interface ICharacterInputProvider
    {
        CharacterInput CharacterInput { get; }
        void UpdateInput();
    }
}