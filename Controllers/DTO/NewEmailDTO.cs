using pkaselj_lab_07_.Models;

namespace pkaselj_lab_07_.Controllers.DTO
{
    public class NewEmailDTO
    {
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public string? Sender { get; set; }
        public string? Receiver { get; set; }

        public Email ToModel()
        {
            return new Email
            {
                ID = -1,
                Body = Body,
                Subject = Subject,
                Sender = Sender,
                Receiver = Receiver,
                Timestamp = DateTime.MinValue
            };
        }
    }
}
