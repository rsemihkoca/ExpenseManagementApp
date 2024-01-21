using AutoMapper;
using Business.Entities;
using Business.Enums;
using Infrastructure.Dtos;

namespace Application.Mapper;

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
                opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.CreatedBy,
                opt => opt.MapFrom(src => src.UserId));

        CreateMap<Expense, ExpenseResponse>()
            .ForMember(dest => dest.PersonnelName,
                opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.ExpenseCategory.CategoryName))
            .ForMember(dest => dest.ExpenseStatus,
                opt => opt.MapFrom(src => src.Status))
            .ForMember(src => src.CreationDate,
                opt => opt.MapFrom(src => src.CreationDate.ToString("dd/MM/yyyy HH:mm:ss")))
            .ForMember(src => src.LastUpdateTime,
                opt => opt.MapFrom(src => src.LastUpdateTime.ToString("dd/MM/yyyy HH:mm:ss")));

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
                opt => opt.MapFrom(src => src.IsActive ? "Active" : "Inactive"))
            .ForMember(src => src.LastActivityDateTime,
                opt => opt.MapFrom(src => src.LastActivityDateTime.ToString("dd/MM/yyyy HH:mm:ss")));

        CreateMap<CreatePaymentInstructionRequest, PaymentInstruction>()
            .ForMember(dest => dest.PaymentStatus,
                opt => opt.MapFrom(src => PaymentRequestStatus.Pending));

        CreateMap<PaymentInstruction, PaymentInstructionResponse>()
            .ForMember(dest => dest.Amount,
                opt => opt.MapFrom(src => src.Expense.Amount))
            .ForMember(dest => dest.Description,
                opt => opt.MapFrom(src => src.Expense.Description))
            .ForMember(dest => dest.ExpenseStatus,
                opt => opt.MapFrom(src => src.Expense.Status.ToString()))
            .ForMember(dest => dest.PaymentStatus,
                opt => opt.MapFrom(src => src.PaymentStatus.ToString()));

    }
}