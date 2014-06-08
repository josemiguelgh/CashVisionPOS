namespace CV.POS.Wpf.ViewModel
{
    public class SimpleSaleLineViewModel
    {
        public short ProductBaseId { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public short Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LinePrice { get; set; }
        
    }
}
