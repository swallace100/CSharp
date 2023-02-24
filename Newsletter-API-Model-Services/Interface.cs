    public interface INewsletterService
    {
        Paged<Newsletter> PagedAll(int pageIndex, int pageSize);
        Paged<Newsletter> QueryPaged(int pageIndex, int pageSize, string query); 
        void Delete(int Id);
        void DeleteComposite(int Id);
        int Insert(NewsletterAddRequest model);
        int InsertComposite(NewsletterAddRequest model);
        void Update(NewsletterUpdateRequest model);
        void UpdateComposite(NewsletterUpdateRequest model);
    }