using Application.Cqrs;
using Application.Services;
using Application.Validators;
using AutoMapper;
using Business.Entities;
using Infrastructure.Data.DbContext;
using Infrastructure.Dtos;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Commands;

public class ExpenseCommandHandler :
    IRequestHandler<CreateExpenseCommand, ExpenseResponse>
{
    private readonly ExpenseDbContext dbContext;

    // private readonly JwtConfig jwtConfig;
    private readonly IUserService userService;
    private readonly IMapper mapper;
    private readonly IHandlerValidator validator;

    public ExpenseCommandHandler(ExpenseDbContext dbContext, IMapper mapper, IHandlerValidator validator,
        IUserService userService) // IOptionsMonitor<JwtConfig> jwtConfig,
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.validator = validator;
        this.userService = userService;
        // this.jwtConfig = jwtConfig.CurrentValue;
    }


    public async Task<ExpenseResponse> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateUserAsync(request.Model.UserId, cancellationToken);
        await validator.ValidateCategoryAsync(request.Model.CategoryId, cancellationToken);

        var role = userService.GetUserRole();

        switch (role)
        {
            case "Admin":
                break;
            case "Personnel":
                if (request.Model.UserId != userService.GetUserId())
                {
                    throw new HttpException("You can't create expense for other users", 403);
                }
                break;
            default:
                throw new HttpException("You can't create expense", 403);
        }
        // + // Get role from jwttoken
        // + // check user is admin or not
        // + // check user is exist or not
        // + // check category is exist or not
        // check category id is integer or not
        // check amount is integer or not
        // 
        // personel sadece kendisi için expense oluşturabilir
        // admin herkes için expense oluşturabilir
        //

        var entity = mapper.Map<InsertExpenseRequest, Expense>(request.Model);

        var entityResult = await dbContext.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var mapped = mapper.Map<Expense, ExpenseResponse>(entityResult.Entity);
        return mapped;
    }
}