using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace PraktikaHinded
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string CurrentUser = "";
        public MainWindow()
        {
            InitializeComponent();
            if (!File.Exists("School.xml"))
            {
                XmlDocument xmlDoc = new XmlDocument();
                //XmlDocument xmlDoc = new XmlDocument();
                XmlNode rootNode = xmlDoc.CreateElement("Data");
                XmlNode marksNode = xmlDoc.CreateElement("Marks");
                XmlNode notesNode = xmlDoc.CreateElement("Notes");
                XmlNode homeworksNode = xmlDoc.CreateElement("Homeworks");
                rootNode.AppendChild(marksNode);
                rootNode.AppendChild(notesNode);
                rootNode.AppendChild(homeworksNode);
                xmlDoc.AppendChild(rootNode);
                xmlDoc.Save("School.xml");
            }
        }

        // GOING IN

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (RegisterUserName.Text != "" && RegisterPassWord.Text != "")
            {
                if (RegisterTeacherOrStudent.SelectedIndex == 1)
                {
                    if (RegisterPassWordComfirm.Text == RegisterPassWord.Text)
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load("School.xml");
                        XmlNode rootNode = xmlDoc.FirstChild;
                        XmlNode studentNode = xmlDoc.CreateElement("Student");
                        XmlAttribute nameAttribute = xmlDoc.CreateAttribute("Name");
                        nameAttribute.Value = RegisterUserName.Text;
                        XmlAttribute passwordAttribute = xmlDoc.CreateAttribute("Pass");
                        passwordAttribute.Value = RegisterPassWord.Text;
                        studentNode.Attributes.Append(nameAttribute);
                        studentNode.Attributes.Append(passwordAttribute);
                        rootNode.AppendChild(studentNode);
                        xmlDoc.Save("School.xml");
                        MessageBox.Show("Registreeritud. Nüüd logi sisse.");
                        RegisterUserName.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Salasõnad ei ühtinud.");
                    }
                    RegisterPassWord.Clear();
                    RegisterPassWordComfirm.Clear();
                }
                if (RegisterTeacherOrStudent.SelectedIndex == 2)
                {
                    if (RegisterPassWordComfirm.Text == RegisterPassWord.Text)
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load("School.xml");
                        XmlNode rootNode = xmlDoc.FirstChild;
                        XmlNode teacherNode = xmlDoc.CreateElement("Teacher");
                        XmlAttribute nameAttribute = xmlDoc.CreateAttribute("Name");
                        nameAttribute.Value = RegisterUserName.Text;
                        XmlAttribute passwordAttribute = xmlDoc.CreateAttribute("Pass");
                        passwordAttribute.Value = RegisterPassWord.Text;
                        teacherNode.Attributes.Append(nameAttribute);
                        teacherNode.Attributes.Append(passwordAttribute);
                        rootNode.AppendChild(teacherNode);
                        xmlDoc.Save("School.xml");
                        MessageBox.Show("Registreeritud. Nüüd logi sisse.");

                        RegisterUserName.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Salasõnad ei ühtinud.");
                    }
                    RegisterPassWord.Clear();
                    RegisterPassWordComfirm.Clear();
                }
                if (RegisterTeacherOrStudent.SelectedIndex == 0)
                {
                    MessageBox.Show("Vali kas oled õpilane või õpetaja.");
                    RegisterPassWord.Clear();
                    RegisterPassWordComfirm.Clear();
                }
            }
            else
            {
                MessageBox.Show("Täida kõik väljad");
                RegisterPassWord.Clear();
                RegisterPassWordComfirm.Clear();
            }
        }

        private void LogInBtn_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("School.xml");
            XmlNodeList students = xmlDoc.SelectNodes("//Data/Student");
            XmlNodeList teachers = xmlDoc.SelectNodes("//Data/Teacher");
            //Check if its a student
            foreach (XmlNode personNode in students)
            {
                if (personNode.Attributes["Name"].Value == logInUserName.Text)
                {
                    if (personNode.Attributes["Pass"].Value == logInPassWord.Text)
                    {
                        LoginView.Visibility = Visibility.Collapsed;
                        StudentView.Visibility = Visibility.Visible;
                        StudentChoiceView.Visibility = Visibility.Visible;
                        CurrentUser = personNode.Attributes["Name"].Value;
                    }
                }
            }
            //Check if its a teacher
            if (StudentView.Visibility == Visibility.Collapsed)
            {
                foreach (XmlNode personNode in teachers)
                {
                    if (personNode.Attributes["Name"].Value == logInUserName.Text)
                    {
                        if (personNode.Attributes["Pass"].Value == logInPassWord.Text)
                        {
                            LoginView.Visibility = Visibility.Collapsed;
                            TeacherView.Visibility = Visibility.Visible;
                            TeacherChoiceView.Visibility = Visibility.Visible;
                            CurrentUser = personNode.Attributes["Name"].Value;
                        }
                    }
                }
            }
            if (StudentView.Visibility == Visibility.Collapsed && TeacherView.Visibility == Visibility.Collapsed)
            {
                MessageBox.Show("Kontrolli oma kasutajanime ja parooli");
                logInUserName.Clear();
                logInPassWord.Clear();
            }
        }

        // ---------------- //

        // TEACHER MENU

        private void TeacherMarkBtn_Click(object sender, RoutedEventArgs e)
        {
            TeacherChoiceView.Visibility = Visibility.Collapsed;
            TeacherMarksView.Visibility = Visibility.Visible;

            TeacherListOfStudentsToMark.Items.Clear();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("School.xml");
            XmlNodeList students = xmlDoc.SelectNodes("//Data/Student");
            foreach ( XmlNode student in students)
            {
                TeacherListOfStudentsToMark.Items.Add(student.Attributes["Name"].Value.ToString());
            }
        }

        private void TeacherNoteBtn_Click(object sender, RoutedEventArgs e)
        {
            TeacherChoiceView.Visibility = Visibility.Collapsed;
            TeacherNoteView.Visibility = Visibility.Visible;

            TeacherListOfStudentsToNote.Items.Clear();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("School.xml");
            XmlNodeList students = xmlDoc.SelectNodes("//Data/Student");
            foreach (XmlNode student in students)
            {
                TeacherListOfStudentsToNote.Items.Add(student.Attributes["Name"].Value.ToString());
            }
        }

        private void TeacherHomeWorkBtn_Click(object sender, RoutedEventArgs e)
        {
            TeacherChoiceView.Visibility = Visibility.Collapsed;
            TeacherHomeworksView.Visibility = Visibility.Visible;

            TeacherCurrentHomeworks.Items.Clear();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("School.xml");
            XmlNodeList homeworks = xmlDoc.SelectNodes("//Data/Homeworks/HomeWork");
            foreach (XmlNode homework in homeworks)
            {
                if (homework.Attributes["From"].Value == CurrentUser)
                {
                    TeacherCurrentHomeworks.Items.Add($"{homework.InnerText}\n");
                }
            }
        }

        // ---------------- //

        // TEACHER COMMITS //

        private void CommitMarkBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TeacherListOfStudentsToMark.SelectedItem != null && TeacherMarkBox.Text.Length == 1 && TeacherMarkExplanation.Text.Length > 3)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("School.xml");
                XmlNode marksNode = xmlDoc.SelectSingleNode("//Data/Marks");

                XmlNode mark = xmlDoc.CreateElement("Mark");
                XmlAttribute fromAttribute = xmlDoc.CreateAttribute("From");
                fromAttribute.Value = CurrentUser;
                XmlAttribute toAttribute = xmlDoc.CreateAttribute("To");
                toAttribute.Value = TeacherListOfStudentsToMark.SelectedItem.ToString();
                XmlAttribute gradeAttribute = xmlDoc.CreateAttribute("Grade");
                gradeAttribute.Value = TeacherMarkBox.Text;

                mark.Attributes.Append(fromAttribute);
                mark.Attributes.Append(toAttribute);
                mark.Attributes.Append(gradeAttribute);
                mark.InnerText = TeacherMarkExplanation.Text;
                marksNode.AppendChild(mark);

                xmlDoc.Save("School.xml");
                MessageBox.Show("Lisatud");
                TeacherListOfStudentsToMark.UnselectAll();
                TeacherMarkBox.Text = "";
                TeacherMarkExplanation.Text = "";
            }
            else
            {
                MessageBox.Show("1. vali õpilane \n2. anna hinne \n3. kirjuta selgitus");
                TeacherListOfStudentsToMark.UnselectAll();
                TeacherMarkBox.Text = "";
                TeacherMarkExplanation.Text = "";
            }
        }

        private void CommitNoteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TeacherListOfStudentsToNote.SelectedItem != null && TeacherNoteToStudent.Text != "")
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("School.xml");
                XmlNode notesNode = xmlDoc.SelectSingleNode("//Data/Notes");

                XmlNode noteNode = xmlDoc.CreateElement("Note");
                XmlAttribute fromAttribute = xmlDoc.CreateAttribute("From");
                XmlAttribute toAttribute = xmlDoc.CreateAttribute("To");
                fromAttribute.Value = CurrentUser;
                toAttribute.Value = TeacherListOfStudentsToNote.SelectedItem.ToString();
                noteNode.Attributes.Append(fromAttribute);
                noteNode.Attributes.Append(toAttribute);
                noteNode.InnerText = TeacherNoteToStudent.Text;
                notesNode.AppendChild(noteNode);

                xmlDoc.Save("School.xml");
                MessageBox.Show("Lisatud");
                TeacherListOfStudentsToNote.UnselectAll();
                TeacherNoteToStudent.Text = "";
            }
            else
            {
                MessageBox.Show("1. Vali õpilane \n2. Kirjuta märkus");
            }
        }

        private void CommitHomework_Click(object sender, RoutedEventArgs e)
        {
            if (TeacherNewHomework.Text != "")
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("School.xml");
                XmlNode homeWorksNode = xmlDoc.SelectSingleNode("//Data/Homeworks");

                XmlNode homeWork = xmlDoc.CreateElement("HomeWork");
                XmlAttribute fromAttribute = xmlDoc.CreateAttribute("From");
                fromAttribute.Value = CurrentUser;
                homeWork.Attributes.Append(fromAttribute);
                homeWork.InnerText = TeacherNewHomework.Text;
                homeWorksNode.AppendChild(homeWork);

                xmlDoc.Save("School.xml");
                MessageBox.Show("Lisatud");
                TeacherNewHomework.Text = "";
            }
            else
            {
                MessageBox.Show("Sa ei kirjutanud kodutööd");
            }
        }

        // --------------- //

        // STUDENT MENU //

        private void StudentMarkBtn_Click(object sender, RoutedEventArgs e)
        {
            StudentChoiceView.Visibility = Visibility.Collapsed;
            StudentMarksView.Visibility = Visibility.Visible;

            StudentsTeachersList.Items.Clear();
            StudentsMarksList.Items.Clear();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("School.xml");
            XmlNodeList teachers = xmlDoc.SelectNodes("//Data/Teacher");
            foreach (XmlNode teacher in teachers)
            {
                StudentsTeachersList.Items.Add(teacher.Attributes["Name"].Value.ToString());
            }
        }

        private void StudentNoteBtn_Click(object sender, RoutedEventArgs e)
        {
            StudentChoiceView.Visibility = Visibility.Collapsed;
            StudentNotesView.Visibility = Visibility.Visible;

            StudentsNotesList.Items.Clear();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("School.xml");
            XmlNodeList notes = xmlDoc.SelectNodes("//Data/Notes/Note");
            foreach (XmlNode note in notes)
            {
                if (note.Attributes["To"].Value == CurrentUser)
                {
                    StudentsNotesList.Items.Add($"{note.Attributes["From"].Value}: {note.InnerText}");
                }
            }
        }

        private void StudentHomeWorkBtn_Click(object sender, RoutedEventArgs e)
        {
            StudentChoiceView.Visibility = Visibility.Collapsed;
            StudentHomeworkView.Visibility = Visibility.Visible;

            StudentsHomeWorksList.Items.Clear();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("School.xml");
            XmlNodeList homeworks = xmlDoc.SelectNodes("//Data/Homeworks/HomeWork");
            foreach (XmlNode homework in homeworks)
            {
                StudentsHomeWorksList.Items.Add($"{homework.Attributes["From"].Value}: {homework.InnerText}");   
            }
        }

        // --------------- //

        // OTHER //


        private void StudentsTeachersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StudentsMarksList.Items.Clear();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("School.xml");
            XmlNodeList marksNodeList = xmlDoc.SelectNodes("//Data/Marks/Mark");
            foreach (XmlNode mark in marksNodeList)
            {
                if (StudentsTeachersList.SelectedItem != null)
                {
                    if (mark.Attributes["From"].Value == StudentsTeachersList.SelectedItem.ToString() && mark.Attributes["To"].Value == CurrentUser)
                    {
                        StudentsMarksList.Items.Add($"Hinne: {mark.Attributes["Grade"].Value}\n\nSELGITUS: {mark.InnerText}");
                    }
                }
            }
            if (!StudentsMarksList.HasItems)
            {
                StudentsMarksList.Items.Add("Selle õpetajaga hinded puuduvad.");
            }
        }

        private void StudentLogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            StudentView.Visibility = Visibility.Collapsed;
            LoginView.Visibility = Visibility.Visible;
            CurrentUser = "";
        }

        private void TeacherLogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            TeacherView.Visibility = Visibility.Collapsed;
            LoginView.Visibility = Visibility.Visible;
            CurrentUser = "";
        }

        private void StudentBackBtn_Click(object sender, RoutedEventArgs e)
        {
            StudentMarksView.Visibility = Visibility.Collapsed;
            StudentNotesView.Visibility = Visibility.Collapsed;
            StudentHomeworkView.Visibility = Visibility.Collapsed;
            StudentChoiceView.Visibility = Visibility.Visible;
        }

        private void TeacherBackBtn_Click(object sender, RoutedEventArgs e)
        {
            TeacherMarksView.Visibility = Visibility.Collapsed;
            TeacherNoteView.Visibility = Visibility.Collapsed;
            TeacherHomeworksView.Visibility = Visibility.Collapsed;
            TeacherChoiceView.Visibility = Visibility.Visible;
        }

        // --------------- //

    }
}
