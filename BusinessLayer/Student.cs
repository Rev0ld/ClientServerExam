using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    [Serializable]
    public class Student
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public byte Age { get; set; }

        public List<int> Points { get; set; }

        public List<Classroom> Classrooms { get; set; }

        private Student()
        {
        }

        public Student(string name, byte age, List<int> points)
        {
            Name = name;
            Age = age;
            Points = points;
        }
    }
}
