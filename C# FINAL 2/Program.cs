#define ADMIN_MODE

using System;
using C__FINAL_2.services;
using C__FINAL_2.models;

namespace C__FINAL_2
{
    class Program
    {
        static User Login(UserService userService)
        {
            Console.Clear();
            Console.WriteLine("Enter your login: ");
            string login = Console.ReadLine();
            Console.WriteLine("Enter your password: ");
            string password = Console.ReadLine();

            Console.WriteLine();
            try
            {
                userService.Login(login, password);
                return userService.GetUser(login);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        static void Register(UserService userService)
        {
            Console.Clear();

            Console.Write("Enter your login: ");
            string login = Console.ReadLine();
            Console.Write("Enter your password: ");
            string password = Console.ReadLine();
            Console.Write("Enter your birth date (yyyy-MM-dd): ");
            DateTime birthDate = DateTime.Parse(Console.ReadLine());
            userService.Register(login, password, birthDate);
        }

        static void Register(UserService userService, string login, string password, DateTime birthDate)
        {
            userService.Register(login, password, birthDate);
        }

        static void QuizMenu(QuizService quizService, User user)
        {
            Console.Clear();

            Console.WriteLine(quizService.GetQuizTitles());
            Console.Write("Enter the quiz number: ");

            int quizNumber = int.Parse(Console.ReadLine());

            Quiz quiz = quizService.GetQuiz(quizNumber-1);

            quizService.StartQuiz(user, quiz);
        }

        static void Main(string[] args)
        {
            UserService userService = new UserService();

#if ADMIN_MODE
            Register(userService, "admin", "admin", new DateTime(2000, 1, 1));
            User user = userService.GetUser("admin");
#else
            User user = null;

            while (user == null)
            {
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                Console.Write("Enter your choice: ");
                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        user = Login(userService);
                        break;
                    case 2:
                        Register(userService);
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
            }
#endif

            QuizService quizService = new QuizService();

            while (true)
            {
                QuizMenu(quizService, user);
            }
        }
    }
}