using Microsoft.AspNetCore.Mvc;

namespace Store.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly ProductService productService;

        public SearchController(ProductService productService)
        {
            this.productService = productService;
        }

        public IActionResult Index(string query)
        {
            Product[] products = productService.GetAllByQuery(query);
             
            return View(products);
        }
    }
}
