using MediatR;
using Schemes.Dtos;

namespace Business.Cqrs;

public record CreateUserCommand(CreateUserRequest Model) : IRequest<UserResponse>;
public record UpdateUserCommand(int UserId, UpdateUserRequest Model) : IRequest<UserResponse>;
public record DeleteUserCommand(int UserId) : IRequest<UserResponse>;
public record ActivateUserCommand(int UserId) : IRequest<UserResponse>;
public record DeactivateUserCommand(int UserId) : IRequest<UserResponse>;

public record GetAllUserQuery() : IRequest<List<UserResponse>>;
public record GetUserByIdQuery(int UserId) : IRequest<UserResponse>;