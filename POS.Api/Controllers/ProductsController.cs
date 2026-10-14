using Microsoft.AspNetCore.Mvc;
//this allows us to ASP.Net's Core MVC features including : controllers, HTTP routes etc.
using POS.Api.Models;

namespace POS.Api.Controllers
//creating filepath for this file.
{
    [ApiController]
    //this line enables API-Specific behaviours + handling certain validation errors.
    [Route("api/[controller]")]
    //this defines the route pattern of this controller.
    // When asp.net see's [controller]: it removes the Controller suffix and gives the route.
    //For example : ProductsController would be Products --> Product.
    //So, route is api/products

    //we will use Dapper: Lightweight ORM.
    public class ProductsController: ControllerBase
    {
        
    }

  

}