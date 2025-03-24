using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using C__FINAL_2.models;
using C__FINAL_2.services;

namespace C__FINAL_2.services
{
    internal class QuizCreator
    {
        private static readonly string projectRoot = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        private static readonly string quizzesDirectory = Path.Combine(projectRoot, "data", "quizzes");

        public void CreateQuiz(QuizService quizService)
        {
            Console.WriteLine("Enter the title of the quiz");
            string title = Console.ReadLine();

            Console.WriteLine("Enter the count of the questions: ");
            int questionsCount = int.Parse(Console.ReadLine());

            List<Question> questions = new List<Question>();

            for (int i = 0; i < questionsCount; i++)
            {
                Console.Clear();
                Console.WriteLine($"({i+1} / {questionsCount})");
                Console.Write("Enter the text of question: ");
                string questionText = Console.ReadLine();

                Console.Write("Enter your answers to the questions separated by a comma: ");
                string answersText = Console.ReadLine();
                List<string> answers = answersText.Split(',')
                                                 .Select(item => item.Trim())
                                                 .Where(item => !string.IsNullOrEmpty(item))
                                                 .ToList();

                Console.Write("Enter the INDEX of the correct answer (first is 1): ");
                int correctAnswer = int.Parse(Console.ReadLine());

                Question question = new Question();
                question.Text = answersText;
                question.Answers = answers;
                question.CorrectAnswer = correctAnswer;

                questions.Add(question);
            }

            Quiz quiz = new Quiz();
            quiz.Title = title;
            quiz.Questions = questions;

            saveQuiz(quiz);
            quizService.LoadQuizzes();

            Console.WriteLine("Quiz succesfully created!");
        }

        private void saveQuiz(Quiz quiz)
        {
            string quizJson = JsonSerializer.Serialize(quiz);
            string quizPath = quizzesDirectory + "/" + quiz.Title + ".json";
            File.WriteAllText(quizPath, quizJson);
        }
    }
}
