using ExpensiveControlApp.Models.Expensives;

namespace ExpensiveControlApp.DTOs
{
    public class ListExpensiveDTO
    {
        public ListExpensiveDTO()
        {
            StartDate = DateTime.UtcNow.AddDays(-7);
            EndDate = DateTime.UtcNow.AddDays(3);
            Items = new List<Expensive>();
            Count = Items.Count;
            Total = VerifyTotal();
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Expensive> Items { get; set; }
        
        public int Count { get; set; }
        public double Total { get; set; }

        public double VerifyTotal()
        {
            double total = 0;
            foreach(Expensive item in this.Items)
            {
                total += item.Value;
            }
            return total;
        }
    }
}
