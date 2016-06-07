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
            textBoxMutationRate.IsEnabled = false;
            textBoxInversionRate.IsEnabled = false;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //if (textBoxFunction.Text == "") { MessageBox.Show("Функция не задана", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; } /// Проверка введена ли функция.
            //int i = 0; /// Количество переменных в уравнении.
            //while (textBoxFunction.Text.Contains("x" + (i + 1))) i++; /// Подсчет количества переменных.

            //Parser parser = new Parser(); /// Инициализация парсера.
            //parser.createDelegat(textBoxFunction.Text); /// Передача парсеру текста целевой функции.
            //Vectors.Function = Parser.variable; /// Создание делегата исходной функции на С#.

            ////Func<double[], double> function = x => 4 * Math.Pow(x[0] - 5, 2) + Math.Pow(x[1] - 6, 2);
            ////Func<double[],double> function1 = x => 100 * Math.Pow(x[1] - x[0], 2) + Math.Pow(1 - x[0], 2);
            ////Func<double[], double> function2 = x => Math.Pow(x[0], 3) + Math.Pow(x[1], 2) - 3 * x[0] - 2 * x[1] + 2;

            //if (textBoxStartPoint.Text != "") { Vectors.StartPoint = new Vectors(textBoxStartPoint.Text); } /// Инициализация стартового интервала.
            //if (textBoxEndPoint.Text != "") { Vectors.EndPoint = new Vectors(textBoxEndPoint.Text); } /// Инициализация конечного интервала.

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

        private void Run()
        {
            if (textBoxFunction.Text == "") { MessageBox.Show("Функция не задана", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; } /// Проверка введена ли функция.
            int i = 0; /// Количество переменных в уравнении.
            while (textBoxFunction.Text.Contains("x" + (i + 1))) i++; /// Подсчет количества переменных.

            Parser parser = new Parser(); /// Инициализация парсера.
            parser.createDelegat(textBoxFunction.Text); /// Передача парсеру текста целевой функции.
            Vectors.Function = Parser.variable; /// Создание делегата исходной функции на С#.

            if (textBoxStartPoint.Text != "") { Vectors.StartPoint = new Vectors(textBoxStartPoint.Text); } /// Инициализация стартового интервала.
            if (textBoxEndPoint.Text != "") { Vectors.EndPoint = new Vectors(textBoxEndPoint.Text); } /// Инициализация конечного интервала.

            GA.CrossingProbability = Convert.ToDouble(textBoxCrossingRate.Text);
            GA.MutationProbability = Convert.ToDouble(textBoxMutationRate.Text);
            GA.SizePopulation = int.Parse(textBoxSizePopulation.Text);
            GA.MaximumIterations = int.Parse(textBoxMaximumIteration.Text);
            GA.TournamentSize = int.Parse(textBoxTournamentSize.Text);
            Vectors.BitsCount = byte.Parse(textBoxBitsCount.Text);
            Vectors minimum = GA.mainGeneticAlgoritm();

            textBoxRezultGA.Text = "Найденный минимум: [ " + minimum.ToString() + " ]" + Environment.NewLine;
            
            //Result.Text = "Начальная точка: [ " + startpoint.ToString() + " ]" + Environment.NewLine;
            //Result.Text += "Найденный минимум: [ " + minimum.ToString() + " ]" + Environment.NewLine;
            //Result.Text += "Количество итераций: " + k;
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
            textBoxTournamentSize.Text = "4";
            textBoxMaximumIteration.Text = "50";
            textBoxBitsCount.Text = "12";
        }
    }
}
