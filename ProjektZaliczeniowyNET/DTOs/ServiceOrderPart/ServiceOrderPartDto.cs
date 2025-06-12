namespace ProjektZaliczeniowyNET.DTOs.ServiceOrderPart
{
    public class ServiceOrderPartDto
    {
        public int Id { get; set; }
        public string PartName { get; set; } = null!;
        public decimal Cost { get; set; }
        public int Quantity { get; set; }
        public decimal TotalCost => Cost * Quantity;
        public int ServiceOrderId { get; set; }
    }
}