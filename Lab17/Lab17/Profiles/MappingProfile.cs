using AutoMapper;
using Lab17.Models;
using Lab17.DTOs;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lab17.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Рівень 1: User -> UserDto (Name, Email)
            CreateMap<User, UserDto>()
                // Email може бути відсутнім у деяких DTO сценаріях
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            // Рівень 2: Address -> AddressDto та User -> UserDto (вкладені об'єкти)
            CreateMap<Address, AddressDto>();
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));

            // Рівень 3: Role -> string та кастомний мапінг для відкидання "Guest"
            CreateMap<Role, string>().ConvertUsing(r => r.Name);
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Roles,
                    opt => opt.MapFrom(src => src.Roles == null
                        ? new List<string>()
                        : src.Roles
                             .Where(r => !string.Equals(r.Name, "Guest", System.StringComparison.OrdinalIgnoreCase))
                             .Select(r => r.Name)
                             .ToList()));

            // Рівень 4: Order -> OrderDto (складний мапінг)
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.Name : string.Empty))
                .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.Items != null ? src.Items.Sum(i => i.Quantity) : 0))
                .ForMember(dest => dest.ProductNames, opt => opt.MapFrom(src => src.Items != null ? src.Items.Select(i => i.ProductName).ToList() : new List<string>()));
        }
    }
}
