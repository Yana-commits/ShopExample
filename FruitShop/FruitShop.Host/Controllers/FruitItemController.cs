using FruitShop.Host.Models.Request;
using FruitShop.Host.Models.Response;
using FruitShop.Host.Services.Interfaces;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Xml.Linq;

namespace FruitShop.Host.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowClientPolicy)]
    [Scope("fruitcatalog.fruitcatalogitem")]
    [Route(ComponentDefaults.DefaultRoute)]
    public class FruitItemController : ControllerBase
    {
        private readonly ILogger<FruitItemController> _logger;
        private readonly IFruitItemService _fruitItemService;

        public FruitItemController(
            ILogger<FruitItemController> logger,
            IFruitItemService fruitItemService)
        {
            _logger = logger;
            _fruitItemService = fruitItemService;
        }


        [HttpPost]
        [ProducesResponseType(typeof(AddFruitItemResponse< int?>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Add(CreateFruitItemRequest request)
        {
            var result = await _fruitItemService.AddFruitItemAsync(request.Name, request.FruitTypeId, request.FruitSortId, request.Description,
            request.Price, request.ProviderId, request.PictureUrl);

            return Ok(new AddFruitItemResponse<int?>() { Id = result });
        }

        [HttpPost]
        [ProducesResponseType(typeof(UpdateItemResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update(UpdateFruitItemRequest request)
        {
            var result = await _fruitItemService.UpdateDescriptionAsync(request.Id, request.Description);
            return Ok(new UpdateItemResponse() { IsUpdated = result });
        }

        [HttpPost]
        [ProducesResponseType(typeof(DeliteFruitItemResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteItem(DeliteFruitItemRequest request)
        {
            var result = await _fruitItemService.DeleteFruitItemAsync(request.Id);
            return Ok(new DeliteFruitItemResponse() { IsDeleted = result });
        }
    }
}
