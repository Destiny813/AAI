using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PasswordGenerator
{
    public partial class PasswordHistoryWindow : Window
    {
        private MainWindow mainWindow;

        public PasswordHistoryWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            LoadHistory();
        }

        private void LoadHistory()
        {
            HistoryListBox.Items.Clear();
            foreach (var entry in mainWindow.PasswordHistory)
            {
                HistoryListBox.Items.Add($"{entry.Password} - {entry.Note} ({entry.DateCreated})");
            }
        }

        private void CopyFromHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (HistoryListBox.SelectedItem != null)
            {
                string selectedItem = HistoryListBox.SelectedItem.ToString();
                string password = selectedItem.Split(new[] { " - " }, StringSplitOptions.None)[0];
                Clipboard.SetText(password);
                MessageBox.Show("Пароль скопирован из истории в буфер обмена.");
            }
            else
            {
                MessageBox.Show("Выберите пароль из истории для копирования.");
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string keyword = SearchTextBox.Text.ToLower();
            HistoryListBox.Items.Clear();

            foreach (var entry in mainWindow.PasswordHistory)
            {
                if (entry.Password.ToLower().Contains(keyword) || entry.Note.ToLower().Contains(keyword))
                {
                    HistoryListBox.Items.Add($"{entry.Password} - {entry.Note} ({entry.DateCreated})");
                }
            }

            if (HistoryListBox.Items.Count == 0)
            {
                MessageBox.Show("Пароли не найдены.");
            }
        }

        private void DeleteFromHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (HistoryListBox.SelectedItem != null)
            {
                string selectedItem = HistoryListBox.SelectedItem.ToString();
                string passwordToRemove = selectedItem.Split(new[] { " - " }, StringSplitOptions.None)[0];

                var entryToRemove = mainWindow.PasswordHistory.FirstOrDefault(entry => entry.Password == passwordToRemove);

                if (entryToRemove != null)
                {
                    mainWindow.PasswordHistory.Remove(entryToRemove);
                    SaveHistory(); // Обновляем историю в файле
                    LoadHistory(); // Перезагружаем отображение истории
                    MessageBox.Show("Пароль удален из истории.");
                }
            }
            else
            {
                MessageBox.Show("Выберите пароль из истории для удаления.");
            }
        }

        private void SaveHistory()
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(mainWindow.PasswordHistory, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(MainWindow.HistoryFilePath, json); // Это исправление здесь
        }
    }
}
