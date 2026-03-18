using Airbnb.Models;
using Airbnb.Data;
using System;

namespace Airbnb.Services
{
    public class UserService
    {
        private IRepository<User> repo = new FileRepository<User>();

        public void CreateUser()
        {
            Console.Write("Emri: ");
            string name = Console.ReadLine();

            User user = new User
            {
                Id = 0,
                Name = name
            };

            repo.Add(user);
            repo.Save();

            Console.WriteLine("User u kriju!");
        }
    }
}