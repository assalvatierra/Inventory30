using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiClass.ViewModels
{
    public class Person
    {
        public Person(string name, string contact)
        {
            this.Name = name;
            this.Contact = contact;
        }

        public string Name { get; set; }
        public string Contact { get; set; }

    }
}
