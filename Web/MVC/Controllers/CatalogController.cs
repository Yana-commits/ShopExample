using MVC.Models.Requests;
using MVC.Services.Interfaces;
using MVC.ViewModels;
using MVC.ViewModels.CatalogViewModels;
using MVC.ViewModels.Pagination;

namespace MVC.Controllers;

public class CatalogController : Controller
{
    private readonly ICatalogService _catalogService;
    private readonly IBasketService _basketService;
    private readonly ILogger<CatalogController> _logger;
    public CatalogController(ICatalogService catalogService, ILogger<CatalogController> logger, IBasketService basketService)
    {
        _catalogService = catalogService;
        _logger = logger;
        _basketService = basketService;
    }

    public async Task<IActionResult> Index(int? brandFilterApplied, int? typesFilterApplied, int? page, int? itemsPage)
    {
        page ??= 0;
        itemsPage ??= 6;

        var catalog = await _catalogService.GetCatalogItems(page.Value, itemsPage.Value, brandFilterApplied, typesFilterApplied);
        if (catalog == null)
        {
            return View("Error");
        }
        var info = new PaginationInfo()
        {
            ActualPage = page.Value,
            ItemsPerPage = catalog.Data.Count,
            TotalItems = catalog.Count,
            TotalPages = (int)Math.Ceiling((decimal)catalog.Count / itemsPage.Value)
        };
        var vm = new IndexViewModel()
        {
            CatalogItems = catalog.Data,
            Types = await _catalogService.GetTypes(),
            PaginationInfo = info
        };

        vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
        vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";

        return View(vm);
    }
    public async Task<IActionResult> Details(int id)
    {
        var item = await _catalogService.GetItemById(id);
        if (item == null)
        {
            return View("Error");
        }

        var detailsVM = new DetailsVM()
        {
            CatalogItem = item,
            ExistsInCart = false
        };

        var existsInCart = await _basketService.IsInBasket(new IsInBasketRequest() { Id = id }) ;
        
        detailsVM.ExistsInCart = existsInCart;

        return View(detailsVM);
    } 
}