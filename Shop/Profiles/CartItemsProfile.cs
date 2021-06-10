using AutoMapper;
using Shop.Dtos;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Profiles
{
    public class CartItemsProfile : Profile
    {
        public CartItemsProfile()
        {
            CreateMap<CartItem, CartItemReadDto>();
            CreateMap<CartItemCreateDto, CartItem>();

        }
    }
}
