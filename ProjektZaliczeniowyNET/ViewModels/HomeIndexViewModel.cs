namespace ProjektZaliczeniowyNET.ViewModels
{
    public class HomeIndexViewModel
    {
        public int ActiveOrdersCount { get; set; }
        public int CompletedOrdersCount { get; set; }
        public string WelcomeMessage { get; set; } = string.Empty;
        
        // Dodatkowe właściwości, które mogą być przydatne na stronie głównej
        public int TotalOrdersCount => ActiveOrdersCount + CompletedOrdersCount;
        public decimal CompletionRate => TotalOrdersCount > 0 
            ? Math.Round((decimal)CompletedOrdersCount / TotalOrdersCount * 100, 1) 
            : 0;
    }
}