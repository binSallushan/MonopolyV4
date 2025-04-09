using Monopoly_V4.Interfaces;

namespace Monopoly_V4
{
    public abstract class Valuable : IValuable
    {
        public Player? Owner { get; protected set; }

        protected virtual void ValidateChangeOwnership() { }

        void IValuable.ChangeOwnership(Player newOwner)
        {
            throw new NotImplementedException();
        }        
    }
}