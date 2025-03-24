using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Учёт_книг_в_библиотеке.Model
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public DateOnly Birthday {  get; set; }     
    }
}
