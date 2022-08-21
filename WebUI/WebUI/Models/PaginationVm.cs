
namespace WebUI.Models
{
    public class PaginationVm
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public PaginationVm(int TotalElements, int currentPage, int pageSize)
        {
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(TotalElements / (double)pageSize);
        }
        public bool HasPreviousPase
        {
            get
            {
                return (CurrentPage > 1);
            }
        }
        public bool HasNextPage
        {
            get
            {
                return (CurrentPage < TotalPages);
            }
        }
    }
}
