namespace pkaselj_lab_07_.Controllers.DTOs
{
    public class EmailDto_In
    {
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public string? Sender { get; set; }
        public string? Receiver { get; set; }
    }
    public class EmailDto_Out
    {
        public int ID { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public string? Sender { get; set; }
        public string? Receiver { get; set; }
        public string? Timestamp { get; set; }
    }
}
