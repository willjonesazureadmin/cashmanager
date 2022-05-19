
namespace cashmanager.web.spa.Services
{
    public interface IHistoryService
    {

        void AddPageToHistory(string pageName);
            
        public List<PageHistory> BreadCrumb();

        public void Back(PageHistory ph);

        string Latest();

        bool ShowHistory();

        public string Penultimate();

    }
}
       