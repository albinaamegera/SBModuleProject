using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ModuleLibrary
{
    public class Consultant : IWorker
    {
        private string Post;
        public Consultant()
        {
            Post = "Consultant";
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
    }
}
