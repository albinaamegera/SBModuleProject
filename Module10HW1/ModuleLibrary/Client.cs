using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Windows;
using System.Text.Json.Nodes;

namespace ModuleLibrary
{
    public enum SortBy
    {
        Name,
        SecondName,
        Patronymic,
        PhoneNumber,
        Passport,
        LastChange,
        ChangeType,
        ChangedField,
        ChangeBy,
    }
    public enum EditField
    {
        Name,
        SecondName,
        Patronymic,
        Phone,
        Passport
    }
    public class Client : INotifyPropertyChanged
    {
        private string name;
        private string secondName;
        private string patronymic;
        private string phoneNumber;
        private string passport;
        private string lastChange;
        private string changeType;
        private string changedField;
        private string changedBy;
        
        #region properties
        public event Action<Operation>? Change;
        public string Name 
        { 
            get { return name; }
            private set 
            { 
                name = value;
                ChangedField = "Name";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                OnPropertyChanged(Name);
            }
        }
        public string SecondName 
        { 
            get { return secondName; }
            private set 
            { 
                secondName = value;
                ChangedField = "Second Name";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SecondName)));
                OnPropertyChanged(SecondName);
            }
        }
        public string Patronymic 
        { 
            get { return patronymic; }
            private set 
            { 
                patronymic = value;
                ChangedField = "Patronymic";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Patronymic)));
                OnPropertyChanged(Patronymic);
            }
        }
        public string Phone 
        { 
            get { return phoneNumber; }
            private set 
            { 
                phoneNumber = value;
                ChangedField = "Phone";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Phone)));
                OnPropertyChanged(Phone);
            }
        }
        public string Passport 
        { 
            get { return passport; }
            private set 
            { 
                passport = value;
                ChangedField = "Passport";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Passport)));
                OnPropertyChanged(Passport);
            }
        }
        public string LastChange 
        { 
            get { return lastChange; }
            private set
            {
                lastChange = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastChange)));
            }
        }
        public string ChangeType 
        { 
            get { return changeType; }
            private set
            {
                changeType = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ChangeType)));
            }
        }
        public string ChangedField 
        { 
            get { return changedField; }
            private set
            {
                changedField = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ChangedField)));
            }
        }
        public string ChangedBy 
        { 
            get { return changedBy; }
            private set
            {
                changedBy = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ChangedBy)));
            }
        }
        [JsonIgnore]
        public MyAccounts MyAccs { get; private set; }
        [JsonIgnore] public string HiddenPassport { get { return "*******"; } }
        #endregion

        #region constructors
        public Client(string name, string secondName, string patronymic, string phoneNumber, string passport)
        {
            this.name = name;
            this.secondName = secondName;
            this.patronymic = patronymic;
            this.phoneNumber = phoneNumber;
            this.passport = passport;
            lastChange = DateTime.Now.ToString("g");
            changeType = "Initialization";
            changedField = "-";
            changedBy = "Manager";
            MyAccs = new(this);
            MyAccs.Operation += Operated;
        }
        public Client(JsonNode? node)
        {
            
            name = node["Name"]!.GetValue<string>();
            secondName = node["SecondName"]!.GetValue<string>();
            patronymic = node["Patronymic"]!.GetValue<string>();
            phoneNumber = node["Phone"]!.GetValue<string>();
            passport = node["Passport"]!.GetValue<string>();
            lastChange = node["LastChange"]!.GetValue<string>();
            changeType = node["ChangeType"]!.GetValue<string>();
            changedField = node["ChangedField"]!.GetValue<string>();
            changedBy = node["ChangedBy"]!.GetValue<string>();
            MyAccs = new(this);
            MyAccs.Deserialize(node["Accounts"]);
            MyAccs.Operation += Operated;
        }
        #endregion

        #region methods
        public void GetChanged(EditField field, string newValue, IWorker user)
        {
            ChangedBy = user.ToString();
            switch (field)
            {
                case EditField.Name: { Name = newValue; break; }
                case EditField.SecondName: { SecondName = newValue; break; }
                case EditField.Patronymic: { Patronymic = newValue; break; }
                case EditField.Phone: { Phone = newValue; break; }
                case EditField.Passport: { Passport = newValue; break; }
                default: break;
            }
            LastChange = DateTime.Now.ToString("g");
            ChangeType = "Edited";
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string changedField)
        {
            Change?.Invoke(new Operation
            {
                Move = Moves.ChangingProfile,
                Profile = Name,
                User = ChangedBy,
                Time = DateTime.Now,
                Comment = $"new {ChangedField} : {changedField}"
            });
        }
        public static IComparer<Client> SortedBy(SortBy field)
        {
            return new SortByField(field);
        }
        
        public void CreateAccount(Type type, string amount, string rate = "0")
        {
            uint amountResult;
            ushort rateResult;
            if(!(uint.TryParse(amount, out amountResult) && ushort.TryParse(rate, out rateResult)))
            {
                throw new ModuleException("start amount or/and interested rate is incorrect");
            }
            var flag = type == typeof(BasicAccount) ? MyAccs.DetectAccount<BasicAccount>() : MyAccs.DetectAccount<DepositAccount>();
            if (flag)
            {
                throw new ModuleException("account has been already created");
            }
            else
            {
                MyAccs.NewAccount(type, amountResult, rateResult);
            }
        }
        
        public bool HasAccount<T>() where T : BankAccount
        {
            return MyAccs.Accounts.Any(e => e.GetType() == typeof(T));
        }
        private void Operated(Operation operation)
        {
            Change?.Invoke(operation);
        }
        public JsonObject ToJson()
        {
            return new JsonObject
            {
                ["Name"] = Name,
                ["SecondName"] = SecondName,
                ["Patronymic"] = Patronymic,
                ["Phone"] = Phone,
                ["Passport"] = Passport,
                ["LastChange"] = LastChange,
                ["ChangeType"] = ChangeType,
                ["ChangedField"] = ChangedField,
                ["ChangedBy"] = ChangedBy,
                ["Accounts"] = MyAccs.ToJson()
            };
        }
        #endregion
        private class SortByField : IComparer<Client>
        {
            private SortBy field;
            public SortByField(SortBy field)
            {
                this.field = field;
            }
            public int Compare(Client x, Client y)
            {
                Client X = (Client)x;
                Client Y = (Client)y;

                switch(field)
                {
                    case SortBy.Name: return String.Compare(X.Name, Y.Name);
                    case SortBy.SecondName: return String.Compare(X.SecondName, Y.SecondName);
                    case SortBy.Patronymic: return String.Compare(X.Patronymic, Y.Patronymic);
                    case SortBy.PhoneNumber: return String.Compare(X.Phone, Y.Phone);
                    case SortBy.Passport: return String.Compare(X.Passport, Y.Passport);
                    case SortBy.ChangeType: return String.Compare(X.ChangeType, Y.ChangeType);
                    case SortBy.ChangedField: return String.Compare(X.ChangedField, Y.changedField);
                    case SortBy.ChangeBy: return String.Compare(X.ChangedBy, Y.ChangedBy);
                    case SortBy.LastChange: return CompareDate(X.LastChange, Y.lastChange);
                    default: break;
                }
                return -1;
            }
            private int CompareDate(string x, string y)
            {
                DateTime X = DateTime.ParseExact(x, "dd.MM.yyyy H:mm", CultureInfo.InvariantCulture);
                DateTime Y = DateTime.ParseExact(y, "dd.MM.yyyy H:mm", CultureInfo.InvariantCulture);

                return DateTime.Compare(X, Y);
            }
        }
    }
}
