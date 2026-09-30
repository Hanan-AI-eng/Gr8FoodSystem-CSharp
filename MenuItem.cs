using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gr8FoodSystem
{
    public class MenuItem
    {
        

        public int ItemID { get; set; }

        public int CategoryID { get; set; }

        public int ChefID { get; set; }

        public string ItemName { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public bool Available { get; set; }

       

        public MenuItem(int categoryID, int chefID, string name, decimal price, string desc, bool available)

        {

            CategoryID = categoryID;

            ChefID = chefID;

            ItemName = name;

            Price = price;

            Description = desc;

            Available = available;

        }

        

        public string GetItemInfo()

        {

            return ItemName + " - RM " + Price;

        }

    }
}

