using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ModuleLibrary
{
    public class Manager : IAdmin
    {
        private string Post;
        public Manager()
        {
            Post = "Manager";
        }
        public override string ToString()
        {
            return Post;
        }
        public void EditProfile(Client client, EditField field, String newValue)
        {
            client.GetChanged(field, newValue, this);
            Repository.Save();
        }
        public void AddNewProfile(string name, string secondName, string patronymic, string phone, string passport)
        {
            Repository.AddNote(new Client(name, secondName, patronymic, phone, passport), Post);
            Repository.Save();
        }
    }
}
