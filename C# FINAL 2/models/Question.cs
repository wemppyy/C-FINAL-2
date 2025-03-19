using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C__FINAL_2.models
{
    internal class Question
    {
        public string Text { get; set; }
        public List<string> Answers { get; set; }
        public int CorrectAnswer { get; set; }
    }
}
