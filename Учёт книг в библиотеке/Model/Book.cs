using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Учёт_книг_в_библиотеке.Model
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public string Genre { get; set; }
        public bool IsAvailable { get; set; }
        public int YearPublished { get; set; }
    }
}
