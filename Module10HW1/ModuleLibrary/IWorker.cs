using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ModuleLibrary
{
    public interface IWorker
    {
        public string ToString();
        public void EditProfile(Client client, EditField field, String newValue);
    }
    public interface IAdmin : IWorker
    {
        public void AddNewProfile(string name, string secondName, string patronymic, string phone, string passport);
    }
}
