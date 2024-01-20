using Application.Cqrs;
using AutoMapper;
using Business.Entities;
using Infrastructure.Data.DbContext;
using Infrastructure.Dtos;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries;
public class UserQueryHandler :
    IRequestHandler<GetAllUserQuery,List<UserResponse>>,
    IRequestHandler<GetUserByIdQuery,UserResponse>
{

    private readonly ExpenseDbContext dbContext;
    private readonly IMapper mapper;

    public UserQueryHandler(ExpenseDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }
    public async Task<List<UserResponse>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        var list = await dbContext.Set<User>().ToListAsync(cancellationToken);
        
        if (list.Count == 0)
        {
            throw new HttpException("No record found", 404);
        }
        
        var mappedList = mapper.Map<List<User>, List<UserResponse>>(list);
        return mappedList;
    }

    public async Task<UserResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Set<User>().FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);
        
        if (entity == null)
        {
            throw new HttpException($"Record {request.UserId} not found", 404);
        }
        
        var mapped = mapper.Map<User, UserResponse>(entity);
        return mapped;
    }
}