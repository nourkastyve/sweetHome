namespace SweetHome.Models;
public class CakeOrder
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string CustomerName { get; set; } = "";
    public string Phone { get; set; } = "";
    public string CakeName { get; set; } = "";
    public string Size { get; set; } = "Moyen";
    public string MessageOnCake { get; set; } = "";
    public DateTime DeliveryDate { get; set; } = DateTime.Today.AddDays(2);
    public decimal TotalPrice { get; set; }
    public decimal Deposit { get; set; }
    public string Status { get; set; } = "Nouvelle commande";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public decimal Remaining => Math.Max(0, TotalPrice - Deposit);
}
