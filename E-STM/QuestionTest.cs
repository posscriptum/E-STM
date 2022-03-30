using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_STM
{
    public class QuestionTest
    {
        public string name { get; set; }
        public int id { get; set; }
        public int time;
        public string nameTask;

        public QuestionTest(int id, string name, int time)
        {
            this.name = name;
            this.id = id;
            this.time = time;
        }
    }
}
