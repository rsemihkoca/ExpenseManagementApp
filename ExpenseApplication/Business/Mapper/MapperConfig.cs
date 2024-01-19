using AutoMapper;
using Business.Entities;
using Business.Enums;

using Infrastructure.Dtos;

namespace Application.Mapper;

public class MapperConfig : Profile
{
    public MapperConfig()
    {

        CreateMap<InsertExpenseRequest, Expense>()
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => ExpenseRequestStatus.Pending))
            .ForMember(dest => dest.CreationDate,
                opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.LastUpdateTime,
                opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.CreatedBy,
                opt => opt.MapFrom(src => src.UserId));
        
        

        CreateMap<Expense, ExpenseResponse>()
            .ForMember(dest => dest.PersonnelName,
                opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.ExpenseCategory.CategoryName))
            .ForMember(dest => dest.ExpenseStatus,
                opt => opt.MapFrom(src => EnumUtils.GetDescription(src.Status)))
            .ForMember(src => src.CreationDate,
                opt => opt.MapFrom(src => src.CreationDate.ToString("dd/MM/yyyy HH:mm:ss")))
            .ForMember(src => src.LastUpdateTime,
                opt => opt.MapFrom(src => src.LastUpdateTime.ToString("dd/MM/yyyy HH:mm:ss")));
        
    }
}