using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithms
{
    /// <summary>
    /// Класс реализующий популяцию, как набор особей.
    /// </summary>
    class Population
    {
        /// <summary>
        /// Класс реализующий популяцию, как набор особей.
        /// </summary>
        private List<Individual> population { get; set; }

        /// <summary>
        /// Переменная для генерации случайного числа из заданного промежутка.
        /// </summary>
        static Random RandomNumber = new Random();

        /// <summary>
        /// Возвращает или задает значение отдельной особи.
        /// </summary>
        public Individual this[int index]
        {
            get { return population[index]; }
            set { population[index] = value; }
        }
    }
}
