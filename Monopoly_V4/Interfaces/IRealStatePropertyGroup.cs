using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4.Interfaces
{
    public interface IRealStatePropertyGroup<out T> where T : IRealStateProperty
    {
        public IEnumerable<T> Properties { get; }
        public void AddProperty(IRealStateProperty property);
        public int GetNumberOfPropertiesOwned(Player transactable);
        public bool GroupOwned(Player transactable);
    }
}
