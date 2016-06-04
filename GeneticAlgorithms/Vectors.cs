using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithms
{
    /// <summary>
    /// Обобщенный класс - вектор.
    /// </summary>
    class Vectors<T>
    {
        /// <summary>
        /// Исследуемая функция в виде лямбда выражения.
        /// </summary>
        public static Delegate Function { get; set; }

        /// <summary>
        /// Приспособленность особи / Значение функции в точке.
        /// </summary>
        public double FitnessFunction { get { return (double)Function.DynamicInvoke(vector); } }

        /// <summary>
        /// Вектор.
        /// </summary>
        public T[] vector { get; }

        /// <summary>
        /// Значение ячейки вектора.
        /// </summary>
        public T this[int index]
        {
            get { return vector[index]; } /// Аксессор для получения данных
            set { vector[index] = value; } /// Аксессор для установки данных
        }

        /// <summary>
        ///Норма вектора.
        /// </summary>
        public double Norma
        {
            get
            {
                double sum = 0;
                for (int i = 0; i < Size; i++) sum += Math.Pow(Convert.ToDouble(this[i]), 2);
                return Math.Sqrt(sum);
            }
        }

        /// <summary>
        /// Длина вектора.
        /// </summary>
        public int Size { get { return vector.Length; } }

        /// <summary>
        /// Строковое представление вектора.
        /// </summary>
        public override string ToString()
        {
            string vector = "";
            for (int i = 0; i < Size - 1; i++) vector += this[i] + " : ";
            vector += this[Size - 1];
            return vector;
        }


        /// <summary>
        /// Пустой вектор, размерностью i.
        /// </summary>
        public Vectors(int i)
        {
            vector = new T[i];
            for (int j = 0; j < i; j++) vector[j] = default(T);
        }

        /// <summary>
        /// Заполнение вектора путем передачи массива.
        /// </summary>
        public Vectors(params T[] vector)
        {
            this.vector = new T[vector.Length];
            this.vector = vector;
        }

        /// <summary>
        /// Коснтруктор, инициализирущий вектор посредством другого вектора.
        /// </summary>
        public Vectors(Vectors<T> vector)
        {
            for (int i = 0; i < vector.Size; i++)
            {
                this.vector[i] = vector[i];
            }
        }

        /// <summary>
        /// Операция разности векоторв.
        /// </summary>
        public static Vectors<double> operator -(Vectors<T> v1, Vectors<T> v2)
        {
            Vectors<double> v3 = new Vectors<double>(v1.Size);
            for (int i = 0; i < v1.Size; i++) v3[i] = Convert.ToDouble(v1[i]) - Convert.ToDouble(v2[i]);
            return v3;
        }

        /// <summary>
        /// Операция умножения вектора на вектор.
        /// </summary>
        public static double operator *(Vectors<T> v1, Vectors<T> v2)
        {
            double v3 = 0;
            for (int i = 0; i < v1.Size; i++) v3 += Convert.ToDouble(v1[i]) * Convert.ToDouble(v2[i]);
            return v3;
        }

        /// <summary>
        /// Инвертируем значения полей вектора.
        /// </summary>
        public static Vectors<T> operator -(Vectors<T> v1)
        {
            for (int i = 0; i < v1.Size; i++) v1[i] *= -1;
            return v1;
        }
    }
}
