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
    class Population
    {
        static List<int> test = new List<int>();
        /// <summary>
        /// Класс реализующий популяцию, как набор особей.
        /// </summary>
        private List<iVector> population { get; set; }

        /// <summary>
        /// Получает число особей, содержащихся в популяции.
        /// </summary>
        public int Count { get { return population.Count; } }

        /// <summary>
        /// Возвращает особь с минимальным значением.
        /// </summary>
        public iVector Min { get { return population.Min(); } }

        /// <summary>
        /// Возвращает особь с максимальным значением.
        /// </summary>
        public iVector Max { get { return population.Max(); } }

        /// <summary>
        /// Возвращает случайную особь из популяции.
        /// </summary>
        public iVector RandomSelection { get { return this[RandomNumber.Next(Count)]; } }

        /// <summary>
        /// Переменная для генерации случайного числа из заданного промежутка.
        /// </summary>
        public static Random RandomNumber = new Random();


        /// <summary>
        /// Возвращает или задает значение отдельной особи.
        /// </summary>
        public iVector this[int index]
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
        public Population(List<iVector> population)
        {
            this.population = new List<iVector>(population);
        }

        /// <summary>
        /// Конструктор, генерирующий случайную популяцию заданного размера.
        /// </summary>
        public Population(iVector StartPoint, iVector EndPoint, int SizePopulation)
        {
            population = new List<iVector>();
            for (int i = 0; i < SizePopulation; i++)
            {
                iVector individ = new iVector(StartPoint.Size);
                for (int j = 0; j < StartPoint.Size; j++)
                {
                    individ[j] = RandomNumber.NextDouble() * (EndPoint[j] - StartPoint[j]) + StartPoint[j];
                }
                population.Add(individ);
            }
        }

        /// <summary>
        /// Турнирная селекция.
        /// </summary>
        /// <param name="TournamentSize">Размер турнира.</param>
        /// <returns></returns>
        private iVector TournamentSelection(int TournamentSize)
        {
            iVector[] SelectedIndividuals = new iVector[TournamentSize]; /// Массив, который хранит отобранные особи.
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
            List<iVector> ParentPool = new List<iVector>();
            for (int i = 0; i < Count; i++)
            {
                //ParentPool.Add(TournamentSelection(TournamentSize));
                ParentPool.Add(new iVector((TournamentSelection(TournamentSize)));
            }
            return new Population(ParentPool);
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
