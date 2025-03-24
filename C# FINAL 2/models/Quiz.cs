using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C__FINAL_2.models
{
    internal class Quiz
    {
        public string Title { get; set; }
        public List<Question> Questions { get; set; }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }
    }
}