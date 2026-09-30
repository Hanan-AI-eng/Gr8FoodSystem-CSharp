using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gr8FoodSystem
{
    public class Order
    {

        public int OrderID { get; set; }

        public int CustomerID { get; set; }

        public decimal TotalCost { get; set; }

        public string Status { get; set; }

        public DateTime OrderDate { get; set; }

        
        public Order()
        {

        }

        
        public Order(int orderID, int customerID, decimal totalCost,
                     string status, DateTime orderDate)
        {
            OrderID = orderID;
            CustomerID = customerID;
            TotalCost = totalCost;
            Status = status;
            OrderDate = orderDate;
        }

        public void UpdateStatus(string newStatus)
        {
            Status = newStatus;
        }

        public decimal CalculateTotal(decimal price, int quantity)
        {
            return price * quantity;
        }

        public override string ToString()
        {
            return $"Order #{OrderID} - {Status}";
        }
    }


}
