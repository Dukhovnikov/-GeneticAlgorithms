using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MathParser;
using myVector;
namespace GeneticAlgorithms
{
    public partial class MainWindow
    {
        /// <summary>
        /// Переменная, для определления - использовать/не использовать классические методы оптимизации.
        /// </summary>
        bool isUseClassicMO = false;

        private void Run()
        {
            textBoxRezultEnd.Text = "";
            textBoxRezultGA.Text = "";
            GA.ClearData();
            GA.DegenerationTrack = false;
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
            GA.BreakGeneration = textBoxBreakGeneration.Text == "" ? -1 : Convert.ToDouble(textBoxBreakGeneration.Text);
            Vectors.BitsCount = Convert.ToByte(textBoxBitsCount.Text);
            Vectors minimum = GA.mainGeneticAlgoritm();

            if (!isUseClassicMO)
            {
                textBoxRezultGA.Text = "Найденный минимум: [ " + minimum.ToString() + " ]" + Environment.NewLine;
                if (GA.DegenerationTrack && checkBoxIsPopulationConfluent.IsChecked == true) { textBoxRezultGA.Text += "У текущего решения была вырожденная популяция"; }
            }
            else
            {
                textBoxRezultGA.Text = "Найденный минимум: [ " + minimum.ToString() + " ]" + Environment.NewLine;
                if (GA.DegenerationTrack && checkBoxIsPopulationConfluent.IsChecked == true) { textBoxRezultGA.Text += "У текущего решения была вырожденная популяция"; }

                ClassicOptimizationMethod ClassicMO = new ClassicOptimizationMethod(Vectors.Function, minimum);

                if (radioButtonBFGSH.IsChecked == true) { minimum = ClassicMO.getOptimizeBFGSH(); }
                if (radioButtonPhletcherRivs.IsChecked == true) { minimum = ClassicMO.getOptimizeConjugateGradient(); }

                textBoxRezultEnd.Text = "Найденный минимум: [ " + minimum.ToString() + " ]" + Environment.NewLine;
            }
        }

    }
}
