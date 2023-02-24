    public class Newsletter
    {
        public int Id { get; set; }
        public NewsletterTemplate NewsletterTemplate { get; set; }
        public string Name { get; set; }
        public string CoverPhoto { get; set; }
        public DateTime DateToPublish { get; set; }
        public DateTime DateToExpire { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public User User { get; set; }
    }