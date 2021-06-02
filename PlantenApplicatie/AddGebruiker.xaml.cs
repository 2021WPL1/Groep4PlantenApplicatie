using PlantenApplicatie.viewmodels;
using System.Windows;
using System.Windows.Input;

namespace PlantenApplicatie
{
    /// <summary>
    /// Interaction logic for AddGebruiker.xaml
    /// </summary>
    public partial class AddGebruiker : Window
    {
        public AddGebruiker()
        {
            InitializeComponent();
            DataContext = new AddGebruikerViewModel(this);
        }

        private void PasswordChanged(object sender, KeyEventArgs e)
        {
            if (DataContext is null) return;

            var dynamicDataContext = ((dynamic)DataContext);

            dynamicDataContext.PasswordChecker(pwbPassword.Password, pwbPasswordConfirm.Password);
        }
    }
}
