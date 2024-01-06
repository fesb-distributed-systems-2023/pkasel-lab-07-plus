using pkaselj_lab_07_.Models;

namespace pkaselj_lab_07_.Controllers.DTO
{
    public class EmailInfoDTO
    {
        public int ID { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public string? Sender { get; set; }
        public string? Receiver { get; set; }
        public string? Timestamp { get; set; }

        public static EmailInfoDTO FromModel(Email model)
        {
            return new EmailInfoDTO
            {
                ID = model.ID,
                Subject = model.Subject,
                Body = model.Body,
                Sender = model.Sender,
                Receiver = model.Receiver,
                Timestamp = model.Timestamp.ToLongTimeString()
            };
        }
    }
}
