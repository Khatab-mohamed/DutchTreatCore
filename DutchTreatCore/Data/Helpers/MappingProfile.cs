using AutoMapper;
using DutchTreatCore.Data.Entities;
using DutchTreatCore.ViewModels;

namespace DutchTreatCore.Data.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrderViewModel>().ForMember(o=>o.OrderId,or=>or.MapFrom(o=>o.Id))
                .ReverseMap();
        }
    }
}
