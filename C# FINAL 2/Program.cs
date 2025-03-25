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
            Console.Write("Enter your login: ");
            string login = Console.ReadLine();
            Console.Write("Enter your password: ");
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

        static User Login(UserService userService, string login, string password)
        {
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

        static void SettingsMenu(UserService userService, User user)
        {
            Console.Clear();
            Console.WriteLine("1. Change password");
            Console.WriteLine("2. Change birth date");
            Console.Write("Enter your choice: ");

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.Clear();

                    Console.Write("Enter the OLD password: ");
                    string oldPassword = Console.ReadLine();
                    Console.Write("Enter the NEW password: ");
                    string newPassword = Console.ReadLine();

                    try
                    {
                        userService.ChangePassword(user, oldPassword, newPassword);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return;
                    }

                    break;
                case 2:
                    Console.Clear();

                    Console.WriteLine("Enter the NEW birth date (yyyy-MM-dd)");
                    DateTime birthDate = DateTime.Parse(Console.ReadLine());
                    userService.ChangeBirthDate(user, birthDate);
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    break;


            }
        }

        static void AdminMenu(QuizService quizService, QuizCreator quizCreator, UserService userService)
        {
            Console.Clear();
            Console.WriteLine("1. Create a new quiz");
            Console.WriteLine("2. Assign a user as an administrator");
            Console.WriteLine("3. Unassign a user as an administrator");
            Console.WriteLine("Enter your choice: ");

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    quizCreator.CreateQuiz(quizService);
                    break;
                case 2:
                    Console.Clear();
                    userService.printAllUsers();
                    Console.WriteLine("Enter user index");
                    int userIndex = int.Parse(Console.ReadLine());
                    userService.assignUserAsAdmin(userService.getUserByIndex(userIndex - 1));
                    break;
                case 3:
                    Console.Clear();
                    userService.printAllUsers();
                    Console.WriteLine("Enter user index");
                    int userIndex2 = int.Parse(Console.ReadLine());
                    userService.unassignUserAsAdmin(userService.getUserByIndex(userIndex2 - 1));
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        }

        static void MainMenu (QuizService quizService, User user, UserService userService, QuizCreator quizCreator)
        {
            Console.Clear();

            int menuCount = 6;

            Console.WriteLine("1. Start a new quiz");
            Console.WriteLine("2. Review your results");
            Console.WriteLine("3. Review the Top 20 from a specific quiz");
            Console.WriteLine("4. Change settings");
            Console.WriteLine("5. Exit");
            Console.WriteLine("---------");

            if (user.IsAdmin)
            {
                Console.WriteLine("6. ADMIN MODE");
            }

            Console.Write("Enter your choice: ");

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    QuizMenu(quizService, user);
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine(quizService.GetQuizTitles());
                    Console.Write("Enter the index of the quiz: ");
                    int index2 = int.Parse(Console.ReadLine());

                    Console.Clear();
                    quizService.statisticService.PrintResultsByUserAndQuiz(quizService.GetQuiz(index2 - 1), user);
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
                case 3:
                    Console.Clear();
                    Console.WriteLine(quizService.GetQuizTitles());
                    Console.Write("Enter the index of the quiz: ");
                    int index3 = int.Parse(Console.ReadLine());

                    Console.Clear();
                    quizService.statisticService.PrintTopByQuiz(quizService.GetQuiz(index3 - 1), 20);
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
                case 4:
                    SettingsMenu(userService, user);
                    break;
                case 5:
                    Environment.Exit(0);
                    break;
                case 6:
                    Console.Clear();
                    if (user.IsAdmin)
                    {
                        AdminMenu(quizService, quizCreator, userService);
                    } else
                        Console.WriteLine("Access denied. You are not an admin!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }

        }

        static void Main(string[] args)
        {
            UserService userService = new UserService();

#if ADMIN_MODE
            User user = Login(userService, "admin", "admin");
#else
            User user = null;

            while (user == null)
            {
                Console.Clear();

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

            QuizCreator quizCreator = new QuizCreator();

            while (true)
            {
                MainMenu(quizService, user, userService, quizCreator);
            }
        }
    }
}