namespace UXComex_challenge.Domain.Entities
{
    public class OrderNotification
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
