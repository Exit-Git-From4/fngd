using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Учёт_книг_в_библиотеке.Model
{
    public class DBAuthor
{
        DBConnection connection;

        private DBAuthor(DBConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Author author)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Author` Values (0, @FirstName, @LastName, @Patronymic ,@Birthday);select LAST_INSERT_ID();");
                cmd.Parameters.Add(new MySqlParameter("FirstName", author.FirstName));
                cmd.Parameters.Add(new MySqlParameter("LastName", author.LastName));
                cmd.Parameters.Add(new MySqlParameter("Patronymic", author.Patronymic));
                cmd.Parameters.Add(new MySqlParameter("Birthday", author.Birthday));
                try
                {
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        author.Id = id;
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

        internal List<Author> SelectAll()
        {
            List<Author> author = new List<Author>();
            if (connection == null)
                return author;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("SELECT `Id`, `FirstName`, `LastName`, `Patronymic`, `Birthday` FROM Author");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);

                        string firstName = string.Empty;
                        if (!dr.IsDBNull(1))
                            firstName = dr.GetString("FirstName");
                        string lastName = string.Empty;
                        if (!dr.IsDBNull(2))
                            lastName = dr.GetString("LastName");
                        string ptronymic = string.Empty;
                        if (!dr.IsDBNull(3))
                            ptronymic = dr.GetString("Patronymic");
                        DateTime birthday = new DateTime();
                        if (!dr.IsDBNull(4))
                            birthday = dr.GetDateTime("Birthday");

                        author.Add(new Author()
                        {
                            Id = id,
                            FirstName = firstName,
                            Patronymic = ptronymic,
                            LastName = lastName,
                            Birthday = birthday
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return author;
        }

        internal bool Update(Author edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `Author` set `FirstName`=@FirstName, `LastName`=@LastName, `Patronymic`=@Patronymic, `Birthday`=@Birthday where `Id` = {edit.Id}");
                mc.Parameters.Add(new MySqlParameter("FirstName", edit.FirstName));
                mc.Parameters.Add(new MySqlParameter("LastName", edit.LastName));
                mc.Parameters.Add(new MySqlParameter("Patronymic", edit.Patronymic));
                mc.Parameters.Add(new MySqlParameter("Birthday", edit.Birthday));

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


        internal bool Remove(Author remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `Author` where `Id` = {remove.Id}");
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

        static DBAuthor db;
        public static DBAuthor GetDb()
        {
            if (db == null)
                db = new DBAuthor(DBConnection.GetDbConnection());
            return db;
        }
    }
}
