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
                        DateOnly birthday = new DateOnly();
                        if (!dr.IsDBNull(9))
                            birthday = dr.GetDateOnly("Birthday");

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
