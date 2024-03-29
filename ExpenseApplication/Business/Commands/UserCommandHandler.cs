using Business.Cqrs;
using Business.Validators;
using AutoMapper;
using Infrastructure.DbContext;
using Infrastructure.Entities;
using MediatR;
using Schemes.Dtos;
using Schemes.Enums;

namespace Business.Commands;

public class UserCommandHandler :
    IRequestHandler<CreateUserCommand, UserResponse>,
    IRequestHandler<UpdateUserCommand, UserResponse>,
    IRequestHandler<DeleteUserCommand, UserResponse>,
    IRequestHandler<ActivateUserCommand, UserResponse>,
    IRequestHandler<DeactivateUserCommand, UserResponse>
{
    private readonly ExpenseDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IHandlerValidator validate;

    public UserCommandHandler(
        ExpenseDbContext dbContext,
        IMapper mapper,
        IHandlerValidator validator)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.validate = validator;
    }

    public async Task<UserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await validate.RecordNotExistAsync<User>(x => x.Username == request.Model.Username, cancellationToken);
        await validate.RecordNotExistAsync<User>(x => x.Email == request.Model.Email, cancellationToken);
        await validate.RecordNotExistAsync<User>(x => x.Iban == request.Model.Iban, cancellationToken);
        
        request.Model.Password = MD5Extensions.ToMD5(request.Model.Password.Trim());
        
        var entity = mapper.Map<CreateUserRequest, User>(request.Model);

        var entityResult = await dbContext.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var mapped = mapper.Map<User, UserResponse>(entityResult.Entity);
        return mapped;
    }

    public async Task<UserResponse> Handle(UpdateUserCommand request,
        CancellationToken cancellationToken)
    {
        await validate.IdGreaterThanZeroAsync(request.UserId, cancellationToken);
        
        var fromdb = await validate.RecordExistAsync<User>(x => x.UserId == request.UserId, cancellationToken);
        
        // Check if username, email and iban is changed if changed check if it is unique
        if (fromdb.Username != request.Model.Username)
        {
            await validate.RecordNotExistAsync<User>(x => x.Username == request.Model.Username, cancellationToken);
        }
        
        if (fromdb.Email != request.Model.Email)
        {
            await validate.RecordNotExistAsync<User>(x => x.Email == request.Model.Email, cancellationToken);
        }
        
        if (fromdb.Iban != request.Model.Iban)
        {
            await validate.RecordNotExistAsync<User>(x => x.Iban == request.Model.Iban, cancellationToken);
        }
        
        fromdb.Username = request.Model.Username;
        fromdb.Password = MD5Extensions.ToMD5(request.Model.Password.Trim());
        fromdb.FirstName = request.Model.FirstName;
        fromdb.LastName = request.Model.LastName;
        fromdb.Email = request.Model.Email;
        fromdb.Iban = request.Model.Iban;
        fromdb.Role = request.Model.Role;
        fromdb.IsActive = request.Model.IsActive;
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        var mapped = mapper.Map<User, UserResponse>(fromdb);
        return mapped;
    }

    public async Task<UserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        /* check if user exist
         * check if pending expense exist with this user
         */
        await validate.IdGreaterThanZeroAsync(request.UserId, cancellationToken);
        
        var fromdb = await validate.RecordExistAsync<User>(x => x.UserId == request.UserId, cancellationToken);
        
        var pendingExpense = await validate.RecordNotExistAsync<Expense>(x => x.UserId == request.UserId && x.Status == ExpenseRequestStatus.Pending, cancellationToken);
        
        fromdb.IsActive = false;
        dbContext.Remove(fromdb);
        await dbContext.SaveChangesAsync(cancellationToken);

        var mapped = mapper.Map<User, UserResponse>(fromdb);
        return mapped;
    }

    public async Task<UserResponse> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
    {
        await validate.IdGreaterThanZeroAsync(request.UserId, cancellationToken);
        
        var fromdb = await validate.RecordExistAsync<User>(x => x.UserId == request.UserId, cancellationToken);
        
        fromdb.IsActive = true;
        await dbContext.SaveChangesAsync(cancellationToken);
        
        var mapped = mapper.Map<User, UserResponse>(fromdb);
        return mapped;

    }

    public async Task<UserResponse> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        await validate.IdGreaterThanZeroAsync(request.UserId, cancellationToken);
        
        var fromdb = await validate.RecordExistAsync<User>(x => x.UserId == request.UserId, cancellationToken);
        
        fromdb.IsActive = false;
        await dbContext.SaveChangesAsync(cancellationToken);
        
        var mapped = mapper.Map<User, UserResponse>(fromdb);
        return mapped;
    }
}