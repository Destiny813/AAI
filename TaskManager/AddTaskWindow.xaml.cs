// AddTaskWindow.xaml.cs
using System;
using System.Windows;
using System.Windows.Controls;
using TaskManager.Models;

namespace TaskManager
{
    public partial class AddTaskWindow : Window
    {
        public TaskItem NewTask { get; private set; }

        public AddTaskWindow()
        {
            InitializeComponent();
            TitleTextBox.Text = "Task Title"; // Плейсхолдер
            TitleTextBox.Foreground = System.Windows.Media.Brushes.Gray;

            // Задаем начальные значения для ComboBox и TextBox
            PriorityComboBox.SelectedIndex = 0; // Устанавливаем Low по умолчанию
            DescriptionTextBox.Text = "Description (optional)";
            DescriptionTextBox.Foreground = System.Windows.Media.Brushes.Gray;
        }

        private void TitleTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TitleTextBox.Text == "Task Title")
            {
                TitleTextBox.Text = "";
                TitleTextBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void TitleTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                TitleTextBox.Text = "Task Title";
                TitleTextBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private void DescriptionTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DescriptionTextBox.Text == "Description (optional)")
            {
                DescriptionTextBox.Text = "";
                DescriptionTextBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void DescriptionTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                DescriptionTextBox.Text = "Description (optional)";
                DescriptionTextBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TitleTextBox.Text == "Task Title" || PriorityComboBox.SelectedItem == null || DueDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Получаем выбранный приоритет
            string selectedPriority = (PriorityComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            PriorityLevel priority = (PriorityLevel)Enum.Parse(typeof(PriorityLevel), selectedPriority);

            NewTask = new TaskItem
            {
                Title = TitleTextBox.Text,
                Priority = priority,
                Description = DescriptionTextBox.Text != "Description (optional)" ? DescriptionTextBox.Text : string.Empty,
                DueDate = DueDatePicker.SelectedDate.Value,
                IsCompleted = false
            };

            DialogResult = true;
            Close();
        }
    }
}
