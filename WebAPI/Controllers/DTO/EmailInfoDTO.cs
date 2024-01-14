using EasyMailCoreApplication.Models;

namespace WebAPI.Controllers.DTO
{
    public class EmailInfoDTO
    {
        public int ID { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public string? Sender { get; set; }
        public IEnumerable<string>? Receivers { get; set; }
        public string? Timestamp { get; set; }

        public static EmailInfoDTO FromModel(Email model)
        {
            return new EmailInfoDTO
            {
                ID = model.ID,
                Subject = model.Subject,
                Body = model.Body,
                Sender = model.Sender,
                Receivers = model.Receivers,
                Timestamp = model.Timestamp.ToLongTimeString()
            };
        }
    }
}
