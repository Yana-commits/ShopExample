using FruitShop.Host.Data;
using FruitShop.Host.Models.Dtos;
using FruitShop.Host.Models.Request;
using FruitShop.Host.Models.Response;
using FruitShop.Host.Services.Interfaces;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FruitShop.Host.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
    [Route(ComponentDefaults.DefaultRoute)]
    public class FruitsBffController : ControllerBase
    {
        private readonly ILogger<FruitsBffController> _logger;
        private readonly IFruitCatalogService _fruitCatalogService;
        private readonly IFruitItemService _fruitItemService;

        public FruitsBffController(
            ILogger<FruitsBffController> logger,
           IFruitCatalogService fruitCatalogService,
           IFruitItemService fruitItemService)
        {
            _logger = logger;
            _fruitCatalogService = fruitCatalogService;
            _fruitItemService = fruitItemService;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PaginatedItemsResponse<FruitItemDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> FruitItems(PaginatedItemRequest request)
        {
            var result = await _fruitCatalogService.GetCatalogItemsAsync(request.PageSize, request.PageIndex);
            return Ok(result);
        }

        [HttpPost]
       
        [ProducesResponseType(typeof(FruitItemsByTypeResponse<FruitItemDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> FruitItemsByType(FruitItemByTypeRequest request)
        {
            var result = await _fruitCatalogService.GetCatalogByTypeAsync(request.Type,request.PageSize, request.PageIndex);
            return Ok(result);
        }

        [HttpPost]
       
        [ProducesResponseType(typeof(FruitItemDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(GetFruitByIdRequest request)
        {
            var result = await _fruitItemService.GetFruitByIdASync(request.FruitId);
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(FruitItemsByType<FruitTypeDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFruitTypes()
        {
            var result = await _fruitCatalogService.GetTypesAsync();
            return Ok(result);
        }
    }
}
