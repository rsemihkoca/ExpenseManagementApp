using Infrastructure.Dtos;
using MediatR;
using Schemes.Dtos;

namespace Business.Cqrs;

public record CreateTokenCommand(TokenRequest Model) : IRequest<TokenResponse>;