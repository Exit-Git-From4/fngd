using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

        public ObservableCollection<Author> Authors { get => authors; set { authors = value; Signal(); } }
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

        public AddEditBookMvvm(Book book)
        {
            SelectedBook = book;
            SelectAll();
            Save = new CommandMvvm(() =>
            {
                SelectedBook.AuthorId = SelectedBook.Author.Id;
                if (SelectedBook.Id > 0)
                    DBBook.GetDb().Update(SelectedBook);
                else
                    DBBook.GetDb().Insert(SelectedBook);
                close();
            }, () => 
            !string.IsNullOrWhiteSpace(SelectedBook.Title) &&
            SelectedBook.Author != null &&
            !string.IsNullOrWhiteSpace(SelectedBook.Genre)
            );
        }

        private void SelectAll()
        {
            Authors = new ObservableCollection<Author>(DBAuthor.GetDb().SelectAll());
            if (SelectedBook.Author != null)
                SelectedBook.Author = Authors.FirstOrDefault(s => s.Id == SelectedBook.AuthorId);
        }
        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}