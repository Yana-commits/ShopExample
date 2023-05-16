using MVC.Dtos;
using MVC.Models;
using MVC.Models.Enums;
using MVC.Models.Requests;
using MVC.Services.Interfaces;
using MVC.ViewModels;
using NuGet.Configuration;

namespace MVC.Services;

public class CatalogService : ICatalogService
{
    private readonly IOptions<AppSettings> _settings;
    private readonly IHttpClientService _httpClient;
    private readonly ILogger<CatalogService> _logger;

    public CatalogService(IHttpClientService httpClient, ILogger<CatalogService> logger, IOptions<AppSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings;
        _logger = logger;
    }

    public async Task<Catalog> GetCatalogItems(int page, int take, int? brand, int? type)
    {
        var filters = new Dictionary<CatalogTypeFilter, int>();

        if (brand.HasValue)
        {
            filters.Add(CatalogTypeFilter.Brand, brand.Value);
        }

        if (type.HasValue)
        {
            filters.Add(CatalogTypeFilter.Type, type.Value);
        }

        var result = await _httpClient.SendAsync<Catalog, PaginatedItemsRequest<CatalogTypeFilter>>($"{_settings.Value.FruitCatalogApi}/fruititems",
           HttpMethod.Post,
            new PaginatedItemsRequest<CatalogTypeFilter>()
            {
                PageIndex = page,
                PageSize = take,
                Filters = filters
            });

        return result;
    }

   
    public async Task<IEnumerable<SelectListItem>> GetTypes()
    {
        var result = await _httpClient.SendAsync<TypeResponce<FruitType>, object>($"{_settings.Value.FruitCatalogApi}/getfruittypes",
           HttpMethod.Post,
           new { }
           );

        return result.Data.Select(s => new SelectListItem()
        {
            Text = s.Type,
            Value = s.Id.ToString()
        }).ToList();
    }
    public async Task<CatalogItem> GetItemById(int id)
    {
        var result = await _httpClient.SendAsync<CatalogItem, ItemByIdRequest>($"{_settings.Value.FruitCatalogApi}/getbyid",
              HttpMethod.Post,
               new ItemByIdRequest()
               {
                   FruitId = id
               });

        return result;
    }

    public async Task<CatalogItemsByIds> GetItemsByIds(GetItemsByIdsRequest getItemsByIdsRequest)
    {
        var result = await _httpClient.SendAsync<CatalogItemsByIds, GetItemsByIdsRequest>($"{_settings.Value.FruitCatalogApi}/getitemsbyids",
                  HttpMethod.Post,
                  getItemsByIdsRequest);

        return result;
    }


}
