using PlantenApplicatie.Data;
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
    /// Interaction logic for BeheerPlanten.xaml
    /// </summary>
    public partial class BeheerPlanten : Window
    {
        private PlantenDao plantenDao;
        private BeheerPlantenViewModel beheerPlantenViewModel;
        
        //initialize de scherm (Davy)
        public BeheerPlanten()
        {
            InitializeComponent();
            plantenDao = PlantenDao.Instance;

            beheerPlantenViewModel = new BeheerPlantenViewModel(
                PlantenDao.Instance);
            DataContext = beheerPlantenViewModel;
            beheerPlantenViewModel.LoadPlants();
            beheerPlantenViewModel.LoadTypes();
            beheerPlantenViewModel.LoadSoorten();
            beheerPlantenViewModel.LoadFamilies();
            beheerPlantenViewModel.LoadGenus();
            beheerPlantenViewModel.LoadVariants();


        }


        /*
       

        
        private PlantenDao plantenDao;

        public BeheerPlanten()
        {
            InitializeComponent();
            plantenDao = PlantenDao.Instance;

            lvPlanten.ItemsSource = plantenDao.GetPlanten();
            cmbType.ItemsSource = plantenDao.GetTypes();
            cmbGeslacht.ItemsSource = plantenDao.GetUniqueGenusNames();
            cmbSoort.ItemsSource = plantenDao.GetUniqueSpeciesNames();
            cmbFamilie.ItemsSource = plantenDao.GetUniqueFamilyNames();
        }

        private void ResetInputFields()
        {
            txtPlantnaam.Text = string.Empty;
            txtNLNaam.Text = string.Empty;
            cmbFamilie.SelectedValue = null;
            cmbGeslacht.SelectedValue = null;
            cmbSoort.SelectedValue = null;
            txtVariant.Text = string.Empty;
        }

        private void SearchPlantenOnEnterPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchPlanten();
            }
        }

       

        private void btnDetailsPlant_Click(object sender, RoutedEventArgs e)
        {
            PlantDetails plantDetails = new PlantDetails();
            
            plantDetails.Show();
        }
        
        private void btnZoeken_Click(object sender, RoutedEventArgs e)
        {
            SearchPlanten();
        }

        private void SearchPlanten()
        {
            var type = cmbType.SelectedValue is null ? null : cmbType.SelectedValue.ToString();
            var family = cmbFamilie.SelectedValue is null ? null : cmbFamilie.SelectedValue.ToString();
            var genus = cmbGeslacht.SelectedValue is null ? null : cmbGeslacht.SelectedValue.ToString();
            var species = cmbSoort.SelectedValue is null ? null : cmbSoort.SelectedValue.ToString();

            var list = plantenDao.SearchPlants(type,
                family, genus,
                species, txtVariant.Text, txtPlantnaam.Text);

            lvPlanten.ItemsSource = list;
        }
        
         */
    }
        
}
