using System;
using Airbnb.Models;
using Airbnb.Data;

namespace Airbnb.Services
{
    public class BookingService
    {
        private IRepository<User> userRepo = new FileRepository<User>("users.json");

        public void Start()
        {
            while (true)
            {
                Console.WriteLine("\n--- MENU ---");
                Console.WriteLine("1. Krijo User");
                Console.WriteLine("2. Shfaq Users");
                Console.WriteLine("0. Exit");

                string input = Console.ReadLine();

                if (input == "1") CreateUser();
                else if (input == "2") ShowUsers();
                else if (input == "0") break;
            }
        }

        private void CreateUser()
        {
            Console.Write("Emri: ");
            string name = Console.ReadLine();
            userRepo.Add(new User
            {
                Id = userRepo.GetAll().Count + 1,
                Name = name
            });
            userRepo.Save();
            Console.WriteLine("User u krijua!");
        }

        private void ShowUsers()
        {
            foreach (var user in userRepo.GetAll())
                Console.WriteLine($"ID: {user.Id}, Emri: {user.Name}");
        }
    }
}