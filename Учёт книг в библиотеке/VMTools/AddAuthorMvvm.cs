using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Учёт_книг_в_библиотеке.Model;
using Учёт_книг_в_библиотеке.View;

namespace Учёт_книг_в_библиотеке.VMTools
{
    internal class AddAuthorMvvm : BaseVM
    {
        private Author selectedAuthor;
        private ObservableCollection<Book> books = new();
        private ObservableCollection<Author> authors = new();

        private ObservableCollection<Author> Authors { get => authors; set { authors = value; Signal(); } }
        public ObservableCollection<Book> Books
        {
            get => books;
            set
            {
                books = value;
                Signal();
            }
        }
        public Author SelectedAuthor
        {
            get => selectedAuthor;
            set
            {
                selectedAuthor = value;
                Signal();
            }
        }
        public CommandMvvm UpdateAuthor { get; set; }
        public CommandMvvm RemoveAuthor { get; set; }
        public CommandMvvm Add { get; set; }

        public AddAuthorMvvm()
        {
            SelectAll();

            UpdateAuthor = new CommandMvvm(() =>
            {
                if (DBAuthor.GetDb().Update(SelectedAuthor))
                    MessageBox.Show("Успешно");
            }, () => SelectedAuthor != null);

            RemoveAuthor = new CommandMvvm(() =>
            {
                DBAuthor.GetDb().Remove(SelectedAuthor);
                SelectAll();
            }, () => SelectedAuthor != null);

            //Add = new CommandMvvm(() =>
            //{
            //    new WindowAddEditBook().ShowDialog();
            //    SelectAll();
            //}, () => true);

        }

        private void SelectAll()
        {
            Authors = new ObservableCollection<Author>(DBAuthor.GetDb().SelectAll());
        }

    }
}
