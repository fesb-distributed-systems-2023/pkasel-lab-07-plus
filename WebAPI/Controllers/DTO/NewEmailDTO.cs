using EasyMailCoreApplication.Models;

namespace WebAPI.Controllers.DTO
{
    public class NewEmailDTO
    {
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public string? Sender { get; set; }
        public IEnumerable<string>? Receivers { get; set; }

        public Email ToModel()
        {
            return new Email
            {
                ID = -1,
                Body = Body,
                Subject = Subject,
                Sender = Sender,
                Receivers = Receivers,
                Timestamp = DateTime.MinValue
            };
        }
    }
}
