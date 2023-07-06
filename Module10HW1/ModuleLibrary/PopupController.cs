using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ModuleLibrary
{
    public class PopupController
    {
        private IWorker user;
        public event Action<string, DateTime>? ShowPopup;
        public PopupController(IWorker user)
        {
            this.user = user;
            Repository.InitializeClient += Initialize;
            Repository.Operation += Operation;
        }
        private void Initialize(Client client)
        {
            Operation(new Operation
            {
                Move = Moves.Initialization,
                Profile = client.Name,
                Time = DateTime.Now
            });
        }
        private void Operation(Operation operation)
        {
            operation.User = user.ToString();
            ShowPopup?.Invoke(operation.ToString(), operation.Time);
        }
        
    }
    
}
