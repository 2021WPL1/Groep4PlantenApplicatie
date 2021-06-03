using PlantenApplicatie.Domain;
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
    /// Interaction logic for EditGebruiker.xaml
    /// </summary>
    public partial class EditGebruiker : Window
    {
        public EditGebruiker(Gebruiker gebruiker)
        {
            InitializeComponent();
            this.DataContext = new EditGebruikerViewModel(this, gebruiker);
        }

        private void PasswordChanged(object sender, KeyEventArgs e)
        {
            if (DataContext is null) return;

            var dynamicDataContext = ((dynamic)DataContext);

            dynamicDataContext.PasswordChecker(pwbPassword.Password, pwbPasswordConfirm.Password);
        }
    }
}
