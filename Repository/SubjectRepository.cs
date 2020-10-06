using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class SubjectRepository
    {
        public string Path { get; private set; }
        public List<Subject> Subjects { get; set; } = new List<Subject>();
        public SubjectRepository(string path)
        {
            Path = path;
        }
        public void SaveToFile()
        {
            string data = JsonConvert.SerializeObject(Subjects);
            using (StreamWriter sw = new StreamWriter(Path, false, System.Text.Encoding.UTF8))
            {
                sw.WriteLine(data);
            }
        }
        public void ReadWithFile()
        {
            using (StreamReader sr = new StreamReader(Path, System.Text.Encoding.UTF8))
            {
                string str = sr.ReadToEnd();
                Subjects.AddRange(JsonConvert.DeserializeObject<List<Subject>>(str));
            }
        }
    }
}