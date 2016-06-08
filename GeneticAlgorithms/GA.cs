using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using myMatrix;
using myVector;
using System.Windows;

namespace GeneticAlgorithms
{
    /// <summary>
    /// Класс, реализующий генетический алгоритм.
    /// </summary>
    sealed class GA
    {
        /// <summary>
        /// Вероятность скрещивания.
        /// </summary>
        public readonly double CrossingProbability;

        /// <summary>
        /// Вероятность мутации.
        /// </summary>
        public readonly double MutationProbability;

        /// <summary>
        /// Максимальное количество итераций.
        /// </summary>
        public int MaximumIterations { get; set; } = 100;

        /// <summary>
        /// Размер турнира.
        /// </summary>
        public int TournamentSize { get; set; }

        /// <summary>
        /// Переменная для генерации случайного числа из заданного промежутка.
        /// </summary>
        static Random RandomNumber = new Random();

        public GA(double CrossingProbability, double MutationProbability, int TournamentSize)
        {
            this.CrossingProbability = CrossingProbability;
            this.MutationProbability = MutationProbability;
            this.TournamentSize = TournamentSize;
        }

        /// <summary>
        /// Скрещивание выбранных особей.
        /// </summary>
        private List<iVector> Crossing(iVector Parent1, iVector Parent2)
        {
            //int BreakPoint = RandomNumber.Next(Parent1.Size); /// Точка разрыва.
            int BreakPoint = Population.RandomNumber.Next(Parent1.Size);
            List<iVector> Children = new List<iVector>();
            //if (CrossingProbability > RandomNumber.NextDouble())
            if (CrossingProbability > Population.RandomNumber.NextDouble())
            {
                double temp = Parent1[BreakPoint];
                Parent1[BreakPoint] = Parent2[BreakPoint];
                Parent2[BreakPoint] = temp;
                Parent1 = MutationRealValued(Parent1);
                Parent2 = MutationRealValued(Parent2);
            }
            Children.Add(Parent1);
            Children.Add(Parent2);
            return Children;
        }

        /// <summary>
        /// Мутация для вещественной особи.
        /// </summary>
        private iVector MutationRealValued(iVector child)
        {
            iVector MutantChild = child; /// Ребенок мутант.
            for (int i = 0; i < child.Size; i++)
            {
                //if (MutationProbability > RandomNumber.NextDouble())
                if (MutationProbability > Population.RandomNumber.NextDouble())
                {
                    //MutantChild[i] = MutantChild[i] + RandomNumber.NextDouble() - 0.5;
                    MutantChild[i] = MutantChild[i] + Population.RandomNumber.NextDouble() - 0.5;
                }
            }
            return MutantChild;
        }

        public iVector GeneticAlgoritm(Population population)
        {
            int k = 0;
            List<iVector> TemporaryPopulation = new List<iVector>(); /// Временная популяция.
            //Population testPopulation = population;
            while (k < MaximumIterations)
            {
                population = population.GetParentPool(TournamentSize);
                //testPopulation = population.GetParentPool(TournamentSize);
                if (population.Max.FitnessFunction - population.Min.FitnessFunction < 0.01) MessageBox.Show("Популяция выродилась!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                while (TemporaryPopulation.Count != population.Count)
                {
                    foreach (iVector item in Crossing(population.RandomSelection, population.RandomSelection))
                    {
                        TemporaryPopulation.Add(item);
                    }
                }

                population = new Population(TemporaryPopulation);
                //if (k == 99) MessageBox.Show("Популяция выродилась!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                TemporaryPopulation.Clear();
                k++;
            }
            //iVector min = population.Max;
            //population = null;
            return population.Min;
        }
    }
}
