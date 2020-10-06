using iTextSharp.text;
using iTextSharp.text.pdf;
using Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace questionsGenerator
{
    public partial class GenerateForm : Form
    {
        private readonly SubjectRepository subjectRepository;
        private readonly Random rnd = new Random();
        public GenerateForm()
        {
            InitializeComponent();
            subjectRepository = new SubjectRepository($"{Environment.CurrentDirectory}//Questions.json");
            subjectRepository.ReadWithFile();
            checkedListBoxSubject.Items.AddRange(subjectRepository.Subjects.Select(n => n.Name).ToArray<String>());
        }

        private void CheckedListBoxSubject_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if(e.NewValue == CheckState.Checked)
            {
                Label label = new Label
                {
                    Name = $"labelSubject{e.Index}",
                    Text = $"{checkedListBoxSubject.Items[e.Index]}"
                };
                this.flowLayoutPanel1.Controls.Add(label);

                NumericUpDown numeric = new NumericUpDown
                {
                    Name = $"NumericUpDownQuestion{e.Index}",
                    Minimum = 0,
                    Maximum = subjectRepository.Subjects[e.Index].Questions.Count()
                };
                this.flowLayoutPanel1.Controls.Add(numeric);
            }
            else
            {
                this.flowLayoutPanel1.Controls.RemoveByKey($"labelSubject{e.Index}");
                this.flowLayoutPanel1.Controls.RemoveByKey($"NumericUpDownQuestion{e.Index}");
            }
        }

        private void ButtonSelectPath_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBoxPath.Text = folderBrowserDialog1.SelectedPath;
        }

        private void ButtonGenerate_Click(object sender, EventArgs e)
        {
            
            if(textBoxPath.Text == "")
            {
                MessageBox.Show("Выберите путь");
                return;
            }
            if (!Directory.Exists(textBoxPath.Text))
            {
                MessageBox.Show("Некорректный путь.");
                return;
            }
            buttonGenerate.Enabled = false;
            progressBar1.Maximum = (int)numericUpDownCountFile.Value;
            progressBar1.Value = progressBar1.Minimum;
            labelPercent.Text = "0%";
            for (int i = 0; i < numericUpDownCountFile.Value; i++)
            {
                string textFile = $"Вариант-{i+1}\n\n";
                foreach (int item in checkedListBoxSubject.CheckedIndices)
                {
                    int total = (int)((NumericUpDown)this.flowLayoutPanel1.Controls[$"NumericUpDownQuestion{item}"]).Value;
                    List<string> questions = subjectRepository.Subjects[item].Questions.OrderBy(x => rnd.Next()).ToList();
                    int numDel = questions.Count() - total;
                    for (int j = 1; j <= numDel; j++)
                    {
                        questions.RemoveAt(rnd.Next(0, questions.Count()));
                    }
                    int numQuestion = 1;
                    foreach (var item2 in questions)
                    {
                        textFile += $"{numQuestion}. {item2}\n";
                        numQuestion++;
                    }
                }
                GeneratePdf($"{textBoxPath.Text}//{i + 1}.pdf", textFile);
                progressBar1.Value++;
            }
            labelPercent.Text = "100%";
            buttonGenerate.Enabled = true;
            Process.Start(textBoxPath.Text);
        }
        private void GeneratePdf(string fullName, string text)
        {
            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            using (PdfWriter.GetInstance(doc, new FileStream(fullName, FileMode.Create)))
            {
                doc.Open();
                BaseFont baseFont = BaseFont.CreateFont("c:/Windows/Fonts/arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 18);
                Paragraph paragraph = new Paragraph(text, font);
                doc.Add(paragraph);
                doc.Close();
            }
        }
    }
}
