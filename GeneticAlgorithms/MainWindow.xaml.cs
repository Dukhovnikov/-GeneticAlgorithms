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
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxFunction.Text == "") { MessageBox.Show("Функция не задана", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; } /// Проверка введена ли функция.
            int i = 0; /// Количество переменных в уравнении.
            while (textBoxFunction.Text.Contains("x" + (i + 1))) i++; /// Подсчет количества переменных.

            Parser parser = new Parser(); /// Инициализация парсера.
            parser.createDelegat(textBoxFunction.Text); /// Передача парсеру текста целевой функции.
            Vectors.Function = Parser.variable; /// Создание делегата исходной функции на С#.

            //Func<double[], double> function = x => 4 * Math.Pow(x[0] - 5, 2) + Math.Pow(x[1] - 6, 2);
            //Func<double[],double> function1 = x => 100 * Math.Pow(x[1] - x[0], 2) + Math.Pow(1 - x[0], 2);
            //Func<double[], double> function2 = x => Math.Pow(x[0], 3) + Math.Pow(x[1], 2) - 3 * x[0] - 2 * x[1] + 2;

            if (textBoxStartPoint.Text != "") { Vectors.StartPoint = new Vectors(textBoxStartPoint.Text); }
            if (textBoxEndPoint.Text != "") { Vectors.EndPoint = new Vectors(textBoxEndPoint.Text); }
            Vectors.CodingType = Coding.Integer;
            GA GneticAlhoritm = new GA(0.7, 0.1, 2);
            GneticAlhoritm.GeneticAlgoritmInteger(new Population(StartPoint, EndPoint, 100, Coding.Integer,24));

        }

        private void listBoxCoding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listBoxCrossing.ItemsSource = VisualizationGA.GetViews().Where(item => (listBoxCoding.SelectedItem.ToString() == item.ViewsCoding)).Select(item => item.ViewsСrossing).ToList();
        }
    }
}
