namespace pkaselj_lab_07_.Configuration
{
    public class ValidationConfiguration
    {
        public int BodyMaxCharacters { get; set; }
        public int SubjectMaxCharacters { get; set; }
        public int EmailMaxCharacters { get; set; }
        public string EmailRegex { get; set; }
    }
}
