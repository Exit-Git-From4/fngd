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
    internal class MainMvvm : BaseVM
    {
        private Book selectedBook;
        private ObservableCollection<Book> books = new();
        private ObservableCollection<Author> authors = new();
        private string search;


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
        public CommandMvvm UpdateBook { get; set; }
        public CommandMvvm RemoveBook { get; set; }
        public CommandMvvm AddBook { get; set; }
        public CommandMvvm OpenListAuthor { get; set; }
        public CommandMvvm Update { get; set; }
        public string Search
        {
            get => search;
            set
            {
                search = value;
                SearchBook(search);
            }
        }

        public MainMvvm()
        {
            SelectAll(); 
            UpdateBook = new CommandMvvm(() =>
            { 
                Book book = SelectedBook;
                new WindowAddEditBook(book).ShowDialog();
                SelectAll();
            }, () => SelectedBook != null);

            RemoveBook = new CommandMvvm(() =>
            {
                if(MessageBox.Show("ТОЧНО?????","Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    DBBook.GetDb().Remove(SelectedBook);
                SelectAll();
            }, () => SelectedBook != null);

            AddBook = new CommandMvvm(() =>
            {
                Book book = new Book();
                new WindowAddEditBook(book).ShowDialog();
                SelectAll();
            }, () => true);

            OpenListAuthor = new CommandMvvm(() => 
            {
                new WindowListAuthor().ShowDialog();
                SelectAll();
            }, () => true);

            Update = new CommandMvvm(SelectAll, () => true);
        }

        private void SelectAll()
        {
            Books = new ObservableCollection<Book>(DBBook.GetDb().SelectAll());
        }
        private void SearchBook(string search)
        {
            Books = new ObservableCollection<Book>(DBBook.GetDb().SearchBook(search));
        }
    }
}

