using Infrastructure.Dtos;
using MediatR;

namespace Application.Cqrs;

public record CreateTokenCommand(TokenRequest Model) : IRequest<TokenResponse>;