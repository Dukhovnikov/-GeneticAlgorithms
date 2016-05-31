using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithms
{
    /// <summary>
    /// Исскуственный класс представляющий собой отдельную особь.
    /// </summary>
    class Individual
    {
        /// <summary>
        /// Гены характеризующие генотип особи.
        /// </summary>
        private double[] gene { get; set; }

        /// <summary>
        /// Исследуемая функция в виде лямбда выражения.
        /// </summary>
        private static Delegate Function { get; set; }

        /// <summary>
        /// Приспособленность особи / Значение функции в точке.
        /// </summary>
        private double FitnessFunction { get { return (double)Function.DynamicInvoke(gene); } }

        /// <summary>
        /// Значение отдельного гена.
        /// </summary>
        public double this[int index]
        {
            get { return gene[index]; } /// Аксессор для получения данных
            set { gene[index] = value; } /// Аксессор для установки данных
        }

        /// <summary>
        /// Сравнивание двух особей.
        /// </summary>
        public static bool operator ==(Individual o1, Individual o2)
        {
            int k = 0;
            for (int i = 0; i < o1.Length; i++)
            {
                if (o1[i] == o2[i]) k++;
            }
            if (k == o1.Length) return true;
            else return false;
        }

        /// <summary>
        /// Сравнивание двух особей.
        /// </summary>
        public static bool operator !=(Individual o1, Individual o2)
        {
            int k = 0;
            for (int i = 0; i < o1.Length; i++)
            {
                if (o1[i] == o2[i]) k++;
            }
            if (k == o1.Length) return false;
            else return true;
        }

        /// <summary>
        /// Размер особи / Количество хромосом.
        /// </summary>
        public int Length { get { return gene.Length; } }

        /// <summary>
        /// Нулевая особь, размерностью i.
        /// </summary>
        public Individual (int i)
        {
            gene = new double[i];
            for (int j = 0; j < i; j++) { gene[i] = 0; }
        }
    }
}
