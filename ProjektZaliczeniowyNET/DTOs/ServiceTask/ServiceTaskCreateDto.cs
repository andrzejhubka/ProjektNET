using System.ComponentModel.DataAnnotations;
using ProjektZaliczeniowyNET.DTOs.Part;

namespace ProjektZaliczeniowyNET.DTOs.ServiceTask
{
    public class ServiceTaskCreateDto
    {
        public string Description { get; set; }

        public decimal LaborCost { get; set; }

        public List<PartCreateDto> Parts { get; set; } = new List<PartCreateDto>();
        public bool IsCompleted { get; set; } =  false;
    }
}