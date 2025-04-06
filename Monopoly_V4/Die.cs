using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4
{
    public class Die
    {
        public int? Number { get; set; }
        private int minValue;
        private int maxValue;
        private Random rnd;

        public Die() : this(1, 6)
        {
            Number = null;
        }

        public Die(int minValue, int maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.rnd = new Random();
        }


        public void Roll()
        {
            Number = rnd.Next(minValue, maxValue);
        }


        public void Clear()
        {
            Number = null;
        }
    }
}
