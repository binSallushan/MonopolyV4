namespace Monopoly_V4.Interfaces
{
    public interface IValuable
    {
        public Player? Owner { get; }        
        public void ChangeOwnership(Player newOwner);        
    }
}