using PlantenApplicatie.Domain;
using PlantenApplicatie.viewmodels;
using System.Windows;

namespace PlantenApplicatie
{
    /// <summary>
    /// Interaction logic for BeheerPlanten.xaml
    /// </summary>
    public partial class BeheerPlanten : Window
    {
        public BeheerPlanten(Gebruiker user)
        {
            InitializeComponent();
            DataContext = new TabsBeheerViewModel(this, user);
        }
    }
}