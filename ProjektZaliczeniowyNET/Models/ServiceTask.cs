using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektZaliczeniowyNET.Models
{
    public class ServiceTask
    {
        public int Id { get; set; }
        public int ServiceOrderId { get; set; }
        public string Description { get; set; }
        public decimal LaborCost { get; set; }
        public List<Part> UsedParts { get; set; } = new();
        public bool IsCompleted { get; set; }
    }
}