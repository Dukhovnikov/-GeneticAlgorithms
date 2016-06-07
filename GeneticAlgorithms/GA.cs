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
    /// Делегат для различных типов скрещивания.
    /// </summary>
    public delegate List<Vectors> CrossingType(Vectors Parent1, Vectors Parent2);
    /// <summary>
    /// Делегат для различных типов генетического алгоритма.
    /// </summary>
    public delegate Vectors GeneticAlgoritmType(Population population);

    /// <summary>
    /// Класс, реализующий генетический алгоритм.
    /// </summary>
    static class GA
    {
        static CrossingType Crossing;
        static GeneticAlgoritmType GeneticAlgoritm;

        /// <summary>
        /// Вероятность скрещивания.
        /// </summary>
        public static double CrossingProbability { get; set; }

        /// <summary>
        /// Вероятность мутации.
        /// </summary>
        public static double MutationProbability { get; set; } = -1;

        /// <summary>
        /// Вероятность инверсии.
        /// </summary>
        public static double InversionProbability { get; set; } = -1;

        /// <summary>
        /// Коэффициент скрещивания, при BLXa - скрещивании.
        /// </summary>
        public static double BLXaRate { get; set; }

        /// <summary>
        /// Максимальное количество итераций.
        /// </summary>
        public static int MaximumIterations { get; set; }

        /// <summary>
        /// Размер турнира.
        /// </summary>
        public static int TournamentSize { get; set; }

        /// <summary>
        /// Размер популяции.
        /// </summary>
        public static int SizePopulation { get; set; }

        /// <summary>
        /// Переменная для генерации случайного числа из заданного промежутка.
        /// </summary>
        static Random RandomNumber = new Random();

        /// <summary>
        /// Скрещивание выбранных особей.
        /// </summary>
        private static List<Vectors> CrossingRealOnePoint(Vectors Parent1, Vectors Parent2)
        {
            int BreakPoint = RandomNumber.Next(Parent1.Size); /// Точка разрыва.
            List<Vectors> Children = new List<Vectors>();
            if (CrossingProbability > RandomNumber.NextDouble())
            {
                double temp = Parent1[BreakPoint];
                Parent1[BreakPoint] = Parent2[BreakPoint];
                Parent2[BreakPoint] = temp;
                if (onMutation)
                {
                    Parent1 = MutationRealValued(Parent1);
                    Parent2 = MutationRealValued(Parent2);
                }
            }
            Children.Add(Parent1);
            Children.Add(Parent2);
            return Children;
        }
        
        /// <summary>
        /// Одноточечное скрещивание для целочисленного кодирования.
        /// </summary>
        private static List<Vectors> CrossingIntegerOnePoint(Vectors Parent1, Vectors Parent2)
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
            if (check && onMutation)
            {
                Parent1 = MutationBit(Parent1);
                Parent2 = MutationBit(Parent2);
            }
            if (onInversion)
            {
                Parent1 = InversionBit(Parent1);
                Parent2 = InversionBit(Parent2);
            }
            Children.Add(Parent1);
            Children.Add(Parent2);
            return Children;
        }

        /// <summary>
        /// Двухточечное скрещиание для целочисленного кодирования.
        /// </summary>
        private static List<Vectors> CrossingIntegerTwoPoint(Vectors Parent1, Vectors Parent2)
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
            if (check && onMutation)
            {
                Parent1 = MutationBit(Parent1);
                Parent2 = MutationBit(Parent2);
            }
            if (onInversion)
            {
                Parent1 = InversionBit(Parent1);
                Parent2 = InversionBit(Parent2);
            }
            Children.Add(Parent1);
            Children.Add(Parent2);
            return Children;
            
        }

        /// <summary>
        /// Однородное скрещивание для целочисленного кодирования.
        /// </summary>
        private static List<Vectors> CrossingIntegerUniform(Vectors Parent1, Vectors Parent2)
        {
            bool check = false;
            List<Vectors> Children = new List<Vectors>();
            for (int i = 0; i < Parent1.Size; i++)
            {
                /// Этап скрещивания
                if (CrossingProbability > RandomNumber.NextDouble())
                {
                    int mask = 0;
                    check = true;
                    for (int j = 0; j < Vectors.BitsCount; j++)
                    {
                        if (RandomNumber.NextDouble() > 0.5) mask += 1 << j;
                    }
                    int swapMask = (Convert.ToInt32(Parent1[i]) ^ Convert.ToInt32(Parent2[i])) & mask;
                    Parent1[i] = Convert.ToInt32(Parent1[i]) ^ swapMask;
                    Parent2[i] = Convert.ToInt32(Parent2[i]) ^ swapMask;
                }
            }
            /// Этап мутации
            if (check && onMutation)
            {
                Parent1 = MutationBit(Parent1);
                Parent2 = MutationBit(Parent2);
            }
            if (onInversion)
            {
                Parent1 = InversionBit(Parent1);
                Parent2 = InversionBit(Parent2);
            }
            Children.Add(Parent1);
            Children.Add(Parent2);
            return Children;
        }

        /// <summary>
        /// Скрещивание BLX-a для вещественного кодирования.
        /// </summary>
        private static List<Vectors> CrossingBLXalpha(Vectors Parent1, Vectors Parent2)
        {
            List<Vectors> Children = new List<Vectors>();
            int BreakPoint = RandomNumber.Next(Parent1.Size);
            for (int i = 0; i < Parent1.Size; i++)
            {
                if (CrossingProbability > RandomNumber.NextDouble())
                {
                    double temp = Parent1[i];
                    Parent1[i] = BLXaRate * Parent1[i] + (1 - BLXaRate) * Parent2[i];
                    Parent2[i] = BLXaRate * Parent2[i] + (1 - BLXaRate) * temp;
                    if (onMutation)
                    {
                        Parent1 = MutationRealValued(Parent1);
                        Parent2 = MutationRealValued(Parent2);
                    }
                }
            }
            Children.Add(Parent1);
            Children.Add(Parent2);
            return Children;
        }

        /// <summary>
        /// Битовая мутация.
        /// </summary>
        private static Vectors MutationBit(Vectors Child)
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
        /// Битовая инверсия.
        /// </summary>
        private static Vectors InversionBit(Vectors Child)
        {
            Vectors MutantChild = Child; /// Ребенок мутант.
            for (int i = 0; i < Child.Size; i++)
            {
                for (int j = 0; j < Vectors.BitsCount; j++)
                {
                    if (InversionProbability > RandomNumber.NextDouble())
                    {
                        int swapMask = 1 << j;
                        MutantChild[i] = Convert.ToInt32(MutantChild[i]) ^ swapMask;
                    }
                }
            }
            return MutantChild;
        }

        /// <summary>
        /// Мутация для вещественной особи.
        /// </summary>
        private static Vectors MutationRealValued(Vectors child)
        {
            Vectors MutantChild = child; /// Ребенок мутант.
            for (int i = 0; i < child.Size; i++)
            {
                if (MutationProbability > RandomNumber.NextDouble())
                {
                    MutantChild[i] = MutantChild[i] + RandomNumber.NextDouble() - 0.5;
                }
            }
            return MutantChild;
        }

        /// <summary>
        /// Генетический алгоритм для вещественного кодирования.
        /// </summary>
        public static Vectors GeneticAlgoritmReal(Population population)
        {
            int k = 0;
            List<Vectors> TemporaryPopulation = new List<Vectors>(); /// Временная популяция.
            Population ControlPopulation = population; /// Популяция родителей/отобранных особей
            while (k++ < MaximumIterations)
            {
                ControlPopulation = ControlPopulation.GetParentPool(TournamentSize);
                //if (population.Max.FitnessFunction - population.Min.FitnessFunction < 0.01) MessageBox.Show("Популяция выродилась!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                while (TemporaryPopulation.Count != ControlPopulation.Count)
                {
                    foreach (Vectors item in Crossing(population.RandomSelection, population.RandomSelection))
                    {
                        TemporaryPopulation.Add(item);
                    }
                }

                ControlPopulation = new Population(TemporaryPopulation);
                TemporaryPopulation.Clear();
            }
            Vectors min = new Vectors(ControlPopulation.Min);
            ControlPopulation = null;
            return min;
        }

        /// <summary>
        /// Генетический алгоритм, для целочисленного кодирования.
        /// </summary>
        public static Vectors GeneticAlgoritmInteger(Population population)
        {
            int k = 0;
            List<Vectors> TemporaryPopulation = new List<Vectors>(); /// Временная популяция.
            Population ControlPopulation = population; /// Популяция родителей/отобранных особей
            while (k++ < MaximumIterations)
            {
                ControlPopulation = ControlPopulation.GetParentPool(TournamentSize);
                //if (population.Max.FitnessFunction - population.Min.FitnessFunction < 0.01) MessageBox.Show("Популяция выродилась!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                while (TemporaryPopulation.Count != ControlPopulation.Count)
                {
                    foreach (var item in Crossing(ControlPopulation.RandomSelection, ControlPopulation.RandomSelection))
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

        public static void AssignedSetting()
        {

        }

        /// <summary>
        /// Вспомогательная функция, для выбора нужного кодирования и метода скрещивания.
        /// </summary>
        public static void AssignedDelegat(int typeGA, int typeCrossing)
        {
            switch (typeGA)
            {
                case 0:

                    GeneticAlgoritm = GeneticAlgoritmReal;
                    switch (typeCrossing)
                    {
                        case 0: Crossing = CrossingRealOnePoint; break;
                        case 1: Crossing = CrossingBLXalpha; break;
                    }
                    break;


                case 1:
                    GeneticAlgoritm = GeneticAlgoritmInteger;
                    switch (typeCrossing)
                    {
                        case 0: Crossing = CrossingIntegerOnePoint; break;
                        case 1: Crossing = CrossingIntegerTwoPoint; break;
                        case 2: Crossing = CrossingIntegerUniform; break;
                    }
                    break;
            }
        }

        public static void defaultSetting()
        {
            CrossingProbability = 0.7;
            MutationProbability = 0.1;
            InversionProbability = 0.05;
            TournamentSize = 2;
            MaximumIterations = 100;
            BLXaRate = 0.5;
            SizePopulation = 100;
        }

        public static Vectors mainGeneticAlgoritm()
        {
            Vectors result = GeneticAlgoritm(new Population(SizePopulation));
            return result;
        }

        /// <summary>
        /// Свойство, которое возвращает true если мутация включена.
        /// </summary>
        public static bool onMutation => (MutationProbability > 0) ? true : false;
        /// <summary>
        /// Свойство, которое возвращает true если инверсия включена.
        /// </summary>
        public static bool onInversion => (InversionProbability > 0) ? true : false;
    }
}
