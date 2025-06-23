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
        public bool IsCompleted { get; set; }
        public virtual ICollection<Part> Parts { get; set; } = new List<Part>();
    }
}