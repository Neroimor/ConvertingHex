using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using Path = System.IO.Path;

namespace RecodingWord
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> allFonts; // Список всех шрифтов
        public MainWindow()
        {
            InitializeComponent();
            // Путь к папке "Format"
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Format");

            // Проверяем, существует ли папка
            if (Directory.Exists(folderPath))
            {
                // Получаем список всех файлов в папке
                string[] files = Directory.GetFiles(folderPath);

                // Добавляем файлы в ComboBox один за другим
                foreach (string file in files)
                {
                    timesShrift.Items.Add(Path.GetFileName(file)); // Только имя файла
                }
                timesShrift.SelectedIndex = 0;
            }

        }

        private void TextTans_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextTans.Text.Length > 255)
            {
                TextTans.Text = TextTans.Text.Substring(0, TextTans.Text.Length - 1);
            }

        }



        private void timesShrift_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Изменение шрифта в зависимости от выбранного элемента ComboBox
            if (timesShrift.SelectedItem != null)
            {
                string selectedFont = timesShrift.SelectedItem.ToString();
                TextTans.FontFamily = new FontFamily(selectedFont);
            }
        }


        private void prefix_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                // Проверяем, что текст состоит только из цифр
                if (!System.Text.RegularExpressions.Regex.IsMatch(textBox.Text, @"^\d*$"))
                {
                    // Убираем последний введенный символ, если это не цифра
                    textBox.Text = string.Concat(textBox.Text.Where(c => Char.IsDigit(c)));
                    // Устанавливаем курсор в конец текста
                    textBox.SelectionStart = textBox.Text.Length;
                }
            }
        }

        private void postfix_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                // Проверяем, что текст состоит только из цифр
                if (!System.Text.RegularExpressions.Regex.IsMatch(textBox.Text, @"^\d*$"))
                {
                    // Убираем последний введенный символ, если это не цифра
                    textBox.Text = string.Concat(textBox.Text.Where(c => Char.IsDigit(c)));
                    // Устанавливаем курсор в конец текста
                    textBox.SelectionStart = textBox.Text.Length;
                }
            }
        }


        private string getFilePath()
        {
            // Проверяем, выбран ли элемент в ComboBox
            if (timesShrift.SelectedItem != null)
            {
                // Получаем имя выбранного файла
                string selectedFileName = timesShrift.SelectedItem.ToString();

                // Путь к папке "Format"
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Format");

                // Возвращаем полный путь к выбранному файлу
                return Path.Combine(folderPath, selectedFileName);
            }
            return "";
        }

        private void transButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (prefix.Text == "")
                    prefix.Text = "0";
                if (postfix.Text == "")
                    postfix.Text = "0";
                string butPath = getFilePath();
                string selectedFont = timesShrift.SelectedItem.ToString();
                string command = new ConvertingHex().WordToHex
                    (int.Parse(prefix.Text.ToString()),
                    int.Parse(postfix.Text.ToString()),
                    TextTans.Text.ToString(),
                    butPath);

                string[] splByte = command.Split(' ');
                string name = "uint8_t phrase[] = {";
                foreach (string s in splByte)
                {
                    name += s+",";
                }
                name = name.Remove(name.Length - 1);
                name += "};";
                string nameFile = $"phrase";
                string format = "h";
                string path = GetSavePath(nameFile, format);
                SaveToFile(path, name);

                // Выводим результат
                Console.WriteLine(command);
            }
            catch (Exception)
            {

            }

        }

        // Метод для сохранения параметров в файл
        public void SaveToFile(string path, string content)
        {
            // Запись содержимого в файл
            File.WriteAllText(path, content);
        }
        // Метод для открытия диалога сохранения файла и получения пути
        private string GetSavePath(string name, string format)
        {
            // Создаем новый SaveFileDialog
            var saveFileDialog = new SaveFileDialog
            {
                Filter = $"{format.ToUpper()} files (*.{format})|*.{format}|All files (*.*)|*.*",
                Title = "Выберите путь для сохранения",
                FileName = name + "." + format  // Добавляем формат файла при сохранении
            };

            // Открываем диалог и получаем результат
            bool? result = saveFileDialog.ShowDialog();

            // Если результат положительный, возвращаем путь
            if (result == true)
            {
                return saveFileDialog.FileName;
            }

            // Если пользователь не выбрал путь, возвращаем null
            return null;
        }

    }
}
