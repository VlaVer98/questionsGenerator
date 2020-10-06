using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Repository;

namespace questionsGenerator
{
    public partial class AddForm : Form
    {
        private readonly SubjectRepository subjectRepository; 
        public AddForm()
        {
            InitializeComponent();
            subjectRepository = new SubjectRepository($"{Environment.CurrentDirectory}//Questions.json");
            subjectRepository.ReadWithFile();
            listBoxSubject.DataSource = subjectRepository.Subjects.Select(u => u.Name).ToList<String>();
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxQuestion.DataSource = subjectRepository.Subjects[listBoxSubject.SelectedIndex].Questions;
        }

        private void ButtonDelQuestion_Click(object sender, EventArgs e)
        {
            if(listBoxQuestion.SelectedIndex != -1)
            {
                subjectRepository.Subjects[listBoxSubject.SelectedIndex].Questions.RemoveAt(listBoxQuestion.SelectedIndex);
                listBoxQuestion.DataSource = null;
                listBoxQuestion.DataSource = subjectRepository.Subjects[listBoxSubject.SelectedIndex].Questions;
                subjectRepository.SaveToFile();
            }
            else
            {
                MessageBox.Show("Выберите вопрос.");
            }
        }

        private void ButtonAddQuestion_Click(object sender, EventArgs e)
        {
            if(listBoxSubject.SelectedIndex != -1)
            {
                if(richTextBoxQuestion.Text != "")
                {
                    subjectRepository.Subjects[listBoxSubject.SelectedIndex].Questions.Add(richTextBoxQuestion.Text);
                    listBoxQuestion.DataSource = null;
                    listBoxQuestion.DataSource = subjectRepository.Subjects[listBoxSubject.SelectedIndex].Questions;
                    richTextBoxQuestion.Text = "";
                    subjectRepository.SaveToFile();
                }
                else
                {
                    MessageBox.Show("Введите вопрос.");
                }
            }
            else
            {
                MessageBox.Show("Выберите тему.");
            }
        }

        private void ButtonAddSubject_Click(object sender, EventArgs e)
        {
            if(textBoxSubject.Text != "")
            {
                subjectRepository.Subjects.Add(new Subject() { Name = textBoxSubject.Text});
                listBoxSubject.DataSource = subjectRepository.Subjects.Select(u => u.Name).ToList<String>();
                subjectRepository.SaveToFile();
                textBoxSubject.Text = "";
            }
            else
            {
                MessageBox.Show("Введите название темы.");
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if(listBoxSubject.SelectedIndex != -1)
            {
                subjectRepository.Subjects.RemoveAt(listBoxSubject.SelectedIndex);
                listBoxSubject.DataSource = subjectRepository.Subjects.Select(u => u.Name).ToList<String>();
                if (listBoxSubject.Items.Count == 0)
                    listBoxQuestion.DataSource = null;
                subjectRepository.SaveToFile();
            }
            else
            {
                MessageBox.Show("Выберите тему.");
            }
        }
    }
}
