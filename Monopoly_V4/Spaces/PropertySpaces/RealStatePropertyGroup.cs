using Monopoly_V4.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4.Spaces.PropertySpaces
{
    public class RealStatePropertyGroup<T> : IRealStatePropertyGroup<T> where T : IRealStateProperty
    {
        public IEnumerable<T> Properties { get; } = [];

        public int GetNumberOfPropertiesOwned(Player player)
        {
            ArgumentNullException.ThrowIfNull(player, nameof(player));
            return Properties.Count(x => x.Owner == player);
        }

        public void AddProperty(IRealStateProperty property)
        {
            ArgumentNullException.ThrowIfNull(property, nameof(property));
            (Properties as List<T>).Add((T)property); // Don't know what I have done.
        }
        

        public bool GroupOwned(Player player)
        {
           ArgumentNullException.ThrowIfNull(player, nameof(player));
            return GetNumberOfPropertiesOwned(player) == Properties.Count(); // Returning if player owns all the properties.
        }
    }
}
