namespace ProjektZaliczeniowyNET.DTOs.ServiceTask
{
    public class ServiceTaskListDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public decimal LaborHours { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal TotalTaskCost { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int ServiceOrderId { get; set; }
        public string ServiceOrderNumber { get; set; } = null!;
    }
}