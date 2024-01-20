// using Application.Cqrs;
// using Application.Validators;
// using AutoMapper;
// using Business.Entities;
// using IbanNet;
// using Infrastructure.Data.DbContext;
// using Infrastructure.Dtos;
// using MediatR;
//
// namespace Application.Commands;
//
// public class OldUserCommandHandler :
//     IRequestHandler<CreateUserCommand, UserResponse>,
//     IRequestHandler<UpdateUserCommand, UserResponse>,
//     IRequestHandler<DeleteUserCommand, UserResponse>,
//     IRequestHandler<ActivateUserCommand, UserResponse>,
//     IRequestHandler<DeactivateUserCommand, UserResponse>
// {
//     private readonly ExpenseDbContext dbContext;
//     private readonly IMapper mapper;
//     private readonly IHandlerValidator validator;
//
//     public OldUserCommandHandler(
//         ExpenseDbContext dbContext,
//         IMapper mapper,
//         IHandlerValidator validator)
//     {
//         this.dbContext = dbContext;
//         this.mapper = mapper;
//         this.validator = validator;
//     }
//
//     public async Task<UserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
//     {
//         // await validator.ValidateUserNameNotExistAsync(request.Model.Username, cancellationToken);
//         // await validator.ValidateEmailNotExistAsync(request.Model.Email, cancellationToken);
//         // await validator.ValidateIbanNotExistAsync(request.Model.Iban, cancellationToken);
//         await validator.RecordNotExistAsync<User>(x => x.Username == request.Model.Username, cancellationToken);
//         await validator.RecordNotExistAsync<User>(x => x.Email == request.Model.Email, cancellationToken);
//         await validator.RecordNotExistAsync<User>(x => x.Iban == request.Model.Iban, cancellationToken);
//         
//
//
//         var entity = mapper.Map<CreateUserRequest, User>(request.Model);
//
//         var entityResult = await dbContext.AddAsync(entity, cancellationToken);
//         await dbContext.SaveChangesAsync(cancellationToken);
//
//         var mapped = mapper.Map<User, UserResponse>(entityResult.Entity);
//         return mapped;
//     }
//
//     public async Task<UserResponse> Handle(UpdateUserCommand request,
//         CancellationToken cancellationToken)
//     {
//         // var fromdb =
//         //     await validator.ValidateUserIsExistAsync(request.UserId, cancellationToken);
//         // await validator.ValidateUserNameNotExistAsync(request.Model.UserName, cancellationToken);
//         var fromdb = await validator.RecordExistAsync<User>(x => x.UserId == request.UserId, cancellationToken);
//         await validator.RecordNotExistAsync<User>(x => x.Username == request.Model.Username, cancellationToken);
//
//         // fromdb.UserName = request.Model.UserName;
//         await dbContext.SaveChangesAsync(cancellationToken);
//
//         return new UserResponse()
//         {
//             UserId = fromdb.UserId,
//             UserName = fromdb.UserName
//         };
//     }
//
//     public async Task<UserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
//     {
//         var fromdb = await validator.ValidateUserIsExistAsync(request.UserId, cancellationToken);
//
//         await validator.ValidateNoActiveExpenseExistAsync(request.UserId, expense => expense.UserId == request.UserId, cancellationToken);
//
//         dbContext.Remove(fromdb);
//         await dbContext.SaveChangesAsync(cancellationToken);
//
//         return new UserResponse()
//         {
//             UserId = fromdb.UserId,
//             UserName = fromdb.Username,
//             Role = fromdb.Role
//         };
//     }
//
//     public async Task<UserResponse> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
//     {
//         var fromdb = await validator.ValidateUserIsExistAsync(request.UserId, cancellationToken);
//         
//         fromdb.IsActive = true;
//         await dbContext.SaveChangesAsync(cancellationToken);
//         
//         return new UserResponse()
//         {
//             UserId = fromdb.UserId,
//             UserName = fromdb.Username,
//             Role = fromdb.Role
//         };
//
//     }
//
//     public async Task<UserResponse> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
//     {
//         var fromdb = await validator.ValidateUserIsExistAsync(request.UserId, cancellationToken);
//         
//         fromdb.IsActive = false;
//         await dbContext.SaveChangesAsync(cancellationToken);
//         
//         return new UserResponse()
//         {
//             UserId = fromdb.UserId,
//             UserName = fromdb.Username,
//             Role = fromdb.Role
//         };
//     }
// }