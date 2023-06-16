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

        private (Order order, Cart cart) GetOrCreateOrderAndCart()
        {
            Order order;

            if (HttpContext.Session.TryGetCart(out Cart cart))
            {
                order = orderRepository.GetById(cart.OrderId);
            }
            else
            {
                order = orderRepository.Create();
                cart = new Cart(order.Id);
            }

            return (order, cart);
        }

        public IActionResult AddItem(int productId, int count = 1)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();

            Product product = repository.GetById(productId);

            order.AddOrUpdateItem(product, count);

            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Product", new { id = productId });
        }

        [HttpPost]
        public IActionResult UpdateItem(int productId, int count)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();

            order.GetItem(productId).Count += count;
            
            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Order");
        }

        public IActionResult RemoveItem(int id)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();

            order.RemoveItem(id);

            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Order");
        }

        private void SaveOrderAndCart(Order order, Cart cart)
        {
            orderRepository.Update(order);

            cart.TotalCount = order.TotalCount;
            cart.TotalPrice = order.TotalPrice;

            HttpContext.Session.Set(cart);
        }
    }
}
