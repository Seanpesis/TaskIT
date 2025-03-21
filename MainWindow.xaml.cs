using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TaskItClient.Data;
using TaskItClient.Models;
using MyTaskStatus = TaskItClient.Models.TaskStatus;

namespace TaskItClient
{
    public partial class MainWindow : Window
    {
        private AppDbContext _dbContext = new AppDbContext();
        private TaskItem _currentTask; 

        public MainWindow()
        {
            InitializeComponent();
            LoadTasks();
            _dbContext.Database.Migrate();
            _currentTask = new TaskItem();
        }

        private void LoadTasks()
        {
            var allTasks = _dbContext.Tasks.ToList();

            var active = allTasks.Where(t => t.Status != MyTaskStatus.Completed).ToList();
            var completed = allTasks.Where(t => t.Status == MyTaskStatus.Completed).ToList();

            ActiveTasksListBox.ItemsSource = active;
            CompletedTasksListBox.ItemsSource = completed;
        }

        private void SaveTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentTask == null)
            {
                _currentTask = new TaskItem
                {
                    CreatedAt = DateTime.Now
                };
                _dbContext.Tasks.Add(_currentTask);
            }

            _currentTask.Title = TaskTitleTextBox.Text;
            _currentTask.Description = TaskDescriptionTextBox.Text;
            _currentTask.DueDate = TaskDueDatePicker.SelectedDate;
            _currentTask.Status = (MyTaskStatus)Enum.Parse(typeof(MyTaskStatus),
                (TaskStatusComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content.ToString()
                ?? "NotStarted");

            var statusString = (TaskStatusComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            if (!string.IsNullOrEmpty(statusString))
            {
                _currentTask.Status = (MyTaskStatus)Enum.Parse(typeof(MyTaskStatus), statusString);
            }

            _currentTask.CreatedAt = DateTime.Now;

            if (_currentTask.Status == MyTaskStatus.Completed && _currentTask.CompletedAt == null)
            {
                _currentTask.CompletedAt = DateTime.Now;
            }
            else if (_currentTask.Status != MyTaskStatus.Completed)
            {
                _currentTask.CompletedAt = null;
            }
            _dbContext.SaveChanges();
            LoadTasks();
        }

        private void NewTaskButton_Click(object sender, RoutedEventArgs e)
        {
            _currentTask = null;
            TaskTitleTextBox.Text = "";
            TaskDescriptionTextBox.Text = "";
            TaskDueDatePicker.SelectedDate = null;
            TaskStatusComboBox.SelectedIndex = 0; 
        }
        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentTask != null)
            {
                _dbContext.Tasks.Remove(_currentTask);
                _dbContext.SaveChanges();

                _currentTask = null;

                LoadTasks();

            }
        }

        private void ActiveTasksListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox == null) return;

            var selectedTask = ActiveTasksListBox.SelectedItem as TaskItem;
            if (selectedTask != null)
            {
                _currentTask = selectedTask;
                FillTaskDetails(_currentTask);
                TaskTitleTextBox.Text = _currentTask.Title ?? "";
                TaskDescriptionTextBox.Text = _currentTask.Description ?? "";
                TaskDueDatePicker.SelectedDate = _currentTask.DueDate;
            }

            TaskStatusComboBox.SelectedIndex = (int)_currentTask.Status;

        }

        private void CompletedTasksListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedTask = CompletedTasksListBox.SelectedItem as TaskItem;
            if (selectedTask != null)
            {
                _currentTask = selectedTask;
                FillTaskDetails(_currentTask);
            }
        }

        private void FillTaskDetails(TaskItem task)
        {
            TaskTitleTextBox.Text = task.Title ?? "";
            TaskDescriptionTextBox.Text = task.Description;
            TaskDueDatePicker.SelectedDate = task.DueDate;
            TaskStatusComboBox.SelectedIndex = (int)task.Status;
        }
    }
}
