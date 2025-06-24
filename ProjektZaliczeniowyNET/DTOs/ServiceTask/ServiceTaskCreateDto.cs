using System.ComponentModel.DataAnnotations;
using ProjektZaliczeniowyNET.DTOs.Part;

namespace ProjektZaliczeniowyNET.DTOs.ServiceTask
{
    public class ServiceTaskCreateDto
    {
        public List<int> PartIds { get; set; } = new List<int>();
        public List<PartDto> Parts { get; set; } = new List<PartDto>();
        
        public string Description { get; set; }

        public decimal LaborCost { get; set; }
        
        public bool IsCompleted { get; set; } =  false;
    }
}