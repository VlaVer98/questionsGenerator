using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class Subject
    {
        public string Name { get; set; }
        public List<String> Questions { get; set; } = new List<string>();
    }
}
