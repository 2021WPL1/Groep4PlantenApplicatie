using PlantenApplicatie.viewmodels;
using System.Windows;

namespace PlantenApplicatie
{
    /// <summary>
    /// Interaction logic for BeheerPlanten.xaml
    /// </summary>
    public partial class BeheerPlanten : Window
    {
        public BeheerPlanten()
        {
            InitializeComponent();

            BeheerPlantenViewModel beheerPlantenViewModel = new();
            DataContext = beheerPlantenViewModel;
        }
    }
}