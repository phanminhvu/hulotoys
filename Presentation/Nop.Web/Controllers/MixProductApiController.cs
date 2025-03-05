using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Data;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Components;
using Nop.Web.Framework.Controllers;
using Nop.Web.Models.Catalog;
using System.Text.Json;


namespace Nop.Web.Controllers
{
    [Route("api/mixproduct")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class MixProductApiController : BaseController
    {
        private readonly IRepository<MixProduct> _mixProductRepository;
        private readonly IShoppingCartService _shoppingCartService;
        protected readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        private readonly ILogger _logger;
        protected readonly ILocalizationService _localizationService;
        protected readonly ShoppingCartSettings _shoppingCartSettings;
        private readonly INopDataProvider _nopDataProvider;
        public MixProductApiController(IRepository<MixProduct> mixProductRepository,
           IShoppingCartService shoppingCartService,
            IWorkContext workContext,
            IProductService productService,
            ICustomerService customerService,
            IStoreContext storeContext,
            ILocalizationService localizationService, 
            ShoppingCartSettings shoppingCartSettings,
            INopDataProvider nopDataProvider,
            ILogger logger
        )
        {
            _mixProductRepository = mixProductRepository;
            _shoppingCartService = shoppingCartService;
            _workContext = workContext;
            _productService = productService;
            _customerService = customerService;
            _localizationService = localizationService;
            _shoppingCartSettings = shoppingCartSettings;
            _logger = logger;
            _storeContext = storeContext;
            _nopDataProvider = nopDataProvider;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var mixProducts = _mixProductRepository.Table.ToList();
            return Ok(mixProducts);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var mixProduct = _mixProductRepository.GetById(id);
            if (mixProduct == null)
                return NotFound();

            return Ok(mixProduct);
        }

        [HttpPost]
        public async Task<IActionResult> CreateorUpdateAsync([FromBody] ProductsMixRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var customer = await _workContext.GetCurrentCustomerAsync();
            
            if (customer == null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Not found data customer"
                });
            }
            var listProducts = JsonSerializer.Deserialize<List<ProductsMixInfoModel>>(model.ProductIds);
            if (listProducts == null || listProducts.Count == 0)
            {
                return Ok(new
                {
                    success = false,
                    message = "Product list is empty"
                });
            }
            
            try
            {
                var store = await _storeContext.GetCurrentStoreAsync();
                var cart = await _shoppingCartService.GetShoppingCartAsync(customer);

                for (var x = 0; x < listProducts.Count; x++)
                {
                    var item = listProducts[x];
                    var product = await _productService.GetProductByIdAsync(item.ProductId);
                    if (product == null)
                    {
                        return Ok(new
                        {
                            success = false,
                            message = "Not found data product"
                        });
                    }
                    var productInCart = cart.FirstOrDefault(x => x.CustomerId == customer.Id && x.ProductId == item.ProductId);
                    if (productInCart != null)
                    {

                        await _shoppingCartService.UpdateShoppingCartItemAsync(customer, productInCart.Id, productInCart.AttributesXml, productInCart.CustomerEnteredPrice, quantity: productInCart.Quantity + item.Quantity);
                    }
                    else
                    {
                        await _shoppingCartService.AddToCartAsync(customer, product, (ShoppingCartType)item.ProductType, store.Id, quantity: item.Quantity);
                    }

                }

                await _mixProductRepository.InsertAsync(new MixProduct
                {
                    CustomerId = customer.Id,
                    Status = model.Status,
                    ProductIds = model.ProductIds,
                });


                var shoppingCarts = _mixProductRepository.Table.Where(x => x.CustomerId == customer.Id).Count();

                var updateTopCartSectionHtml = string.Format(
                    await _localizationService.GetResourceAsync("ShoppingCart.HeaderQuantity"), shoppingCarts);

                var updateFlyoutCartSectionHtml = _shoppingCartSettings.MiniShoppingCartEnabled
                     ? await RenderViewComponentToStringAsync(typeof(FlyoutShoppingCartViewComponent))
                     : string.Empty;

                return Ok(new
                {
                    success = true,
                    message = string.Format(await _localizationService.GetResourceAsync("Products.ProductHasBeenAddedToTheCart.Link"),
                        Url.RouteUrl("ShoppingCart")),
                    updatetopcartsectionhtml = updateTopCartSectionHtml,
                    updateflyoutcartsectionhtml = updateFlyoutCartSectionHtml
                });
                
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    message = "Add product to cart fail"
                });
            }
            
        }



        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] MixProduct model)
        {
            var mixProduct = _mixProductRepository.GetById(id);
            if (mixProduct == null)
                return NotFound();

            mixProduct.ProductIds = model.ProductIds;

            _mixProductRepository.Update(mixProduct);
            return NoContent();
        }


        [HttpPost("remove/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var mixProduct = _mixProductRepository.GetById(id);
            if (mixProduct == null)
                return BadRequest(new
                {
                    success = false,
                    message = "Not found mix product"
                });

            var customer = await _workContext.GetCurrentCustomerAsync();
            if (customer == null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Not found data customer"
                });
            }
            var listProducts = JsonSerializer.Deserialize<List<ProductsMixInfoModel>>(mixProduct.ProductIds);
            if (listProducts == null || listProducts.Count == 0)
            {
                return Ok(new
                {
                    success = false,
                    message = "Product list is empty"
                });
            }
            try
            {
                var store = await _storeContext.GetCurrentStoreAsync();
                var cart = await _shoppingCartService.GetShoppingCartAsync(customer);

                for (var x = 0; x < listProducts.Count; x++)
                {
                    var item = listProducts[x];
                    var product = await _productService.GetProductByIdAsync(item.ProductId);
                    if (product == null)
                    {
                        return Ok(new
                        {
                            success = false,
                            message = "Not found data product"
                        });
                    }
                    var productInCart = cart.FirstOrDefault(x => x.CustomerId == customer.Id && x.ProductId == item.ProductId);
                    if (productInCart != null)
                    {
                        // Giảm số lượng sản phẩm con theo số lượng sản phẩm mix
                        if (productInCart.Quantity > item.Quantity)
                        {
                            await _shoppingCartService.UpdateShoppingCartItemAsync(customer, productInCart.Id, productInCart.AttributesXml, productInCart.CustomerEnteredPrice, quantity: productInCart.Quantity - item.Quantity);
                        }
                        else
                        {
                            await _shoppingCartService.DeleteShoppingCartItemAsync(productInCart, true, false);
                        }
                    }
                    
                }

                _mixProductRepository.Delete(mixProduct);

                var productinCart = _mixProductRepository.Table.Where( x => x.CustomerId == customer.Id).Count();
                var shoppingCarts = await _shoppingCartService.GetShoppingCartAsync(customer, ShoppingCartType.ShoppingCart, store.Id);

                var updateTopCartSectionHtml = string.Format(
                    await _localizationService.GetResourceAsync("ShoppingCart.HeaderQuantity"), productinCart
                   );

                
                return Ok(new
                {
                    success = true,
                    message = string.Format(await _localizationService.GetResourceAsync("Products.ProductHasBeenAddedToTheCart.Link"),
                        Url.RouteUrl("ShoppingCart")),
                    updatetopcartsectionhtml = updateTopCartSectionHtml,
                });

            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    message = "Add product to cart fail"
                });
            }
            
            return NoContent();
        }
       
    }
}
