using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    [Serializable]
    public class Classroom
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string Subject { get; set; }

        public List<Student> Students { get; set; }

        private Classroom()
        {

        }

        public Classroom(string name, string subject)
        {
            this.ID = Guid.NewGuid().ToString();
            Name = name;
            Subject = subject;
            
        }
    }
}
