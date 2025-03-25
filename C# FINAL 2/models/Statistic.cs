using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C__FINAL_2.models
{
    internal class Statistic
    {
        public string UserLogin { get; set; }
        public string QuizTitle { get; set; }
        public int Score { get; set; }
        public Dictionary<string, string> QuestionsAndUserAnswers {  get; set; }

    }
}
