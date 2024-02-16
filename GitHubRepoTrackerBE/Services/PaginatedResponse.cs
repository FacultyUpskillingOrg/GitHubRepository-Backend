namespace GitRepositoryTracker.Services
{
    public class PaginatedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PaginatedResponse(PagedList<T> pagedList)
        {
            Data = pagedList;
            CurrentPage = pagedList.CurrentPage;
            TotalPages = pagedList.TotalPages;
            PageSize = pagedList.PageSize;
            TotalCount = pagedList.TotalCount;
        }
    }

}
