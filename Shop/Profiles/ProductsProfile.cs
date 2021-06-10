using AutoMapper;
using Shop.Dtos;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Profiles
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            CreateMap<Product, ProductReadDto>();
            CreateMap<ProductCreateDto, Product>();
            CreateMap<Product, ProductUpdateDto>();
            CreateMap<ProductUpdateDto, Product>();
        }
    }
}
