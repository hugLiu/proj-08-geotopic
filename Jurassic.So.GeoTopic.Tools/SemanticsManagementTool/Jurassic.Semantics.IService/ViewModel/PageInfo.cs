namespace Jurassic.Semantics.IService.ViewModel
{
    public class PagerInfo
    {
        public PagerInfo()
        {
            this.Page = 1;
            this.PageSize = 10000;
            this.Total = -1;
        }
        public PagerInfo(int page, int pageSize)
        {
            this.Page = page;
            this.PageSize = pageSize;
            this.Total = -1;
        }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public long Total { get; set; }
    }
}
