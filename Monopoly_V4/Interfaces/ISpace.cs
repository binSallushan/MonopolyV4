namespace Monopoly_V4.Interfaces
{
    public interface ISpace
    {
        string Name { get; }
        void PlayerLanded(Player player);
    }
}

