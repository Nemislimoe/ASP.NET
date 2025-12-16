using AutoMapper;
using Lab17.Models;
using Lab17.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Lab17.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;

        public UsersController(IMapper mapper)
        {
            _mapper = mapper;
        }

        // Рівень 1: Get single user -> UserDto (без Id)
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = new User
            {
                Id = id,
                Name = "Inga",
                Email = "inga@example.com"
            };

            var dto = _mapper.Map<UserDto>(user);
            return Ok(dto);
        }

        // Рівень 2: Get list of users with Address -> List<UserDto>
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = new List<User>
            {
                new User { Id = 1, Name = "Inga", Age = 30, Address = new Address { City = "Kyiv", Street = "Main St" } },
                new User { Id = 2, Name = "Anna", Age = 25, Address = new Address { City = "Lviv", Street = "Freedom Ave" } },
                new User { Id = 3, Name = "Olga", Age = 28, Address = new Address { City = "Odesa", Street = "Sea Rd" } }
            };

            var dtos = _mapper.Map<List<UserDto>>(users);
            return Ok(dtos);
        }

        // Рівень 3: Get user with roles (Role -> string), Guest відкидається
        [HttpGet("{id}/with-roles")]
        public IActionResult GetWithRoles(int id)
        {
            var user = new User
            {
                Id = id,
                Name = "Ivan",
                Roles = new List<Role>
                {
                    new Role { Name = "Admin" },
                    new Role { Name = "User" },
                    new Role { Name = "Guest" } // має бути відкинута
                }
            };

            var dto = _mapper.Map<UserDto>(user);
            return Ok(dto);
        }
    }
}
