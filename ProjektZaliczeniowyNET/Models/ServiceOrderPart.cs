using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektZaliczeniowyNET.Models
{
    public class ServiceOrderPart
    {
        public int Id { get; set; }

        public int ServiceOrderId { get; set; }
        public ServiceOrder ServiceOrder { get; set; } = null!;

        public int PartId { get; set; }
        public Part Part { get; set; } = null!;

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Cost { get; set; } // koszt = Part.Price * Quantity
    }
}