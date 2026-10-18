using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

//this allows us to ASP.Net's Core MVC features including : controllers, HTTP routes etc.
using POS.Api.Models;
using POS.Api.Repositories;

namespace POS.Api.Controllers
//creating filepath for this file.
{
    [ApiController]
    //this line enables API-Specific behaviours + handling certain validation errors.
    [Route("api/[controller]")]
    //this defines the route pattern of this controller.
    // When asp.net see's [controller]: it removes the Controller suffix and gives the route.
    //For example : ProductsController would be Products.
    //So, route is api/products

    //we will use Dapper: Lightweight ORM.
    public class ProductsController: ControllerBase
    {
        private readonly ProductRepository _productRepository;
        public ProductsController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
    

    [HttpGet]
    public async Task<IActionResult> GetAll()
        {
            //so _productRepository is the referenced one we were talking before that could be used by the controller
            //it is being used below.
            
            //await means : the task is being performed asynchronously.
            var products = await _productRepository.GetAllAsync();
            return Ok(products);
        }

    }
}
