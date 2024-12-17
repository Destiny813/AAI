using System;

namespace TaskManager.Models
{
    public class TaskItem
    {
        public string Title { get; set; }
        public PriorityLevel Priority { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
    }
    public enum PriorityLevel
    {
        High,
        Medium,
        Low // Убедитесь, что это значение присутствует в перечислении
    }


}
