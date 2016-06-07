using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using myMatrix;
using myVector;
using System.Collections;
using System.Windows;

namespace GeneticAlgorithms
{
    /// <summary>
    /// Класс реализующий популяцию, как набор особей.
    /// </summary>
    public class Population
    {
        /// <summary>
        /// Класс реализующий популяцию, как набор особей.
        /// </summary>
        private List<Vectors> population { get; set; }

        /// <summary>
        /// Получает число особей, содержащихся в популяции.
        /// </summary>
        public int Count { get { return population.Count; } }

        /// <summary>
        /// Возвращает особь с минимальным значением.
        /// </summary>
        public Vectors Min { get { return population.Min(); } }

        /// <summary>
        /// Возвращает особь с максимальным значением.
        /// </summary>
        public Vectors Max { get { return population.Max(); } }

        /// <summary>
        /// Возвращает случайную особь из популяции.
        /// </summary>
        public Vectors RandomSelection { get { return this[RandomNumber.Next(Count)]; } }

        /// <summary>
        /// Переменная для генерации случайного числа из заданного промежутка.
        /// </summary>
        public static Random RandomNumber = new Random();

        /// <summary>
        /// Возвращает или задает значение отдельной особи.
        /// </summary>
        public Vectors this[int index]
        {
            get { return population[index]; }
            set { population[index] = value; }
        }

        /// <summary>
        /// Стандартный коструктор, для общей логики.
        /// </summary>
        public Population()
        {

        }

        /// <summary>
        /// Конструктор, который инициализирует особи, принимая набор особей.
        /// </summary>
        public Population(List<Vectors> population)
        {
            this.population = new List<Vectors>(population);
        }

        /// <summary>
        /// Конструктор, генерирующий случайную популяцию заданного размера.
        /// </summary>
        public Population(int SizePopulation, Coding CodingType = Coding.Real)
        {
            population = new List<Vectors>();
            for (int i = 0; i < SizePopulation; i++)
            {
                Vectors individ = new Vectors(Vectors.StartPoint.Size);
                for (int j = 0; j < Vectors.StartPoint.Size; j++)
                {
                    individ[j] = RandomNumber.NextDouble() * (Vectors.EndPoint[j] - Vectors.StartPoint[j]) + Vectors.StartPoint[j];
                }
                population.Add(individ);
            }
        }

        /// <summary>
        /// Конструктор, генерирующий случайную популяцию заданного размера.
        /// </summary>
        public Population(int SizePopulation, Coding CodingType = Coding.Integer, int xz = 1)
        {
            population = new List<Vectors>();

            for (int i = 0; i < SizePopulation; i++)
            {
                Vectors individ = new Vectors(Vectors.StartPoint.Size);

                for (int j = 0; j < Vectors.StartPoint.Size; j++)
                {
                    individ[j] = RandomNumber.Next(1,1 << Vectors.BitsCount) - 1;
                }

                population.Add(individ);
            }

        }
        
        /// <summary>
        /// Умный коснтруктор, который создает популяцию с вещественным/целочисленным кодированием.
        /// </summary>
        public Population(int SizePopulation)
        {
            population = new List<Vectors>();
            
            switch (Vectors.CodingType)
            {
                /// Вещественное кодирование---------------------------------------------------------------------------------------------------
                case Coding.Real:
                    for (int i = 0; i < SizePopulation; i++)
                    {
                        Vectors individ = new Vectors(Vectors.StartPoint.Size);
                        for (int j = 0; j < Vectors.StartPoint.Size; j++)
                        {
                            individ[j] = RandomNumber.NextDouble() * (Vectors.EndPoint[j] - Vectors.StartPoint[j]) + Vectors.StartPoint[j];
                        }
                        population.Add(individ);
                    }
                        break;

                /// Целочисленное кодирование---------------------------------------------------------------------------------------------------
                case Coding.Integer:
                    for (int i = 0; i < SizePopulation; i++)
                    {
                        Vectors individ = new Vectors(Vectors.StartPoint.Size);
                        for (int j = 0; j < Vectors.StartPoint.Size; j++)
                        {
                            individ[j] = RandomNumber.Next(1 << Vectors.BitsCount);
                        }
                        population.Add(individ);
                    }
                        break;
            }
        } 

        /// <summary>
        /// Турнирная селекция.
        /// </summary>
        /// <param name="TournamentSize">Размер турнира.</param>
        /// <returns></returns>
        private Vectors TournamentSelection(int TournamentSize)
        {
            Vectors[] SelectedIndividuals = new Vectors[TournamentSize]; /// Массив, который хранит отобранные особи.
            for (int i = 0; i < TournamentSize; i++)
            {
                SelectedIndividuals[i] = population[RandomNumber.Next(Count)];
            }
            return SelectedIndividuals.Min();
        }

        /// <summary>
        /// Функция, которая возвращает родительский отобранный из текущей популяции. В качестве аргумента принимается размер турнира.
        /// </summary>
        public Population GetParentPool(int TournamentSize)
        {
            List<Vectors> ParentPool = new List<Vectors>();
            for (int i = 0; i < Count; i++)
            {
                ParentPool.Add(new Vectors((TournamentSelection(TournamentSize))));
            }
            return new Population(ParentPool);
        }

        /// <summary>
        /// Оператор популяционного всплеска популяции.
        /// </summary>
        public void PopulationSpike(double PartPopulation = 0.6)
        {
            int CountNewIndivid = Convert.ToInt32(PartPopulation*Count);
            Vectors individ;
            for (int i = CountNewIndivid; i > 0; i--)
            {
                switch (Vectors.CodingType)
                {
                    case Coding.Integer:
                        individ = new Vectors(Vectors.StartPoint.Size);
                        for (int j = 0; j < Vectors.StartPoint.Size; j++)
                        {
                            individ[j] = RandomNumber.Next(1 << Vectors.BitsCount);
                        }
                        this[RandomNumber.Next(Count)] = individ;
                        break;
                    case Coding.Real:
                        individ = new Vectors(Vectors.StartPoint.Size);
                        for (int j = 0; j < Vectors.StartPoint.Size; j++)
                        {
                            individ[j] = RandomNumber.NextDouble() * (Vectors.EndPoint[j] - Vectors.StartPoint[j]) + Vectors.StartPoint[j];
                        }
                        this[RandomNumber.Next(Count)] = individ;
                        break;
                }
            }
        }

        /// <summary>
        /// Итератор, выполняющий перебор элементов популяции.
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return (this[i]);
            }
        }

    }
}
