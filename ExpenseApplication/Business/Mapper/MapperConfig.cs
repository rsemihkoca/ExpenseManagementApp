using AutoMapper;
using Business.Entities;
using Infrastructure.Dtos;

namespace Application.Mapper;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<Expense, ExpenseResponse>()
            .ForMember(dest => dest.PersonnelName,
                opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.ExpenseCategory.CategoryName))
            .ForMember(dest => dest.ExpenseStatus,
                opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.PaymentStatus,
                opt => opt.MapFrom(src => src.PaymentInstruction.PaymentStatus.ToString()));

    }
}