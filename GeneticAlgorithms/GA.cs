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
        private List<Vectors> Crossing(Vectors Parent1, Vectors Parent2)
        {
            //int BreakPoint = RandomNumber.Next(Parent1.Size); /// Точка разрыва.
            int BreakPoint = Population.RandomNumber.Next(Parent1.Size);
            List<Vectors> Children = new List<Vectors>();
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
        /// Одноточечное скрещивание для целочисленного кодирования.
        /// </summary>
        private List<Vectors> CrossingIntegerOnePoint(Vectors Parent1, Vectors Parent2)
        {
            bool check = false;
            List<Vectors> Children = new List<Vectors>();
            /// Этап скрещивания
            for (int i = 0; i < Parent1.Size; i++)
            {
                if (CrossingProbability > RandomNumber.NextDouble())
                {
                    check = true;
                    int mask = (1 << RandomNumber.Next(Vectors.BitsCount)) - 1;
                    int swapMask = (Convert.ToInt32(Parent1[i]) ^ Convert.ToInt32(Parent2[i])) & mask;
                    Parent1[i] = Convert.ToInt32(Parent1[i]) ^ swapMask;
                    Parent2[i] = Convert.ToInt32(Parent2[i]) ^ swapMask;
                }
            }
            /// Этап мутации
            if (check)
            {
                Parent1 = MutationBit(Parent1);
                Parent2 = MutationBit(Parent2);
            }
            Children.Add(Parent1);
            Children.Add(Parent2);
            return Children;
        }

        /// <summary>
        /// Двухточечное скрещиание для целочисленного кодирования.
        /// </summary>
        private List<Vectors> CrossingIntegerTwoPoint(Vectors Parent1, Vectors Parent2)
        {
            bool check = false;
            List<Vectors> Children = new List<Vectors>();
            for (int i = 0; i < Parent1.Size; i++)
            {
                /// Этап скрещивания
                if (CrossingProbability > RandomNumber.NextDouble())
                {
                    check = true;
                    int mask1 = (1 << RandomNumber.Next(Vectors.BitsCount)) - 1;
                    int mask2 = (1 << RandomNumber.Next(Vectors.BitsCount)) - 1;
                    int mask = Math.Abs(mask1 - mask2);
                    int swapMask = (Convert.ToInt32(Parent1[i]) ^ Convert.ToInt32(Parent2[i])) & mask;
                    Parent1[i] = Convert.ToInt32(Parent1[i]) ^ swapMask;
                    Parent2[i] = Convert.ToInt32(Parent2[i]) ^ swapMask;
                }
            }
                /// Этап мутации
                if (check)
                {
                    Parent1 = MutationBit(Parent1);
                    Parent2 = MutationBit(Parent2);
                }
                Children.Add(Parent1);
                Children.Add(Parent2);
                return Children;
            
        }

        private List<Vectors> CrossingIntegerUniform(Vectors Parent1, Vectors Parent2)
        {

        }

        /// <summary>
        /// Битовая мутация.
        /// </summary>
        private Vectors MutationBit(Vectors Child)
        {
            Vectors MutantChild = Child; /// Ребенок мутант.
            for (int i = 0; i < Child.Size; i++)
            {
                for (int j = 0; j < Vectors.BitsCount; j++)
                {
                    if (MutationProbability > RandomNumber.NextDouble())
                    {
                        int swapMask = 1 << j;
                        MutantChild[i] = Convert.ToInt32(MutantChild[i])^swapMask; 
                    }
                }
            }
            return MutantChild;
        }

        /// <summary>
        /// Мутация для вещественной особи.
        /// </summary>
        private Vectors MutationRealValued(Vectors child)
        {
            Vectors MutantChild = child; /// Ребенок мутант.
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

        /// <summary>
        /// Генетический алгоритм для вещественного кодирования.
        /// </summary>
        public Vectors GeneticAlgoritm(Population population)
        {
            int k = 0;
            List<Vectors> TemporaryPopulation = new List<Vectors>(); /// Временная популяция.
            Population testPopulation = population;
            while (k < MaximumIterations)
            {
                //population = population.GetParentPool(TournamentSize);
                testPopulation = population.GetParentPool(TournamentSize);
                if (population.Max.FitnessFunction - population.Min.FitnessFunction < 0.01) MessageBox.Show("Популяция выродилась!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                while (TemporaryPopulation.Count != population.Count)
                {
                    foreach (Vectors item in Crossing(population.RandomSelection, population.RandomSelection))
                    {
                        TemporaryPopulation.Add(item);
                    }
                }

                population = new Population(TemporaryPopulation);
                if (k == 99) MessageBox.Show("Популяция выродилась!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                TemporaryPopulation.Clear();
                k++;
            }
            Vectors min = population.Min;
            population = null;
            return min;
        }

        /// <summary>
        /// Генетический алгоритм, для целочисленного кодирования.
        /// </summary>
        public Vectors GeneticAlgoritmInteger(Population population)
        {
            int k = 0;
            List<Vectors> TemporaryPopulation = new List<Vectors>(); /// Временная популяция.
            Population ControlPopulation = population; /// Популяция родителей/отобранных особей
            while (k++ < MaximumIterations)
            {
                ControlPopulation = ControlPopulation.GetParentPool(TournamentSize);
                if (population.Max.FitnessFunction - population.Min.FitnessFunction < 0.01) MessageBox.Show("Популяция выродилась!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                while (TemporaryPopulation.Count != ControlPopulation.Count)
                {
                    foreach (var item in CrossingIntegerOnePoint(ControlPopulation.RandomSelection, ControlPopulation.RandomSelection))
                    {
                        TemporaryPopulation.Add(item);
                    }
                }

                ControlPopulation = new Population(TemporaryPopulation);
                TemporaryPopulation.Clear();
            }
            Vectors min = new Vectors(ControlPopulation.Min);
            ControlPopulation = null;
            return min.ToReal();
        }
    }
}
