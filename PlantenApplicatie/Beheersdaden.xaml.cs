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
    /// Interaction logic for Beheersdaden.xaml
    /// </summary>
    public partial class Beheersdaden : Window
    {

        // constructor Davy
        public Beheersdaden(Plant selectedPlant)
        {
            InitializeComponent();
            DataContext = new BeheerDadenViewModel(selectedPlant);
        }
    }
}
