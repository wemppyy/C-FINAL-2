using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using C__FINAL_2.models;

namespace C__FINAL_2.services
{
    internal class QuizService
    {
        private static readonly string projectRoot = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        private static readonly string quizzesDirectory = Path.Combine(projectRoot, "data", "quizzes");
        private List<Quiz> _quizzes;

        public StatisticService statisticService { get; set; }

        public QuizService()
        {
            LoadQuizzes();
            statisticService = new StatisticService();
        }

        public void LoadQuizzes()
        {
            _quizzes = new List<Quiz>();
            string[] quizFiles = Directory.GetFiles(quizzesDirectory);
            foreach (string quizFile in quizFiles)
            {
                string quizJson = File.ReadAllText(quizFile);
                Quiz quiz = JsonSerializer.Deserialize<Quiz>(quizJson);
                _quizzes.Add(quiz);
            }
        }

        

        

        public Quiz GetQuiz(int index)
        {
            return _quizzes[index];
        }

        public string GetQuizTitles()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _quizzes.Count; i++)
            {
                sb.AppendLine($"{i + 1}. {_quizzes[i].Title}");
            }
            return sb.ToString();
        }

        

        public void StartQuiz(User user, Quiz quiz)
        {
            Statistic statistic = new Statistic
            {
                UserLogin = user.Login,
                QuizTitle = quiz.Title,
                Score = 0,
                UserTime = DateTime.Now
            };
            int questionIndex = 1;
            foreach (Question question in quiz.Questions)
            {
                Console.Clear();
                Console.WriteLine($"({questionIndex++}/{quiz.Questions.Count}) {question.Text}\n");
                int i = 1;
                foreach (string option in question.Answers)
                {
                    Console.WriteLine($"{i++}. {option}");
                }
                Console.Write("\nEnter your answer (index): ");
                int answerIndex = int.Parse(Console.ReadLine());
                if (answerIndex == question.CorrectAnswer)
                {
                    statistic.Score++;
                }
            }
            statisticService.AddStatistic(statistic);

            Console.Clear();
            Console.WriteLine($"Your score: {statistic.Score}");

            Console.WriteLine("\nTop scores:");
            statisticService.PrintTopByQuiz(quiz, 5);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
