namespace MVC.ViewModels
{
    public class DetailsVM
    {
        public DetailsVM()
        {
            CatalogItem = new CatalogItem();
        }

        public CatalogItem CatalogItem { get; set; }
        public bool ExistsInCart { get; set; }
    }
}
