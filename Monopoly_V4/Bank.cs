using Monopoly_V4.Enums;
using Monopoly_V4.Interfaces;
using Monopoly_V4.Spaces.PropertySpaces;

namespace Monopoly_V4
{
    public class Bank
    {
        public IReadOnlyDictionary<PlayerToken, int> MoneyByPlayersPlaying { get => playerMoney; }
        public IReadOnlyList<TransactionInfo> Transactions { get => closedTransactions.Select(x => x.Info).ToList().Concat(openTransactions.Keys.Select(x => x.Info)).ToList(); }
        public int Houses { get => houses; private set => houses = 0 <= value && value <= totalHouses ? value : throw new ArgumentOutOfRangeException(nameof(value)); }
        public int Hotels { get => hotels; private set => hotels = 0 <= value && value <= totalHotels ? value : throw new ArgumentOutOfRangeException(nameof(value)); }

        private readonly List<Transaction> closedTransactions;
        private readonly Dictionary<Transaction, IValuable?> openTransactions;
        private readonly Dictionary<PlayerToken, int> playerMoney;
        private readonly int totalHouses;
        private readonly int totalHotels;
        private int houses;
        private int hotels;
        private int pendingHouses;
        private int pendingHotels;
        
        public Bank(PlayerToken[] playerTokens)
        {
            ArgumentNullException.ThrowIfNull(playerTokens, nameof(playerTokens));
            closedTransactions = [];
            openTransactions = [];
            playerMoney = [];
            foreach (var token in playerTokens)
                playerMoney.Add(token, 1500);
        }

        private TransactionInfo StartTransaction(PlayerToken? payee, PlayerToken? payer, int amount, bool required, IValuable? valuableInvolved = null, Action? onComplete = null, Action? onCancel = null)
        {
            if (payee == null && payer == null) throw new ArgumentException("Both payee and payer can not be null.");
            if (payee is PlayerToken p && !playerMoney.ContainsKey(p)) throw new ArgumentException($"{nameof(payee)}, player token not found.");
            if (payer is PlayerToken r && !playerMoney.ContainsKey(r)) throw new ArgumentException($"{nameof(payer)}, player token not found.");
            ArgumentOutOfRangeException.ThrowIfNegative(amount, nameof(amount));
            
            var transaction = new Transaction(payee, payer, amount, required, onComplete, onCancel);
            openTransactions.Add(transaction, valuableInvolved);
            return transaction.Info;
        }

        public TransactionInfo StartTransaction(PlayerToken? payee, PlayerToken? payer, int amount, bool required) // For any transaction that does not require valuable to be locked (fee, salary etc)
        {
            if (payee == null && payer == null) throw new ArgumentException("Both payee and payer can not be null.");
            if (payee is PlayerToken p && !playerMoney.ContainsKey(p)) throw new ArgumentException($"{nameof(payee)}, player token not found.");
            if (payer is PlayerToken r && !playerMoney.ContainsKey(r)) throw new ArgumentException($"{nameof(payer)}, player token not found.");
            ArgumentOutOfRangeException.ThrowIfNegative(amount, nameof(amount));

            return StartTransaction(payee, payer, amount, required, null, null, null);        
        }
        public TransactionInfo RequestBuilding(Street street)
        {
            ArgumentNullException.ThrowIfNull(street, nameof(street));
            if (street.Owner == null) throw new ArgumentException("Street does not have an owner.");
            if (openTransactions.ContainsValue(street)) throw new ArgumentException("Street is already involved in a transaction.");

            var nextUpgrade = street.GetNextBuildingType() ?? throw new ArgumentException("Street is not eligible for upgrade.");
            if (nextUpgrade == BuildingType.House)
            {
                if (houses == 0) throw new InvalidOperationException("No buildings left in bank.");
            }                
            else
            {
                if (hotels == 0) throw new InvalidOperationException("No buildings left in bank.");
            }
                

            RemoveBuilding(nextUpgrade);
            return StartTransaction(null, street.Owner.PlayerToken, street.BuildingCost, true, street, () => { street.UpgradeBuilding(); ConfirmPendingBuilding(nextUpgrade); }, null);                        
        }
        public TransactionInfo TakeBuilding(Street street)
        {
            ArgumentNullException.ThrowIfNull(street, nameof(street));
            if (street.Owner == null) throw new ArgumentException("Street does not have an owner.");
            if (openTransactions.ContainsValue(street)) throw new ArgumentException("Street is already involved in a transaction.");
            throw new NotImplementedException();
        }
        private void ConfirmPendingBuilding(BuildingType buildingType)
        {
            if (buildingType == BuildingType.House)
                pendingHouses--;
            else
            {
                pendingHotels--;
                houses++;
            }                
        }
        private void RemoveBuilding(BuildingType buildingType)
        {
            if (buildingType == BuildingType.House)
            {
                houses--;
                pendingHouses++;
            } 
            else
            {
                hotels--;
                pendingHotels++;
            }
        }
        public TransactionInfo Trade(Player customer, IValuable valuable, int amount)
        {
            ArgumentNullException.ThrowIfNull(customer, nameof(customer));
            ArgumentNullException.ThrowIfNull(valuable, nameof(valuable));
            ArgumentOutOfRangeException.ThrowIfNegative(amount, nameof(amount));

            void onComplete() => valuable.ChangeOwnership(customer);
            Action? onCancel = null;
            if (valuable.Owner == null)
            {
                // TODO: Action on cancel should start bid
            }

            return StartTransaction(customer.PlayerToken, valuable.Owner?.PlayerToken, amount, false, valuable, onComplete, onCancel);
        }
        public TransactionInfo MortgageProperty(IRealStateProperty property)
        {
            ArgumentNullException.ThrowIfNull(property, nameof(property));
            if (property.Owner == null) throw new ArgumentException("Property is not owned.");

            var owner = property.Owner;
            var amount = property.CalculateMortgageAmount();
            return StartTransaction(null, owner.PlayerToken, amount, false, property, property.MortgageProperty);
        }
        public TransactionInfo UnMortgageProperty(IRealStateProperty property)
        {
            ArgumentNullException.ThrowIfNull(property, nameof(property));
            if (property.Owner == null) throw new ArgumentException("Property is not owned.");

            var owner = property.Owner;
            var amount = property.CalculateUnMortgageAmount();
            return StartTransaction(owner.PlayerToken, null, amount, false, property, property.UnMortgageProperty, null);
        }
        public void CompleteTransaction(TransactionInfo info)
        {
            ArgumentNullException.ThrowIfNull(info, nameof(info));
            if (!Transactions.Contains(info)) throw new ArgumentException("Invalid Transaction Info. Transaction not found.");
            if (info.State == TransactionState.Closed) throw new InvalidOperationException("Transaction is already closed.");

            var transaction = openTransactions.Keys.Where(x => x.Info == info).First();            
            if (info.Payer != null)
            {
                var sufficientFunds = (playerMoney[info.Payer!.Value] - info.Amount) >= 0;
                if (!sufficientFunds && info.Required)
                    throw new InvalidOperationException("Insufficient Funds");
                else if (!(sufficientFunds || info.Required))
                {
                    CancelTransaction(info);
                    return;
                }                                    
            }

            transaction.Complete();
            TransferMoney(info.Payee, info.Payer, info.Amount);            
            TransactionStateClosed(transaction);
        }
        public void CancelTransaction(TransactionInfo info)
        {
            ArgumentNullException.ThrowIfNull(info, nameof(info));
            if (!Transactions.Contains(info)) throw new ArgumentException("Invalid Transaction Info. Transaction not found.");
            if (info.State == TransactionState.Closed) throw new InvalidOperationException("Transaction is already closed.");
            if (info.Required) throw new InvalidOperationException("Transaction cannot be cancelled. It is required.");

            var transaction = openTransactions.Keys.Where(x => x.Info == info).First();
            transaction.Cancel();
            TransactionStateClosed(transaction);
        }

        private void TransactionStateClosed(Transaction transaction)
        {
            ArgumentNullException.ThrowIfNull(transaction, nameof(transaction));
            openTransactions.Remove(transaction);
            closedTransactions.Add(transaction);
        }
        private void TransferMoney(PlayerToken? payee, PlayerToken? payer, int amount)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(amount, nameof(amount));
            if (payer == null && payee == null) throw new ArgumentException("Payer and Payee can not be null.");
            if (payer != null && (playerMoney[payer!.Value] - amount) < 0) throw new InvalidOperationException("Insufficient Funds.");

            if (payer == null)
                playerMoney[payee!.Value] += amount; // Bank gives money to payee
            else if (payee == null)
                playerMoney[payer!.Value] -= amount; // Bank takes money from payer
            else
            {
                playerMoney[payer!.Value] -= amount;
                playerMoney[payee!.Value] += amount;
            }                
        }

        private class Transaction
        {
            public TransactionInfo Info { get; }
            private readonly Action? onComplete;
            private readonly Action? onCancel;
            private TransactionState state;

            public Transaction(PlayerToken? payee, PlayerToken? payer, int amount, bool required, Action? onComplete, Action? onCancel)
            {
                Info = new TransactionInfo(
                        payee,
                        payer,
                        amount,
                        required,
                        GetState
                    );
                this.onComplete = onComplete;
                this.onCancel = onCancel;
            }
            private TransactionState GetState() { return state; }
            private void Close() { state = TransactionState.Closed; }
            public void Complete()
            {
                onComplete?.Invoke();
                Close();
            }
            public void Cancel()
            {
                onCancel?.Invoke();
                Close();
            }
        }

    }
}