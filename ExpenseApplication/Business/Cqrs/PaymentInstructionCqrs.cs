using Infrastructure.Dtos;
using MediatR;

namespace Application.Cqrs;

public record CreatePaymentInstructionCommand(CreatePaymentInstructionRequest Model) : IRequest<PaymentInstructionResponse>;
public record UpdatePaymentInstructionCommand(int PaymentInstructionId, UpdatePaymentInstructionRequest Model) : IRequest<PaymentInstructionResponse>;
public record DeletePaymentInstructionCommand(int PaymentInstructionId) : IRequest<PaymentInstructionResponse>;

public record GetAllPaymentInstructionQuery() : IRequest<List<PaymentInstructionResponse>>;
public record GetPaymentInstructionByIdQuery(int PaymentInstructionId) : IRequest<PaymentInstructionResponse>;