﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Учёт_книг_в_библиотеке.Model;
using Учёт_книг_в_библиотеке.VMTools;

namespace Учёт_книг_в_библиотеке.View
{
    /// <summary>
    /// Логика взаимодействия для WindowAddAuthor.xaml
    /// </summary>
    public partial class WindowAddAuthor : Window
    {
        public WindowAddAuthor(Author author)
        {
            InitializeComponent();
            ((AddAuthorMvvm)this.DataContext).SetAuthor(author);
            ((AddAuthorMvvm)this.DataContext).SetClose(Close);
        }
    }
}
