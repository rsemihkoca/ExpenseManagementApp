using AutoMapper;
using Infrastructure.Entities;
using Schemes.Dtos;
using Schemes.Enums;

namespace Business.Mapper;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<CreateExpenseRequest, Expense>()
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => ExpenseRequestStatus.Pending))
            .ForMember(dest => dest.CreationDate,
                opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.LastUpdateTime,
                opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<Expense, ExpenseResponse>()
            .ForMember(dest => dest.ExpenseStatus,
                opt => opt.MapFrom(src => src.Status))
            .ForMember(src => src.CreationDate,
                opt => opt.MapFrom(src => src.CreationDate.ToString(Constants.DateFormats.DateTimeFormat)))
            .ForMember(src => src.LastUpdateTime,
                opt => opt.MapFrom(src => src.LastUpdateTime.ToString(Constants.DateFormats.DateTimeFormat)));

        CreateMap<ExpenseCategory, ExpenseCategoryResponse>();

        CreateMap<CreateUserRequest, User>()
            .ForMember(dest => dest.PasswordRetryCount,
                opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.IsActive,
                opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.LastActivityDateTime,
                opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<UpdateUserRequest, User>()
            .ForMember(dest => dest.IsActive,
                opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.LastActivityDateTime,
                opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<User, UserResponse>()
            .ForMember(dest => dest.IsActive,
                opt => opt.MapFrom(src => src.IsActive ? Constants.UserStatus.Active : Constants.UserStatus.Inactive))
            .ForMember(src => src.LastActivityDateTime,
                opt => opt.MapFrom(src => src.LastActivityDateTime.ToString(Constants.DateFormats.DateTimeFormat)));

    }
}