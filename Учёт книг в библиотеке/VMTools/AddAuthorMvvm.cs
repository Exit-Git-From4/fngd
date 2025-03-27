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
        public CommandMvvm Save { get; set; }

        public AddAuthorMvvm()
        {
            SelectAll();
            Save = new CommandMvvm(() =>
            {
                if (SelectedAuthor.Id > 0)
                    DBAuthor.GetDb().Update(SelectedAuthor);
                else
                    DBAuthor.GetDb().Insert(SelectedAuthor);
                close();
            }, () =>
            SelectedAuthor != null &&
            !string.IsNullOrWhiteSpace(SelectedAuthor.FirstName) &&
            !string.IsNullOrWhiteSpace(SelectedAuthor.LastName) &&
            !string.IsNullOrWhiteSpace(SelectedAuthor.Patronymic)
            );
        }

        private void SelectAll()
        {
            Authors = new ObservableCollection<Author>(DBAuthor.GetDb().SelectAll());
        }
        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
        internal void SetAuthor(Author author)
        {
            SelectedAuthor = author;
        }
    }
}
