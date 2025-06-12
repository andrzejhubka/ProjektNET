namespace ProjektZaliczeniowyNET.DTOs.ServiceTask
{
    public class ServiceTaskDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string? DetailedDescription { get; set; }
        public decimal LaborHours { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal LaborCost { get; set; }
        public decimal TotalTaskCost { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? Notes { get; set; }
        public int ServiceOrderId { get; set; }
        public string ServiceOrderNumber { get; set; }
    }
}