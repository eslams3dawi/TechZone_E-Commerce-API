using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.BLL.DTOs.CategoryDTO;
using TechZone.BLL.DTOs.CategoryDTOs;
using TechZone.BLL.DTOs.OrderDTOs;
using TechZone.BLL.DTOs.PaymentDTOs;
using TechZone.BLL.DTOs.ProductDTO;
using TechZone.BLL.DTOs.ShoppingCartDTOs;
using TechZone.BLL.Wrappers;
using TechZone.DAL.Models;

namespace TechZone.BLL.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Product
            CreateMap<Product, ProductAddDTO>().ReverseMap();
            CreateMap<Product, ProductReadDTO>().ReverseMap();
            CreateMap<Product, ProductDetailsDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            //To partial update and prevent swagger or postman null values (string)
            CreateMap<ProductUpdateDTO, Product>()
                .ForAllMembers(opts =>
                opts.Condition((src, dest, srcMember, destMember, context) => ExtensionMethods.IsValidUpdate(srcMember)));

            //Category
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<CategoryAddDTO, Category>()
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore());
                

            //ShoppingCart
            CreateMap<ShoppingCart, ShoppingCartReadDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.ProductImgUrl, opt => opt.MapFrom(src => src.Product.ImgUrl));

            CreateMap<ShoppingCart, ShoppingCartAddDTO>().ReverseMap();
            CreateMap<ShoppingCart, ShoppingCartUpdateDTO>().ReverseMap();

            //Order
            CreateMap<OrderHeader, OrderAddDTO>().ReverseMap();
            CreateMap<OrderHeader, OrderHeaderReadDTO>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailReadDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));

            //Payment
            CreateMap<Payment, PaymentReadDTO>().ReverseMap();
            CreateMap<Payment, PaymentAddDTO>().ReverseMap();
        }
    }
}
