using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager
{
    public partial class MainWindow : Window
    {
        private List<TaskItem> _tasks;
        private TaskService _taskService;

        public MainWindow()
        {
            InitializeComponent();
            _taskService = new TaskService();
            LoadTasks();
        }

        private void LoadTasks()
        {
            _tasks = _taskService.LoadTasks() ?? new List<TaskItem>(); // Инициализация если LoadTasks возвращает null
            TaskListView.ItemsSource = _tasks;
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var addTaskWindow = new AddTaskWindow();
            if (addTaskWindow.ShowDialog() == true)
            {
                var newTask = addTaskWindow.NewTask;
                if (_tasks != null)
                {
                    _tasks.Add(newTask);
                }
                else
                {
                    _tasks = new List<TaskItem> { newTask };
                }

                _taskService.SaveTasks(_tasks);
                LoadTasks();
            }
        }

        private void FilterComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string selectedFilter = ((ComboBoxItem)FilterComboBox.SelectedItem).Content.ToString();
            if (_tasks == null) return; // Проверка на null перед использованием

            if (selectedFilter == "Completed")
            {
                TaskListView.ItemsSource = _tasks.Where(t => t.IsCompleted).ToList();
            }
            else if (selectedFilter == "Not Completed")
            {
                TaskListView.ItemsSource = _tasks.Where(t => !t.IsCompleted).ToList();
            }
            else
            {
                TaskListView.ItemsSource = _tasks;
            }
        }

        private void SortByDateButton_Click(object sender, RoutedEventArgs e)
        {
            if (_tasks != null)
            {
                TaskListView.ItemsSource = _tasks.OrderBy(t => t.DueDate).ToList();
            }
        }

        private void TaskListView_ColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            var column = e.OriginalSource as GridViewColumnHeader;
            if (column != null)
            {
                var propertyName = column.Column.Header.ToString();
                IEnumerable<TaskItem> sortedTasks;

                if (propertyName == "Priority")
                {
                    sortedTasks = _tasks.OrderBy(t => t.Priority).ToList(); // сортировка по приоритету
                }
                else if (propertyName == "Completed")
                {
                    sortedTasks = _tasks.OrderBy(t => t.IsCompleted).ToList(); // сортировка по завершенности
                }
                else
                {
                    return;
                }

                TaskListView.ItemsSource = sortedTasks;
            }
        }

        private void TaskListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TaskListView.SelectedItem is TaskItem selectedTask)
            {
                // Используем Expander для отображения описания
                Expander expander = new Expander
                {
                    Header = selectedTask.Title,
                    Content = new TextBlock
                    {
                        Text = selectedTask.Description,
                        TextWrapping = TextWrapping.Wrap
                    }
                };

                MessageBox.Show(expander.Header.ToString(), "Task Description", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SortByPriorityButton_Click(object sender, RoutedEventArgs e)
        {
            if (_tasks != null)
            {
                TaskListView.ItemsSource = _tasks.OrderBy(t => t.Priority).ToList();
            }
        }
    }
}
