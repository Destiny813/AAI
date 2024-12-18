using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace PasswordGenerator
{
    public class PasswordEntry
    {
        public string Password { get; set; }
        public string Note { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public partial class MainWindow : Window
    {
        public const string HistoryFilePath = "password_history.json"; // Сделать его публичным
        public List<PasswordEntry> PasswordHistory { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            LoadHistory();
        }

        private void GeneratePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(PasswordLengthTextBox.Text, out int length) && length > 0)
            {
                GeneratedPasswordTextBox.Text = GeneratePassword(length);
                NoteTextBox.Clear(); // очищаем заметку при каждом новом пароле
            }
            else
            {
                MessageBox.Show("Введите корректную длину пароля.");
            }
        }

        private string GeneratePassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_-+=<>?";
            StringBuilder password = new StringBuilder();
            Random rnd = new Random();

            for (int i = 0; i < length; i++)
            {
                password.Append(validChars[rnd.Next(validChars.Length)]);
            }

            return password.ToString();
        }

        private void CopyToClipboardButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(GeneratedPasswordTextBox.Text);
            MessageBox.Show("Пароль скопирован в буфер обмена.");
        }

        private void SaveToHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            string password = GeneratedPasswordTextBox.Text;
            string note = NoteTextBox.Text;

            if (!string.IsNullOrEmpty(password))
            {
                var entry = new PasswordEntry
                {
                    Password = password,
                    Note = note,
                    DateCreated = DateTime.Now
                };

                PasswordHistory.Add(entry);
                SaveHistory();
                NoteTextBox.Clear(); // очищаем заметку после добавления в историю
            }
            else
            {
                MessageBox.Show("Сначала сгенерируйте пароль.");
            }
        }

        private void LoadHistory()
        {
            if (File.Exists(HistoryFilePath))
            {
                var json = File.ReadAllText(HistoryFilePath);
                PasswordHistory = JsonConvert.DeserializeObject<List<PasswordEntry>>(json) ?? new List<PasswordEntry>();
            }
            else
            {
                PasswordHistory = new List<PasswordEntry>();
            }
        }

        private void SaveHistory()
        {
            var json = JsonConvert.SerializeObject(PasswordHistory, Formatting.Indented);
            File.WriteAllText(HistoryFilePath, json);
        }

        private void ShowHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            PasswordHistoryWindow historyWindow = new PasswordHistoryWindow(this);
            historyWindow.Show();
        }
    }
}
