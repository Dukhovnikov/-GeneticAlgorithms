using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using myMatrix;
using myVector;

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
        public int MaximumIterations { get; set; } = 500;

        /// <summary>
        /// Размер турнира.
        /// </summary>
        public int TournamentSize { get; set; }

        /// <summary>
        /// Переменная для генерации случайного числа из заданного промежутка.
        /// </summary>
        static Random RandomNumber = new Random();

        /// <summary>
        /// Скрещивание выбранных особей.
        /// </summary>
        private List<Vector> Crossing(Vector Parent1, Vector Parent2)
        {
            int BreakPoint = RandomNumber.Next(0, Parent1.Size); /// Точка разрыва.
            List<Vector> Children = new List<Vector>();
            if (CrossingProbability > RandomNumber.NextDouble())
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
        private Vector MutationRealValued(Vector child)
        {
            Vector MutantChild = child; /// Ребенок мутант.
            for (int i = 0; i < child.Size; i++)
            {
                if (MutationProbability > RandomNumber.NextDouble())
                {
                    MutantChild[i] = MutantChild[i] + RandomNumber.NextDouble() - 0.5;
                }
            }
            return MutantChild;
        }

        public Vector GeneticAlgoritm(Population population)
        {
            int k = 0;
            List<Vector> TemporaryPopulation = new List<Vector>(); /// Временная популяция.

            while (k < MaximumIterations)
            {
                population = population.GetParentPool(TournamentSize);

                while (TemporaryPopulation.Count != population.Count)
                {
                    foreach (Vector item in Crossing(population.RandomSelection, population.RandomSelection))
                    {
                        TemporaryPopulation.Add(item);
                    }
                }

                population = new Population(TemporaryPopulation);
                TemporaryPopulation.Clear();
                k++;
            }
            return population.Min;
        }
    }
}
