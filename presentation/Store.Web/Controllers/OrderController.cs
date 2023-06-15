using Microsoft.AspNetCore.Mvc;
using Store.Web.Models;

namespace Store.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IRepository repository;
        private readonly IOrderRepository orderRepository;

        public OrderController(IRepository repository, IOrderRepository orderRepository)
        {
            this.repository = repository;
            this.orderRepository = orderRepository;
        }

        public ActionResult Index()
        {
            if (HttpContext.Session.TryGetCart(out Cart cart) && cart.TotalCount > 0)
            {
                var order = orderRepository.GetById(cart.OrderId);
                OrderModel model = Map(order);

                return View(model);
            }

            return View("Empty");
        }

        private OrderModel Map(Order order)
        {
            var productIds = order.Items.Select(x => x.ProductId);
            var products = repository.GetAllByIds(productIds);
            var itemModels = from item in order.Items
                             join product in products on item.ProductId equals product.Id
                             select new OrderItemModel
                             {
                                 ProductId = product.Id,
                                 ProductName = product.Name,
                                 Count = item.Count,
                                 Price = item.Price,
                             };

            return new OrderModel
            {
                Id = order.Id,
                Items = itemModels.ToArray(),
                TotalCount = order.TotalCount,
                TotalPrice = order.TotalPrice,
            };
        }

        public IActionResult AddItem(int id)
        {
            Order order;
            Cart cart;

            if (HttpContext.Session.TryGetCart(out cart))
            {
                order = orderRepository.GetById(cart.OrderId);
            }
            else
            {
                order = orderRepository.Create();
                cart = new Cart(order.Id);
            }

            var product = repository.GetById(id);
            order.AddItem(product, 1);
            orderRepository.Update(order);

            cart.TotalCount = order.TotalCount;
            cart.TotalPrice = order.TotalPrice;

            HttpContext.Session.Set(cart);
            
            return RedirectToAction("Index", "Product", new { id });
        }

        public IActionResult RemoveItem(int id)
        {
            Order order;
            Cart cart;

            Console.WriteLine(id);

            if (HttpContext.Session.TryGetCart(out cart))
            {
                order = orderRepository.GetById(cart.OrderId);
            }
            else
            {
                return RedirectToAction("Empty", "Order");
            }

            var product = repository.GetById(id);
            order.RemoveItem(product, 1);
            orderRepository.Update(order);

            cart.TotalCount = order.TotalCount;
            cart.TotalPrice = order.TotalPrice;

            HttpContext.Session.Set(cart);

            return RedirectToAction("Index", "Order");
        }
    }
}
