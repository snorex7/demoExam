using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demoA
{
    internal static class CurrentSession
    {
        public static Session CurrentUser { get; set; }
    }
    internal class Session
    {
        public UserRole Role { get; set; } = UserRole.Guest;
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
