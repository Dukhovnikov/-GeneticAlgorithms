using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using myMatrix;
using myVector;
using System.Collections;

namespace GeneticAlgorithms
{
    /// <summary>
    /// Класс реализующий популяцию, как набор особей.
    /// </summary>
    class Population
    {
        /// <summary>
        /// Количество особей в популяции.
        /// </summary>
        public readonly int AmountIndivid;

        /// <summary>
        /// Класс реализующий популяцию, как набор особей.
        /// </summary>
        private List<Vector> population { get; set; }

        /// <summary>
        /// Получает число особей, содержащихся в популяции.
        /// </summary>
        public int Count { get { return population.Count; } }

        /// <summary>
        /// Возвращает особь с минимальным значением.
        /// </summary>
        public Vector Min { get { return population.Min(); } }

        /// <summary>
        /// Возвращает случайную особь из популяции.
        /// </summary>
        public Vector RandomSelection { get { return this[RandomNumber.Next(0, Count)]; } }

        /// <summary>
        /// Переменная для генерации случайного числа из заданного промежутка.
        /// </summary>
        static Random RandomNumber = new Random();


        /// <summary>
        /// Возвращает или задает значение отдельной особи.
        /// </summary>
        public Vector this[int index]
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
        public Population(List<Vector> population)
        {
            this.population = new List<Vector>(population);
        }

        /// <summary>
        /// Конструктор, генерирующий случайную популяцию заданного размера.
        /// </summary>
        public Population(Vector StartPoint, Vector EndPoint, int SizePopulation)
        {
            AmountIndivid = SizePopulation;
            for (int i = 0; i < SizePopulation; i++)
            {
                population.Add(new Vector(StartPoint.Size));
                for (int j = 0; j < StartPoint.Size; j++)
                {
                    population[i][j] = RandomNumber.NextDouble() * (EndPoint[j] - StartPoint[j]) + StartPoint[j];
                }
            }
        }

        /// <summary>
        /// Турнирная селекция.
        /// </summary>
        /// <param name="TournamentSize">Размер турнира.</param>
        /// <returns></returns>
        private Vector TournamentSelection(int TournamentSize)
        {
            Vector[] SelectedIndividuals = new Vector[TournamentSize]; /// Массив, который хранит отобранные особи.
            for (int i = 0; i < TournamentSize; i++)
            {
                SelectedIndividuals[i] = population[RandomNumber.Next(0, AmountIndivid)];
            }
            return SelectedIndividuals.Min();
        }

        /// <summary>
        /// Функция, которая возвращает родительский отобранный из текущей популяции. В качестве аргумента принимается размер турнира.
        /// </summary>
        public Population GetParentPool(int TournamentSize)
        {
            List<Vector> ParentPool = new List<Vector>();
            for (int i = 0; i < AmountIndivid; i++)
            {
                ParentPool.Add(TournamentSelection(TournamentSize));
            }
            return new Population(ParentPool);
        }

        /// <summary>
        /// Итератор, выполняющий перебор элементов популяции.
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < AmountIndivid; i++)
            {
                yield return (this[i]);
            }
        }

    }
}
