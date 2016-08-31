namespace Rahnemun.EmailContracts
{
    public class EmailModel
    {
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string ReceiverEmail { get; set; }
        public string ReceiverName { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}