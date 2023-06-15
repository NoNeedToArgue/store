﻿using Microsoft.AspNetCore.Mvc;
using Store.Web.Models;

namespace Store.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IRepository repository;
        private readonly IOrderRepository orderRepository;

        public CartController(IRepository repository, IOrderRepository orderRepository)
        {
            this.repository = repository;
            this.orderRepository = orderRepository;
        }

        public IActionResult Add(int id)
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
    }
}