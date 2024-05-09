using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsPlayClassLibrary
{

    public static class Model
    {
        // properties
        public static List<User> Users { get; set; }
        public static List<Account> Accounts { get; set; }
        public static List<Project> Projects { get; set; }
        public static List<decimal> Balances = new List<decimal>();


        // constructor
        static Model()
        {
            Users = new List<User>();
            Accounts = new List<Account>();
            Projects = new List<Project>();
            Balances = new List<decimal> { };
        }
    }
}

