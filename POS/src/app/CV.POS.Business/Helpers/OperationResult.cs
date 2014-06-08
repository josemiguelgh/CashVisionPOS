using CV.POS.Entities;

namespace CV.POS.Business.Helpers
{
    public class OperationResult
    {
        public bool Succeed { get; set; }
        public string Message { get; set; }
        public User User { get; set; }
    }
}
