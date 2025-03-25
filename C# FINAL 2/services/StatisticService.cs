using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using C__FINAL_2.models;

namespace C__FINAL_2.services
{
    internal class StatisticService
    {
        private static readonly string projectRoot = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        private static readonly string statisticsFile = Path.Combine(projectRoot, "data", "statistics.json");
        public List<Statistic> _statistics { get; set; }

        public StatisticService()
        {
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            if (!File.Exists(statisticsFile))
            {
                _statistics = new List<Statistic>();
                SaveStatistics();
            }
            string statisticsJson = File.ReadAllText(statisticsFile);
            _statistics = JsonSerializer.Deserialize<List<Statistic>>(statisticsJson);
        }

        private void SaveStatistics()
        {
            string statisticsJson = JsonSerializer.Serialize(_statistics);
            File.WriteAllText(statisticsFile, statisticsJson);
        }

        private void SortStatistics()
        {
            _statistics = _statistics.OrderByDescending(s => s.Score).ToList();
        }

        public void AddStatistic(Statistic statistic)
        {
            _statistics.Add(statistic);
            SortStatistics();
            SaveStatistics();
        }

        public void PrintTopByQuiz(Quiz quiz, int quantity)
        {
            var filteredStats = _statistics
                .Where(s => s.QuizTitle == quiz.Title)
                .Take(quantity)
                .ToList();

            for (int i = 0; i < quantity; i++)
            {
                if (i < filteredStats.Count)
                    Console.WriteLine($"{i + 1}. {filteredStats[i].UserLogin} - {filteredStats[i].Score}");
                else
                    Console.WriteLine($"{i + 1}. -------");
            }
        }

        public void PrintResultsByUserAndQuiz(Quiz quiz, User user)
        {
            var filteredResults = _statistics
                .Where(s => s.QuizTitle == quiz.Title && s.UserLogin == user.Login)
                .ToList();

            if (filteredResults.Count > 0) {
                var Results = filteredResults.Last().QuestionsAndUserAnswers;

                int i = 1;
                foreach (var pair in Results)
                {
                    Console.WriteLine($"{i}. {pair.Key}");
                    Console.WriteLine($"Your answer: {pair.Value}\n");
                    i++;
                }
            } else
            {
                Console.WriteLine("Quiz is empty");
            }

            

        }
    }
}
