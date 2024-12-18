using System;

public class PasswordEntry
{
    public string UserId { get; set; } // Добавлено поле для ID пользователя
    public string Password { get; set; }
    public string Note { get; set; }
    public DateTime DateCreated { get; set; }
}
