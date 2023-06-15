using Microsoft.AspNetCore.Mvc;

namespace Store.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IRepository repository;

        public ProductController(IRepository repository)
        {
            this.repository = repository;
        }

        public IActionResult Index(int id)
        {
            Product product = repository.GetById(id);

            return View(product);
        }
    }
}
