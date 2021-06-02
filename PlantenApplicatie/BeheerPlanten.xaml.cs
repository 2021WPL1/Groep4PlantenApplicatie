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
        public BeheerPlanten(Gebruiker gebruiker)
        {
            InitializeComponent();
            DataContext = new TabsBeheerViewModel(this, gebruiker);
        }
    }
}