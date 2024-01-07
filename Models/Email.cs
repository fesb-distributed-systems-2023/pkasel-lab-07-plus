namespace pkaselj_lab_07_.Models
{
    public class Email
    {
        public int ID { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public string? Sender { get; set; }
        public IEnumerable<string>? Receivers { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
