using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using myVector;
using MathParser;
using DrawGraph;
namespace GeneticAlgorithms
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            listBoxCoding.ItemsSource = VisualizationGA.GetViews().Select(item => item.ViewsCoding).Distinct().ToList();
            listBoxCoding.SelectedIndex = 0;
            listBoxCrossing.SelectedIndex = 1;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Run();
        }
        private void listBoxCoding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listBoxCrossing.ItemsSource = VisualizationGA.GetViews().Where(item => (listBoxCoding.SelectedItem.ToString() == item.ViewsCoding)).Select(item => item.ViewsСrossing).ToList();
            switch (listBoxCoding.SelectedIndex)
            {
                case 0: checkBoxMeshSeal.IsEnabled = false; Vectors.CodingType = Coding.Real; textBoxBitsCount.IsEnabled = false; break;
                case 1: checkBoxMeshSeal.IsEnabled = true; Vectors.CodingType = Coding.Integer; textBoxBitsCount.IsEnabled = true; break;
            }
        }

        private void listBoxCrossing_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GA.AssignedDelegat(listBoxCoding.SelectedIndex, listBoxCrossing.SelectedIndex);
        }

        private void checkBoxMutation_Checked(object sender, RoutedEventArgs e)
        {
            textBoxMutationRate.IsEnabled = true;
        }

        private void checkBoxMutation_Unchecked(object sender, RoutedEventArgs e)
        {
            textBoxMutationRate.IsEnabled = false;
            GA.MutationProbability = -1;
            textBoxMutationRate.Text = "";
        }

        private void checkBoxInversion_Checked(object sender, RoutedEventArgs e)
        {
            textBoxInversionRate.IsEnabled = true;
        }

        private void checkBoxInversion_Unchecked(object sender, RoutedEventArgs e)
        {
            textBoxInversionRate.IsEnabled = false;
            GA.InversionProbability = -1;
            textBoxInversionRate.Text = "";
        }

        private void defaultSetting()
        {
            GA.defaultSetting();
            textBoxCrossingRate.Text = GA.CrossingProbability.ToString();
            textBoxMutationRate.Text = GA.MutationProbability.ToString();
            textBoxInversionRate.Text = GA.InversionProbability.ToString();
            textBoxTournamentSize.Text = GA.TournamentSize.ToString();
            textBoxMaximumIteration.Text = GA.MaximumIterations.ToString();
            checkBoxMutation.IsChecked = true;
            checkBoxInversion.IsChecked = true;
            listBoxCoding.SelectedIndex = 1;
            listBoxCrossing.SelectedIndex = 0;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            defaultSetting();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            textBoxFunction.Text = "8*x1^2 + 4*x1*x2 + 5*x2^2";
            textBoxStartPoint.Text = "-5,12 -5,12";
            textBoxEndPoint.Text = "5,12 5,12";
            textBoxSizePopulation.Text = "30";
            textBoxCrossingRate.Text = "0,9";
            textBoxMutationRate.Text = "0,05";
            checkBoxMutation.IsChecked = true;
            textBoxTournamentSize.Text = "4";
            textBoxMaximumIteration.Text = "50";
            textBoxBitsCount.Text = "12";
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Form1 form = new Form1(GA.Data.MinValues, GA.Data.MiddleValues);
        }

        private void checkBoxPopulationSpike_Checked(object sender, RoutedEventArgs e)
        {
            GA.PopulationSpike = true;
        }

        private void checkBoxPopulationSpike_Unchecked(object sender, RoutedEventArgs e)
        {
            GA.PopulationSpike = false;
        }

        private void checkBoxMeshSeal_Checked(object sender, RoutedEventArgs e)
        {
            GA.PopulationMeshSeal = true;
        }

        private void checkBoxMeshSeal_Unchecked(object sender, RoutedEventArgs e)
        {
            GA.PopulationMeshSeal = false;
        }

        private void checkBoxBreakGeneration_Checked(object sender, RoutedEventArgs e)
        {
            textBoxBreakGeneration.IsEnabled = true;
        }

        private void checkBoxBreakGeneration_Unchecked(object sender, RoutedEventArgs e)
        {
            GA.BreakGeneration = -1;
            textBoxBreakGeneration.IsEnabled = false;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            textBoxFunction.Text = "100*(x2 - x1^3)^2 + (1 - x1)^2";
            textBoxStartPoint.Text = "0 0";
            textBoxEndPoint.Text = "5,12 5,12";
            textBoxSizePopulation.Text = "100";
            textBoxCrossingRate.Text = "0,75";
            textBoxMutationRate.Text = "0,1";
            textBoxInversionRate.Text = "0,07";
            textBoxBitsCount.Text = "12";
            textBoxMaximumIteration.Text = "50";
            textBoxTournamentSize.Text = "5";
        }

        private void checkBoxClassicMO_Checked(object sender, RoutedEventArgs e)
        {
            isUseClassicMO = true;
            groupBoxClassicMO.IsEnabled = true;
        }

        private void checkBoxClassicMO_Unchecked(object sender, RoutedEventArgs e)
        {
            isUseClassicMO = false;
            groupBoxClassicMO.IsEnabled = false;
        }
    }
}
