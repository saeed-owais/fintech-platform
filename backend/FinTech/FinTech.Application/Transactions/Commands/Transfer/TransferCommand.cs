using FinTech.Application.common;
using MediatR;

namespace FinTech.Application.Transactions.Commands.Transfer
{
    public record TransferCommand(string ReceiverEmail, decimal Amount) : IRequest<Result<TransferResponse>>;
}
