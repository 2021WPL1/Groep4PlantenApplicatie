using PlantenApplicatie.Domain;
using PlantenApplicatie.viewmodels;
using System.Windows;
namespace PlantenApplicatie
{
    /// <summary>
    /// Interaction logic for PlantDetails.xaml
    /// </summary>
    public partial class PlantDetails : Window
    {
        public PlantDetails(Plant selectedPlant, Gebruiker gebruiker)
        {
            InitializeComponent();
            DataContext = new TabsViewModel(selectedPlant, gebruiker);
        }

       
    }
}

