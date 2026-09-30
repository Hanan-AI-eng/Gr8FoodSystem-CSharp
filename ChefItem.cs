using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gr8FoodSystem
{
    public class ChefItem
    {
        public string Name { get; set; }

        public int ID { get; set; }

        public ChefItem(string name, int id)
        {
            Name = name;
            ID = id;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
