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
    public class AddEditBookMvvm : BaseVM
    {
        private Book selectedBook;
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
        public Book SelectedBook
        {
            get => selectedBook;
            set
            {
                selectedBook = value;
                Signal();
            }
        }
        public CommandMvvm Save { get; set; }

        Action close;
        public AddEditBookMvvm(Action close)
        {
            SelectAll();

            Save = new CommandMvvm(() =>
            {
                this.close = close;
            }, () => SelectedBook != null);
        }

        private void SelectAll()
        {
            Books = new ObservableCollection<Book>(DBBook.GetDb().SelectAll());
        }

    }
}