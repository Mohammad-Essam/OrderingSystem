﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task.Dto;
using Task.Models;


namespace Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("myorder")]
        public async Task<IActionResult> MyOrders()
        {
            if (HttpContext.Request.Headers.Authorization.Count == 0)
            {
                return BadRequest("you have to be authenticated");
            }

            var user = await _context.Users.Include(u => u.Orders).SingleOrDefaultAsync(u =>
                u.Api_token == HttpContext.Request.Headers.Authorization[0]);

            if (user == null)
                return BadRequest("you have to be authenticated");
            var orders = await _context.Orders
                .Include(o => o.Product)
                .Where(o => o.UserId == user.Id)
                .Select(o => new
                {
                    Id = o.Id,
                    UserId = o.UserId,
                    Quantity = o.Quantity,
                    Product = o.Product,
                    Totalprice = o.Product.Price * o.Quantity,
                })
                .ToListAsync();
            return Ok(orders);
        }



        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(x => x.Product)
                .Join(
                    _context.Users,
                    o => o.UserId,
                    u => u.Id,
                    (o, u) => new
                    {
                        Id = o.Id,
                        UserId = o.UserId,
                        User = u,
                        Quantity = o.Quantity,
                        Product = o.Product,
                        Totalprice = o.Product.Price * o.Quantity,
                    }
                )
                .ToListAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Show(int id)
        {
            var order = await _context.Orders
                .Include(x => x.Product).Where(o => o.Id == id)
                .SingleOrDefaultAsync();
            return Ok(order);
        }



        [HttpPost]
        public async Task<IActionResult> Store([FromBody] OrderDto order)
        {
            if (HttpContext.Request.Headers.Authorization.Count == 0)
                return NotFound("Guest");

            var user = await _context.Users.SingleOrDefaultAsync(u =>
                u.Api_token == HttpContext.Request.Headers.Authorization[0]);
            if (user == null)
                return BadRequest("you have to be authenticated");


            var product = await _context.Products.SingleOrDefaultAsync(p =>
                p.Id == order.ProductId
            );
            if (product == null)
                return NotFound("No Product found with this id");

            Order o = new Order {
                ProductId = order.ProductId,
                UserId = user.Id,
                Quantity = order.Quantity
            };
            await _context.Orders.AddAsync(o);
            _context.SaveChanges();
            return Ok(o);
        }

        [HttpGet("customer/{id}")]
        public async Task<IActionResult> GetOrdersByCustomerId(int id)
        {
            List<Order> orders = await _context.Orders.Include(o => o.Product).Where(o => o.UserId == id).ToListAsync<Order>();
            return Ok(orders);
        }

        //Delete all orders of the user of id {Id}
        [HttpDelete("customer/{id}")]
        public async Task<IActionResult> DeleteAllOrdersByCustomerId(int id)
        {
            var user = await _context.Users.Include(u => u.Orders).SingleOrDefaultAsync(u =>
                u.Id == id);

            if (user == null)
                return BadRequest("you have to be authenticated");
            List<Order> orders = await _context.Orders.Include(o => o.Product).Where(o => o.UserId == user.Id).ToListAsync<Order>();
            foreach (Order o in orders)
            {
                _context.Orders.Remove(o);
            }
            _context.SaveChanges();
            return Ok(orders);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Destroy(int id)
        {
            if (HttpContext.Request.Headers.Authorization.Count == 0)
            {
                return BadRequest("you have to be authenticated");
            }

            var user = await _context.Users.SingleOrDefaultAsync(u =>
                u.Api_token == HttpContext.Request.Headers.Authorization[0]);

            if (user == null)
                return BadRequest("you have to be authenticated");

            var order = await _context.Orders.SingleOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound("invalid id");

            if (order.UserId != user.Id)
            {
                return BadRequest("you can't delete another customer order");
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();
            return Ok(order);
        }

    }
}
