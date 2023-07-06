using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ModuleLibrary
{
    public class Operation
    {
        public Moves Move { get; set; }
        public string? Profile { get; set; }
        public string? User { get; set; }
        public DateTime Time { get; set; }
        [JsonIgnore]
        public string? Comment { get; set; }
        public override string ToString()
        {
            switch (Move)
            {
                case Moves.Initialization: return $"new profile has been initialized : {Profile}";
                case Moves.ChangingProfile: return $"new changes by : {User}\nprofile : {Profile} {Comment}";
                case Moves.OpenAccount: return $"{Profile} opened new account : {Comment}";
                case Moves.Transation: return $"{Profile} transated to {Comment}";
                case Moves.FillUp: return $"{Profile} added to account : {Comment}";
                default: return $"{Profile} closed account : {Comment}";
            }
        }

    }
    public enum Moves
    {
        Initialization,
        ChangingProfile,
        OpenAccount,
        Transation,
        FillUp,
        CloseAccount,
    }
}
