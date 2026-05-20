using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo
{
    internal static class CacheSession
    {
        public static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=demo;Integrated Security=true";
        public static Session user { get; set; }
    }
    internal class Session
    {
        public UserRole Role { get; set; }
        public string SurName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }

        public enum UserRole
        {
            Guest,
            Client,
            Manager,
            Admin
        }
    }
}
