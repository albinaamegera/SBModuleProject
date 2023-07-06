using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Nodes;
using System.Windows;

namespace ModuleLibrary
{
    public static class Repository
    {
        private static string data = @"data.json";
        private static JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
        public static event Action<Client>? InitializeClient;
        public static event Action<Operation>? Operation;
        public static ObservableCollection<Client> Clients { get; private set; }
        public static ObservableCollection<Operation> Operations { get; private set; }

        static Repository()
        {
            Clients = new();
            Operations = new();
            Deserialise();
        }
        public static void AddNote(Client newClient, string user)
        {
            newClient.Change += NewOperation;
            Clients.Add(newClient);
            Operations.Add(new Operation
            {
                Move = Moves.Initialization,
                Profile = newClient.Name,
                User = user,
                Time = DateTime.Now
            });
            InitializeClient?.Invoke(newClient);
        }
        public static void NewOperation(Operation operation)
        {
            Operations.Add(operation);
            Operation?.Invoke(operation);
        }
        public static List<Client> GetList()
        {
            return Clients.ToList();
        }
        public static void Sort(SortBy field)
        {
            List<Client> clients = Clients.ToList<Client>();
            clients.Sort(Client.SortedBy(field));
            Clients.Clear();
            foreach (var c in clients)
            {
                Clients.Add(c);
            }
        }
        public static void Save()
        {
            Serialize();
        }
        private static void Serialize()
        {
            JsonArray array = new JsonArray();

            foreach (var client in Clients)
            {
                array.Add(client.ToJson());
            }

            var obj = new JsonObject
            {
                ["Clients"] = array,
                ["Operations"] = JsonSerializer.SerializeToNode(Operations)
            };
            string json = obj.ToJsonString(options);
            File.WriteAllText(data, json);
        }
        private static void Deserialise()
        {
            string json = File.ReadAllText(data);
            JsonNode root = JsonNode.Parse(json)!;
            JsonArray clientArray = root["Clients"]!.AsArray();
            foreach (var client in clientArray)
            {
                var c = new Client(client);
                c.Change += NewOperation;
                Clients.Add(c);
            }
            var list = JsonSerializer.Deserialize<List<Operation>>(root["Operations"]);
            if (list != null)
            {
                foreach (var o in list)
                {
                    Operations.Add(o);
                }
            }
        }
    }
}
