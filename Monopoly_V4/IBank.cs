using Monopoly_V4.Enums;
using Monopoly_V4.Interfaces;

namespace Monopoly_V4
{
    public interface IBank
    {
        TransactionInfo StartTransaction(PlayerToken? payee, PlayerToken? payer, int amount, bool required, IValuable? valuableInvolved, Action? onComplete, Action? onCancel);
    }
}