using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Учёт_книг_в_библиотеке.Model
{
    public class DBBook
    {
        DBConnection connection;

        private DBBook(DBConnection db)
        {
            this.connection = db;
        }
        public List<Book> SearchBook(string search)
        {
            List<Book> result = new();
            List<Author> authors = new();

            string query = $"SELECT Book.Id AS 'bookid', Title, YearPublished, Genre, IsAvailable, AuthorId, a.LastName, a.FirstName, a.Patronymic, a.Birthday FROM Book JOIN Author a ON Book.AuthorID = a.Id WHERE Title LIKE @search OR a.LastName LIKE @search";

            if (connection.OpenConnection())
            {// using уничтожает объект после окончания блока (вызывает Dispose)
                using (var mc = connection.CreateCommand(query))
                {
                    // передача поиска через переменную в запрос
                    mc.Parameters.Add(new MySqlParameter("search", $"%{search}%"));
                    using (var dr = mc.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            // создание книги на каждую строку в результате
                            var book = new Book();
                            book.Id = dr.GetInt32("bookid");
                            book.YearPublished = dr.GetInt32("YearPublished");
                            book.Title = dr.GetString("Title");
                            book.AuthorId = dr.GetInt32("AuthorID");
                            book.Genre = dr.GetString("Genre");
                            book.IsAvailable = dr.GetBoolean("IsAvailable");

                            // поиск объекта-автора в коллекции authors по ID
                            var author = authors.FirstOrDefault(s => s.Id == book.AuthorId);
                            if (author == null)
                            {
                                // создание автора, если его не было в коллекции
                                author = new Author();
                                author.Id = book.AuthorId;
                                author.FirstName = dr.GetString("FirstName");
                                author.LastName = dr.GetString("LastName");
                                author.Patronymic = dr.GetString("Patronymic");
                                author.Birthday = dr.GetDateTime("Birthday");
                                // добавление автора в коллекцию
                                authors.Add(author);
                            }
                            // добавление книги автору
                            //author.Books.Add(book);
                            // указание книге автора
                            book.Author = author;

                            // добавление книги в результат запроса
                            result.Add(book);
                        }
                    }
                }
                connection.CloseConnection();
            }
            return result;

        }

        public bool Insert(Book supplier)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Book` Values (0, @Title, @AuthorId, @YearPublished, @Genre ,@IsAvailable);select LAST_INSERT_ID();");
                cmd.Parameters.Add(new MySqlParameter("Title", supplier.Title));
                cmd.Parameters.Add(new MySqlParameter("AuthorId", supplier.AuthorId));
                cmd.Parameters.Add(new MySqlParameter("YearPublished", supplier.YearPublished));
                cmd.Parameters.Add(new MySqlParameter("Genre", supplier.Genre));
                cmd.Parameters.Add(new MySqlParameter("IsAvailable", supplier.IsAvailable));
                try
                {
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        supplier.Id = id;
                        result = true;
                    }
                    else
                    {
                        MessageBox.Show("Запись не добавлена");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }

        internal List<Book> SelectAll()
        {
            List<Book> book = new List<Book>();
            if (connection == null)
                return book;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("SELECT b.`Id`, `Title`, `AuthorId`, `Genre`, `IsAvailable`, `YearPublished`, a.`FirstName`, a.`LastName`, a.`Patronymic`, a.`Birthday` FROM Book b JOIN Author a ON b.AuthorId = a.Id");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string title = string.Empty;
                        if (!dr.IsDBNull(1))
                            title = dr.GetString("Title");
                        int authorID = dr.GetInt32("AuthorId");
                        string genre = string.Empty;
                        if (!dr.IsDBNull(4))
                            genre = dr.GetString("Genre");
                        bool isAvailable = true;
                        if (!dr.IsDBNull(5))
                            isAvailable = dr.GetBoolean("IsAvailable");
                        int yearPublished = 0;
                        if (!dr.IsDBNull(3))
                            yearPublished = dr.GetInt16("YearPublished");

                        string firstName = string.Empty;
                        if (!dr.IsDBNull(6))
                            firstName = dr.GetString("FirstName");
                        string lastName = string.Empty;
                        if (!dr.IsDBNull(8))
                            lastName = dr.GetString("LastName");
                        string ptronymic = string.Empty;
                        if (!dr.IsDBNull(7))
                            ptronymic = dr.GetString("Patronymic");
                        DateTime birthday = new DateTime();
                        if (!dr.IsDBNull(9))
                            birthday = dr.GetDateTime("Birthday");

                        Author author = new Author()
                        {
                            Id = authorID,
                            FirstName = firstName,
                            Patronymic = ptronymic,
                            LastName = lastName,
                            Birthday = birthday
                        };
                        book.Add(new Book
                        {
                            Id = id,
                            Title = title,
                            AuthorId = authorID,
                            Author = author,
                            Genre = genre,
                            IsAvailable= isAvailable,
                            YearPublished= yearPublished
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return book;
        }

        internal bool Update(Book edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `Book` set `Title`=@Title, `AuthorId`=@AuthorId, `Genre`=@Genre, `IsAvailable`=@IsAvailable, `YearPublished`=@YearPublished where `id` = {edit.Id}");
                mc.Parameters.Add(new MySqlParameter("Title", edit.Title));
                mc.Parameters.Add(new MySqlParameter("AuthorId", edit.AuthorId));
                mc.Parameters.Add(new MySqlParameter("Genre", edit.Genre));
                mc.Parameters.Add(new MySqlParameter("IsAvailable", edit.IsAvailable));
                mc.Parameters.Add(new MySqlParameter("YearPublished", edit.YearPublished));

                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }


        internal bool Remove(Book remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `Book` where `id` = {remove.Id}");
                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }

        static DBBook db;
        public static DBBook GetDb()
        {
            if (db == null)
                db = new DBBook(DBConnection.GetDbConnection());
            return db;
        }
    }

}
//ggg
