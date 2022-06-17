﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chat
{
    /// <summary>
    /// Логика взаимодействия для Message.xaml
    /// </summary>
    public partial class Messagectrl : UserControl
    {
        public string Msg { get; set; }
        public string UserName { get; set; }
        public DateTime Time { get; set;}
        public Messagectrl()
        {
            InitializeComponent();
            DataContext = this;
           
        }
    }
}
