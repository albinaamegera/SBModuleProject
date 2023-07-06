using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace ModuleLibrary
{
    interface IAccountable<out T> where T : BankAccount
    {
        public T AddAccount();
    }
    interface IAccount<in T> where T : BankAccount
    {
        public void FillUp(T account, string amount);
    }
    public abstract class BankAccount : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private string name;
        private uint amount;
        private ushort rate = 0;
        protected string info;
        public string Name 
        { 
            get => name;
            protected set => name = value; 
        }
        public uint Amount 
        { 
            get => amount;
            protected set => amount = value;
        }
        public ushort Rate 
        { 
            get => rate;
            protected set => amount = value; 
        }
        public string Info
        {
            get => info;
            set
            {
                info = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(info)));
            }
        }
        public BankAccount(uint startValue, string name)
        {
            this.amount = startValue;
            this.name = name;
            SetInfo(Amount);
        }
        public BankAccount(uint startValue, ushort rate, string name) : this (startValue, name)
        {
            this.rate = rate;
            SetInfo(Amount, Rate);
        }
        public void FillUp(uint amount)
        {
            Amount += amount;
            SetInfo(Amount);
        }
        public void Transacted(BankAccount account, uint amount)
        {
            Amount -= amount;
            account.FillUp(amount);
            SetInfo(Amount);
        }
        public bool Transactable(uint amount) => Amount < amount;
        protected void SetInfo(uint amount)
        {
            info = string.Format("Amount : {0}", amount);
        }
        protected void SetInfo(uint amount, ushort rate)
        {
            info = string.Format("Amount : {0}\nInterest Rate : {1}%", amount, rate);
        }
    }

    public class BasicAccount : BankAccount , IAccountable<BasicAccount>
    {
        public BasicAccount(uint startValue) : base(startValue, "Basic") { }
        public BasicAccount AddAccount()
        {
            return this;
        }
    }
    public class DepositAccount : BankAccount , IAccountable<DepositAccount>
    {
        public DepositAccount(uint startValue, ushort rate) : base(startValue, rate, "Deposit") { }
        public DepositAccount AddAccount()
        {
            return this;
        }
    }
    public class MyAccounts : IAccount<BankAccount>
    {
        private IAccountable<BankAccount>? basic;
        private IAccountable<BankAccount>? deposit;
        public Client AccountsHolder { get; private set; }
        public event Action<Operation>? Operation;
        public ObservableCollection<BankAccount> Accounts { get; private set; } = new ObservableCollection<BankAccount>();
        public MyAccounts(Client client) => AccountsHolder = client;
        #region methods
        internal void NewAccount(Type type, uint startValue, ushort rate)
        {
            if (type == typeof(BasicAccount))
            {
                AddAccount(startValue);
            }
            else
            {
                AddAccount(startValue, rate);
            }
        }
        // open a basic account
        private void AddAccount(uint startValue)
        {
            basic = new BasicAccount(startValue);
            Accounts.Add(basic.AddAccount());
            Operation?.Invoke(new Operation
            {
                Move = Moves.OpenAccount,
                Profile = AccountsHolder.Name,
                Time = DateTime.Now,
                Comment = "Basic"
            });
            Save();
        }
        // override to open a deposit account
        private void AddAccount(uint startValue, ushort rate)
        {
            deposit = new DepositAccount(startValue, rate);
            Accounts.Add(deposit.AddAccount());
            Operation?.Invoke(new Operation
            {
                Move = Moves.OpenAccount,
                Profile = AccountsHolder.Name,
                Time = DateTime.Now,
                Comment = "Deposit"
            });
            Save();
        }
        public void TransactTo(MyAccounts acc, string amount)
        {
            uint _amount;
            if (!uint.TryParse(amount, out _amount))
            {
                throw new ModuleException("not correct input value !");
            }
            if (Accounts[GetAccountIndex<BasicAccount>()].Transactable(_amount))
            {
                throw new ModuleException("not enough amount on the account");
            }
            Accounts[GetAccountIndex<BasicAccount>()].Transacted(acc.GetAccount<BasicAccount>(), _amount);
            Operation?.Invoke(new Operation
            {
                Move = Moves.Transation,
                Profile = AccountsHolder.Name,
                Time = DateTime.Now,
                Comment = $"{acc.AccountsHolder.Name} : {_amount}"
            });
            Save();
        }
        public void FillUp(BankAccount account, string amount)
        {
            uint _amount;
            if (!uint.TryParse(amount, out _amount))
            {
                throw new ModuleException("not correct input value !");
            }
            account.FillUp(_amount);
            Operation?.Invoke(new Operation
            {
                Move = Moves.FillUp,
                Profile = AccountsHolder.Name,
                Time = DateTime.Now,
                Comment = $"{_amount}"
            });
            Save();
        }
        public void Close(int index)
        {
            Operation?.Invoke(new Operation
            {
                Move = Moves.CloseAccount,
                Profile = AccountsHolder.Name,
                Time = DateTime.Now
            });
            Accounts.RemoveAt(index);
            Save();
        }
        
        internal bool DetectAccount<T>() where T: BankAccount
        {
            return Accounts.Any(e => e.GetType() == typeof(T));
        }
        public BankAccount GetAccount<T>()
        {
            var list = Accounts.ToList();
            return list.Find(e => e.GetType() == typeof(T));
        }
        public int GetAccountIndex<T>()
        {
            var list = Accounts.ToList();
            return list.FindIndex(e => e.GetType() == typeof(T));
        }
        private void Save() => Repository.Save();

        #endregion
        #region Serialization & Deserialization
        
        public JsonObject ToJson()
        {
            JsonObject obj = new JsonObject();
            foreach (var account in Accounts)
            {
                JsonObject o = new JsonObject();
                if (account.GetType() == typeof(BasicAccount))
                {
                    o["Amount"] = account.Amount;
                    obj["Basic"] = o;
                }
                else
                {
                    o["Amount"] = account.Amount;
                    o["Rate"] = account.Rate;
                    obj["Deposit"] = o;
                }
            }
            return obj;
        }
        public void Deserialize(JsonNode? node)
        {
            if (node == null) return;
            if (node["Basic"] != null)
            {
                AddAccount(node["Basic"]!["Amount"]!.GetValue<uint>());
            }
            if (node["Deposit"] != null)
            {
                AddAccount(node["Deposit"]!["Amount"]!.GetValue<uint>(), node["Deposit"]!["Rate"]!.GetValue<ushort>());
            }
        }
        #endregion
    }
}
