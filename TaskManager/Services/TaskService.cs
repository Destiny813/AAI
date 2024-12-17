using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using TaskManager.Models;

namespace TaskManager.Services
{
    public class TaskService
    {
        private readonly string _filePath = "tasks.json";

        public void SaveTasks(List<TaskItem> tasks)
        {
            var json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        public List<TaskItem> LoadTasks()
        {
            if (!File.Exists(_filePath))
            {
                return new List<TaskItem>();
            }
            var json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<TaskItem>>(json);
        }
    }
}
