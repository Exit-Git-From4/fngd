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
    internal class ListAuthorMvvm : BaseVM
    {
        private Author selectedAuthor;
        private ObservableCollection<Author> authors = new();


        public ObservableCollection<Author> Authors { get => authors; set { authors = value; Signal(); } }
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
        public CommandMvvm AddAuthor { get; set; }

        public ListAuthorMvvm()
        {
            SelectAll();

            UpdateAuthor = new CommandMvvm(() =>
            {
                Author author = SelectedAuthor;
                new WindowAddAuthor(author).ShowDialog();
                SelectAll();
            }, () => SelectedAuthor != null);

            RemoveAuthor = new CommandMvvm(() =>
            {
                if (IsNotInListBook())
                    DBAuthor.GetDb().Remove(SelectedAuthor);
                else
                {
                    MessageBox.Show("Существует книга, содержащая данного автора");
                }
                SelectAll();
            }, () => SelectedAuthor != null);

            AddAuthor = new CommandMvvm(() =>
            {
                Author author = new Author();
                new WindowAddAuthor(author).ShowDialog();
                SelectAll();
            }, () => true);
        }

        private void SelectAll()
        {
            Authors = new ObservableCollection<Author>(DBAuthor.GetDb().SelectAll());
        }
        public bool IsNotInListBook()
        {
            foreach (var book in DBBook.GetDb().SelectAll())
            {
                if (SelectedAuthor.Id == book.AuthorId)
                    return false;
            }
            return true;
        }
    }
}
