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

            BeheerPlantenViewModel beheerPlantenViewModel = new(gebruiker);
            DataContext = beheerPlantenViewModel;
        }
    }
}