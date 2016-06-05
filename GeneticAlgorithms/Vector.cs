using System;
using myMatrix;
using System.Windows;
using System.Collections;
using GeneticAlgorithms;
namespace myVector
{
    /// <summary>
    /// Вид кодирования.
    /// </summary>
    public enum Coding
    {
        /// <summary>
        /// Целочисленное.
        /// </summary>
        Integer,
        /// <summary>
        /// Вещественное.
        /// </summary>
        Real
    }


    public class Vectors : IComparable<Vectors>
    {
        /// <summary>
        /// Реализация интерфейса IComparable, для сортировки векторов.
        /// </summary>
        public int CompareTo(Vectors other)
        {
            if (Function == null) throw new ArgumentException("Не зада исследуемая функция.");
            return FitnessFunction.CompareTo(other.FitnessFunction);
        }

        /// <summary>
        /// Итератор, выполняющий перебор элементов популяции.
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < Size; i++)
            {
                yield return (this[i]);
            }
        }

        /// <summary>
        /// Исследуемая функция в виде лямбда выражения.
        /// </summary>
        public static Delegate Function { get; set; }

        /// <summary>
        /// Приспособленность особи / Значение функции в точке.
        /// </summary>
        public double FitnessFunction
        {
            get
            {
                switch (CodingType)
                {
                    case Coding.Integer:
                        Vectors rezult = new Vectors(StartPoint.Size);

                        for (int i = 0; i < StartPoint.Size; i++)
                        {
                            rezult[i] = this[i] * (EndPoint[i] - StartPoint[i]) / (Math.Pow(2, BitsCount) - 1) + StartPoint[i];
                        }

                        return (double)Function.DynamicInvoke(rezult.vector);
                    case Coding.Real:
                        return (double)Function.DynamicInvoke(vector);
                    default:
                        throw new IndexOutOfRangeException("Не задан тип кодирования для особи.");
                }
            }
        }

        /// <summary>
        /// Начальная точка.
        /// </summary>
        public static Vectors StartPoint { get; set; }

        /// <summary>
        /// Конечная точка.
        /// </summary>
        public static Vectors EndPoint { get; set; }

        /// <summary>
        /// Количество разрядов для целочисленной кодировки.
        /// </summary>
        public static byte BitsCount { get; set; }

        /// <summary>
        /// Тип кодирования.
        /// </summary>
        public static Coding CodingType { get; set; } = Coding.Real;


        /// <summary>
        /// Вектор.
        /// </summary>
        public double[] vector { get; }


        /// <summary>
        /// Пустой вектор, размерностью i.
        /// </summary>
        public Vectors(int i)
        {
            vector = new double[i];
            for (int j = 0; j < i; j++) vector[j] = 0;
        }


        /// <summary>
        /// Заполнение вектора путем передачи массива.
        /// </summary>
        public Vectors(params double[] vector)
        {
            this.vector = new double[vector.Length];
            this.vector = vector;
        }

        /// <summary>
        /// Коснтруктор, инициализирущий вектор посредством другого вектора.
        /// </summary>
        public Vectors(Vectors vector)
        {
            this.vector = new double[vector.Size];
            for (int i = 0; i < vector.Size; i++)
            {
                this.vector[i] = vector[i];
            }
        }


        /// <summary>
        /// Операция разности векоторв.
        /// </summary>
        public static Vectors operator -(Vectors v1, Vectors v2) 
        {
            Vectors v3 = new Vectors(v1.Size);
            for (int i = 0; i < v1.Size; i++) v3[i] = v1[i] - v2[i];
            return v3;
        }


        /// <summary>
        /// Инвертируем значения полей вектора.
        /// </summary>
        public static Vectors operator -(Vectors v1) 
        {
            for (int i = 0; i < v1.Size; i++) v1[i] *= -1;
            return v1;
        }


        /// <summary>
        /// Операция сложения векторов.
        /// </summary>
        public static Vectors operator +(Vectors v1, Vectors v2) 
        {
            Vectors v3 = new Vectors(v1.Size);
            for (int i = 0; i < v1.Size; i++) v3.vector[i] = v1.vector[i] + v2.vector[i];
            return v3;
        }

        /// <summary>
        /// Сложение числа с вектором.
        /// </summary>
        public static Vectors operator +(double C, Vectors v1)
        {
            Vectors v3 = new Vectors(v1.Size);
            for (int i = 0; i < v1.Size; i++) v3.vector[i] = C + v1.vector[i];
            return v3;
        }


        /// <summary>
        /// Операция умножения числа на вектор.
        /// </summary>
        public static Vectors operator *(double C, Vectors v1)
        {
            Vectors v3 = new Vectors(v1.Size);
            for (int i = 0; i < v1.Size; i++) v3.vector[i] = C * v1.vector[i];
            return v3; 
        }


        /// <summary>
        /// Операция умножения вектора на вектор.
        /// </summary>
        public static double operator *(Vectors v1, Vectors v2)
        {
            double v3 = 0;
            for (int i = 0; i < v1.Size; i++) v3 += v1[i] * v2[i];
            return v3;
        }

        public static Vectors operator /(Vectors v1, double C)
        {
            Vectors v3 = new Vectors(v1.Size);
            for (int i = 0; i < v3.Size; i++) v3[i] = v1[i] / C;
            return v3;
        }

        /// <summary>
        /// Умножение вектора на транспонированный вектор.
        /// </summary>
        public static Matrix Multiplication(Vectors v1, Vectors v2)
        {
            Matrix M = new Matrix(v1.Size);
            if (v1.Size != v2.Size) throw new ArgumentException("Число столбцов матрицы А не равно числу строк матрицы В.");
            for (int i = 0; i < M.Size; i++)
                for (int j = 0; j < M.Size; j++)
                    M[i, j] = v1[i] * v2[j];
            return M;
        }

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

        public Vectors ToReal()
        {
            Vectors RealVector = new Vectors(Size);
            for (int i = 0; i < Size; i++)
            {
                RealVector[i] = this[i] * (EndPoint[i] - StartPoint[i]) / (Math.Pow(2, BitsCount) - 1) + StartPoint[i];
            }
            return RealVector;
        }

        /// <summary>
        ///Норма вектора.
        /// </summary>
        public double Norma
        {
            get
            {
                double sum = 0;
                for (int i = 0; i < Size; i++) sum += Math.Pow(this[i], 2);
                return Math.Sqrt(sum);
            }
        }
        /// <summary>
        /// Значение ячейки вектора.
        /// </summary>
        public double this[int index]
        {
            get { return vector[index]; } /// Аксессор для получения данных
            set { vector[index] = value; } /// Аксессор для установки данных
        }
        /// <summary>
        /// Длина вектора.
        /// </summary>
        public int Size { get { return vector.Length; } } 
    }
}
