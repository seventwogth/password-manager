using System;

namespace password_manager
{
  public class DbException : Exception
  {
    public DbException() { }

    public DbException(string message) : base(message) { }

    public DbException(string message, Exception inner) : base(message, inner) { }
  }
}

//кастомный класс для обработки исключений, связанных с работой с БД
