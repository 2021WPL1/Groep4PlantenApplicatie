using PlantenApplicatie.Domain;
using PlantenApplicatie.viewmodels;
using System.Windows;
using System.Windows.Input;

namespace PlantenApplicatie
{
    /// <summary>
    /// Interaction logic for WijzigWachtwoord.xaml
    /// </summary>
    public partial class WijzigWachtwoord : Window
    {
        public WijzigWachtwoord(Gebruiker gebruiker)
        {
            InitializeComponent();
            DataContext = new EditPasswordViewModel(this, gebruiker);
        }

        private void PasswordChanged(object sender, KeyEventArgs e)
        {
            if (DataContext is null) return;

            var dynamicDataContext = (dynamic)DataContext;

            dynamicDataContext.PasswordChecker(pwbNewPassword.Password, 
                pwbNewPasswordConfirm.Password);
        }
    }
}
