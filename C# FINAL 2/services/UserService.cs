using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using C__FINAL_2.models;

namespace C__FINAL_2.services
{
    internal class UserService
    {
        private static readonly string projectRoot = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        private static readonly string _usersFile = Path.Combine(projectRoot, "data", "users.json");

        private List<User> _users;

        public UserService()
        {
            OpenUsers();
        }

        public User GetUser(string login)
        {
            return _users.Find(u => u.Login == login);
        }

        private void SaveUsers()
        {
            string usersJson = JsonSerializer.Serialize(_users);
            File.WriteAllText(_usersFile, usersJson);
        }
        private void OpenUsers()
        {
            if (!File.Exists(_usersFile))
            {
                _users = new List<User>();
                SaveUsers();
            }
            string usersJson = File.ReadAllText(_usersFile);
            _users = JsonSerializer.Deserialize<List<User>>(usersJson);
        }

        public void Login(string login, string password)
        {
            User user = _users.Find(u => u.Login == login && u.Password == password);
            if (user == null)
            {
                throw new Exception("User not found");
            }
        }

        public void Register(string login, string password, DateTime birthDate)
        {
            User user = new User
            {
                Login = login,
                Password = password,
                BirthDate = birthDate
            };
            _users.Add(user);
            SaveUsers();
        }
    }
}
