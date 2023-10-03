using AutoMapper;
using eathappy.order.domain.Article;
using eathappy.order.domain.Dtos.Local.Parameter;
using eathappy.order.domain.Dtos.Local.Result;
using eathappy.order.domain.Models;
using eathappy.order.domain.Order;
using eathappy.order.domain.User;
using System;

namespace eathappy.order.business.Mappers
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Order, OrderResultDto>().ReverseMap();

            CreateMap<Article, ArticleDto>().ReverseMap();
            CreateMap<Article, ArticleResultDto>()
                .ForMember(articleResultDto => articleResultDto.ReplacementArticle,
                opt => opt.MapFrom(article => article.ReplacementArticle != Guid.Empty ? article.ReplacementArticle : (Guid?)null))
                .ReverseMap();

            CreateMap<UserRegistrationDto, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));

            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
