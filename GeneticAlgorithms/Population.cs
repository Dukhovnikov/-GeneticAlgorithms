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
    /// Класс реализующий популяцию, как набор особей.
    /// </summary>
    class Population
    {
        /// <summary>
        /// Класс реализующий популяцию, как набор особей.
        /// </summary>
        private List<Vector> population { get; set; }

        /// <summary>
        /// Переменная для генерации случайного числа из заданного промежутка.
        /// </summary>
        static Random RandomNumber = new Random();

        /// <summary>
        /// Количество особей в популяции.
        /// </summary>
        private int Value { get; }

        /// <summary>
        /// Возвращает или задает значение отдельной особи.
        /// </summary>
        public Vector this[int index]
        {
            get { return population[index]; }
            set { population[index] = value; }
        }

        /// <summary>
        /// Конструктор, генерирующий случайную популяцию заданного размера.
        /// </summary>
        public Population(Vector StartPoint, Vector EndPoint, int SizePopulation, int ChromosomeCount)
        {
            размер = РазмерПопуляции;
            популяция = new Особь[РазмерПопуляции];
            double delta = КонецПромежутка - НачалоПромежутка;
            for (int i = 0; i < РазмерПопуляции; i++)
            {
                популяция[i] = new Особь(КоличествоХромосом);
                for (int j = 0; j < КоличествоХромосом; j++)
                {
                    популяция[i][j] = RandomNumber.NextDouble() * (КонецПромежутка - НачалоПромежутка) + НачалоПромежутка;
                }
            }
        }
    }
}
