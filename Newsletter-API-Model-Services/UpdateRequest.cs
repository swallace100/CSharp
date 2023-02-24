    public class NewsletterUpdateRequest : NewsletterAddRequest, IModelIdentifier
    {
        [Required]
        public int Id { get; set; }
    }