using PlantenApplicatie.viewmodels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PlantenApplicatie
{
    /// <summary>
    /// Interaction logic for Wachtwoord_vergeten.xaml
    /// </summary>
    public partial class Wachtwoord_vergeten : Window
    {
        public Wachtwoord_vergeten()
        {
            InitializeComponent();
            this.DataContext = new WachtwoordVergetenViewModel(this);
        }
    }
}
