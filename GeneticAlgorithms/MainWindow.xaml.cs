﻿using System;
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
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Func<double[], double> function = x => 4 * Math.Pow(x[0] - 5, 2) + Math.Pow(x[1] - 6, 2);
            Func<double[],double> function1 = x => 100 * Math.Pow(x[1] - x[0], 2) + Math.Pow(1 - x[0], 2);
            Func<double[], double> function2 = x => Math.Pow(x[0], 3) + Math.Pow(x[1], 2) - 3 * x[0] - 2 * x[1] + 2;
            Vectors.Function = function2;
            Vectors StartPoint = new Vectors(-5.12, -5.12);
            Vectors EndPoint = new Vectors(1, 1);
            Vectors.CodingType = Coding.Integer;


            GA GneticAlhoritm = new GA(0.7, 0.1, 2);
            GneticAlhoritm.GeneticAlgoritmInteger(new Population(StartPoint, EndPoint, 100, Coding.Integer,24));

        }
    }
}
