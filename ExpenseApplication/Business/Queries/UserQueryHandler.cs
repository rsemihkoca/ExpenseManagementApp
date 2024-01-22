using Business.Cqrs;
using AutoMapper;
using Business.Validators;
using Infrastructure.DbContext;
using Infrastructure.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Schemes.Dtos;
using Schemes.Exceptions;

namespace Business.Queries;
public class UserQueryHandler :
    IRequestHandler<GetAllUserQuery,List<UserResponse>>,
    IRequestHandler<GetUserByIdQuery,UserResponse>
{

    private readonly ExpenseDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IHandlerValidator validate;

    public UserQueryHandler(ExpenseDbContext dbContext, IMapper mapper, IHandlerValidator validate)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.validate = validate;
    }
    public async Task<List<UserResponse>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        var list = await dbContext.Set<User>().ToListAsync(cancellationToken);
        
        if (list.Count == 0)
        {
            throw new HttpException(Constants.ErrorMessages.NoRecordFound, 404);
        }
        
        var mappedList = mapper.Map<List<User>, List<UserResponse>>(list);
        return mappedList;
    }

    public async Task<UserResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        await validate.IdGreaterThanZeroAsync(request.UserId, cancellationToken);
        var entity = await dbContext.Set<User>().FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);
        
        if (entity == null)
        {
            throw new HttpException($"Record {request.UserId} not found", 404);
        }
        
        var mapped = mapper.Map<User, UserResponse>(entity);
        return mapped;
    }
}