﻿using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
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
    /// Interaction logic for PlantDetails.xaml
    /// </summary>

    public partial class PlantDetails : Window
    {
       
        private PlantenDao plantenDAO;

        private Plant plant;
        public PlantDetails()
        {
            InitializeComponent();
            plantenDAO = PlantenDao.Instance;
            plant = (Plant)lblPlantnaam.Content;
            
            Start();
        }

        private void Start()
        {
            cmbEigenschappen.Items.Add("Plant");
            cmbEigenschappen.Items.Add("FenoType");
            cmbEigenschappen.Items.Add("Abiotiek");
            cmbEigenschappen.Items.Add("Beheer");
            cmbEigenschappen.Items.Add("Extra's");
            cmbEigenschappen.Items.Add("Commensalisme");

            cmbEigenschappen.SelectedIndex = 0;


        }

        private void btnSluiten_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void cmbEigenschappen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectie();
        }

        private void selectie()
        {
            string comboboxText = cmbEigenschappen.SelectedValue.ToString();

            switch (comboboxText)
            {
                case "Plant":
                    lstLijst.Visibility = Visibility.Visible;
                    ChangeToPlant();
                    break;
                case "FenoType":
                    lstLijst.Visibility = Visibility.Visible;
                    ChangeToFenotype();

                    break;
                case "Abiotiek":
                    lstLijst.Visibility = Visibility.Collapsed;
                    ChangeToAbiotiek();

                    break;
                case "Beheer":
                    lstLijst.Visibility = Visibility.Visible;
                    ChangeToBeheer();

                    break;
                case "Extra's":
                    lstLijst.Visibility = Visibility.Collapsed;
                    ChangeToExtra();

                    break;
                case "Commensalisme":
                    lstLijst.Visibility = Visibility.Collapsed;
                    ChangeToCommensalisme();

                    break;

                default:
                    break;
            }
        }
    

        private void ChangeToPlant()
        {
            lblType.Content = plant.Fgsv;
       
          
        }
        private void ChangeToFenotype()
        {
            
            lblType.Content = "Bladgrootte :";
            lblFamilie.Content = "Bladvorm :";
            lblGeslacht.Content = "Ratio Bloei/Blad :";
            lblSoort.Content = "Bloeiwijze :";
            lblVariant.Content = "Habitus :";
            lblPlantdichtheidMax.Content = "Levensvorm :";
            lblPlantdichtheidMin.Content = "";
            lblLaatste.Content = "";
        }

        private void ChangeToAbiotiek()
        {
            
            lblType.Content = "Bezonning :";
            lblFamilie.Content = "Grondsoort :";
            lblGeslacht.Content = "Vochtbehoefte :";
            lblSoort.Content = "Voedingsbehoefte :";
            lblVariant.Content = "Antagonische omgeving :";
            lblPlantdichtheidMax.Content = "Habitat :";
            lblPlantdichtheidMin.Content = "";
            lblLaatste.Content = "";
            //lblBladGrootteResult.Content = plantenDAO.GetAbiotiek();
        }
        private void ChangeToBeheer()
        {
            lblType.Content = "";
            lblFamilie.Content = "";
            lblGeslacht.Content = "";
            lblSoort.Content = "";
            lblVariant.Content = "";
            lblPlantdichtheidMax.Content = "";
            lblPlantdichtheidMin.Content = "";
            lblLaatste.Content = "";
        }
        private void ChangeToExtra()
        {
            lblType.Content = "Nectarwaarde :";
            lblFamilie.Content = "Pollenwaarde :";
            lblGeslacht.Content = "Bijvriendelijk :";
            lblSoort.Content = "Vlindervriendelijk :";
            lblVariant.Content = "Eetbaar :";
            lblPlantdichtheidMax.Content = "Kruidgebruik :";
            lblPlantdichtheidMin.Content = "Geurend :";
            lblLaatste.Content = "Vorstgevoelig :";
        }
        private void ChangeToCommensalisme()
        {
            lblType.Content = "Ontwikkelingssnelheid :";
            lblFamilie.Content = "Strategie :";
            lblGeslacht.Content = "Sociabiliteit :";
            lblSoort.Content = "Levensvorm :";
            lblVariant.Content = "";
            lblPlantdichtheidMax.Content = "";
            lblPlantdichtheidMin.Content = "";
            lblLaatste.Content = "";
        }
    }
}

