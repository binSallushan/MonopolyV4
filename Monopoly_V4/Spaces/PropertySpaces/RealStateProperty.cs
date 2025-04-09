using Monopoly_V4.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4.Spaces.PropertySpaces
{
    /// <summary>
    /// Abstract class for base functionality regarding Properties such as Trading and Player Landing as these are common within all Properties.
    /// Trading, Player Landing, Mortgage.
    /// </summary>
    public abstract class RealStateProperty<T> : Valuable, IRealStateProperty where T : RealStateProperty<T>
    {
        public string Name { get; }
        public int Price { get; }
        public bool IsMortgaged { get; protected set;  }
        public IRealStatePropertyGroup<T> PropertyGroup { get; protected set; }

        protected Bank bank;
        protected int turnNumberAtTradeWhileMortgaged;
        protected bool changedOwnerWhenMortgaged;        
        protected RealStateProperty(string name, int price, Bank bank, Player? owner, IRealStatePropertyGroup<T> group)
        {            
            ArgumentOutOfRangeException.ThrowIfNegative(price, nameof(price));            
            this.Owner = owner;
            this.Name = name;
            this.Price = price;
            this.bank = bank ?? throw new ArgumentNullException(nameof(bank));
            this.PropertyGroup = PropertyGroup ?? throw new ArgumentNullException(nameof(group));
            group.AddProperty(this);
        }

        public abstract int CalculateRent();

        public int CalculateMortgageAmount()
        {
            return Price / 2;
        }        
        public int CalculateUnMortgageAmount()
        {
            var mortgagedPrice = CalculateMortgageAmount();
            int unMortgagedPrice = Convert.ToInt32(mortgagedPrice + mortgagedPrice * 0.1);
            if (Owner is Player player)
            {
                if (changedOwnerWhenMortgaged && player.TurnNumber != turnNumberAtTradeWhileMortgaged)
                    unMortgagedPrice += Convert.ToInt32(mortgagedPrice * 0.1);
            }

            return unMortgagedPrice;
        }        

        protected virtual void ValidateMortgageProperty()
        {
            if (Owner is null) throw new InvalidOperationException("Property does not have an owner.");
            if (IsMortgaged) throw new InvalidOperationException("Property is already mortgaged.");                        
        }        
        public void MortgageProperty()
        {
            ValidateMortgageProperty();
            IsMortgaged = true;            
        }                
        public void UnMortgageProperty()
        {
            if (!IsMortgaged) throw new InvalidOperationException("Property is not mortgaged.");
            IsMortgaged = false;
            changedOwnerWhenMortgaged = false;
        }

        public virtual void PlayerLanded(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
