namespace ProjektZaliczeniowyNET.DTOs.Part
{
    public class PartListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int QuantityInStock { get; set; }
    }
}