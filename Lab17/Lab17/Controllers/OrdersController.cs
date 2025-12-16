using AutoMapper;
using Lab17.Models;
using Lab17.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Lab17.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMapper _mapper;

        public OrdersController(IMapper mapper)
        {
            _mapper = mapper;
        }

        // Рівень 4: Get order -> OrderDto (UserName, TotalItems, ProductNames)
        [HttpGet("{id}")]
        public IActionResult GetOrder(int id)
        {
            var order = new Order
            {
                OrderId = id,
                User = new User { Id = 10, Name = "Petro", Email = "petro@example.com" },
                Items = new List<OrderItem>
                {
                    new OrderItem { ProductName = "Apple", Quantity = 2 },
                    new OrderItem { ProductName = "Banana", Quantity = 3 },
                    new OrderItem { ProductName = "Orange", Quantity = 1 }
                }
            };

            var dto = _mapper.Map<OrderDto>(order);
            return Ok(dto);
        }
    }
}
