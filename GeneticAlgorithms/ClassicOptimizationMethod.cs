using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using myVector;
using myMatrix;
using Laboratory;
namespace GeneticAlgorithms
{
    class ClassicOptimizationMethod
    {
        /// <summary>
        /// Переменная для выполения одномерной минимизациию
        /// </summary>
        LaboratoryWork getAlpha { get; set; }
        /// <summary>
        /// Стартовая точка.
        /// </summary>
        Vectors x { get; set; }
        /// <summary>
        /// Исследуемая функция.
        /// </summary>
        Delegate function { get; set; }


        /// <summary>
        /// Точность локализации минимума для многомерного поиска.
        /// </summary>
        double eps { get; set; } = 1e-6;


        /// <summary>
        /// Ограничение количества итераций
        /// </summary>
        int m = 500;


        /// <summary>
        /// Конструктор класса, для задания начальных параметров.
        /// </summary>
        public ClassicOptimizationMethod(Delegate function, Vectors vector)
        {
            this.function = function;
            x = new Vectors(vector);
            A = new Matrix(x.Size);
            getAlpha = new LaboratoryWork(function, x.Size);
        }


        #region Реализация Флетчера-Ривса
        /// <summary>
        ///Текущее значение функции.
        /// </summary>
        double y(Vectors x)
        {
            return (double)function.DynamicInvoke(x.vector); /// Возвращает значение динамической функции уравнения
        }

        /// <summary>
        /// Значение производной в текущей точке. Вторая формула численного дифференцирования.
        /// </summary>
        Vectors grad(Vectors x)
        {
            Vectors g = new Vectors(x.Size);
            double h = 1e-5;
            for (int i = 0; i < x.Size; i++)
            {
                double[] hi = new double[x.Size];
                hi[i] = h;
                g[i] = (y(x + new Vectors(hi)) - y(x - new Vectors(hi))) / (2 * h);
            }
            return g;
        }

        /// <summary>
        ///значение бета по формуле Флетчера-Ривса.
        /// </summary>
        double betaPhitchersRivers(Vectors g2, Vectors g1)
        {
            return (Math.Pow(g2.Norma, 2)) / (Math.Pow(g1.Norma, 2));
        }

        /// <summary>
        /// Нахождение минимума с помощью одного из четырех методов сопряженных градиентов. 
        /// </summary>
        public Vectors getOptimizeConjugateGradient()
        {
            int k = 1;
            int j = 0;
            Vectors p = new Vectors(x.Size);
            int N = x.Size;
            double alpha;
            do
            {
                if (k == j * N + 1)
                {
                    j++;
                    p = -grad(x);
                }
                else
                {
                    p = -grad(x) + betaPhitchersRivers(grad(x), (-p)) * p;
                }
                alpha = getAlpha.getAlphaZS1(x, p);
                x = x + alpha * p;
                k++;
            }
            while (grad(x).Norma > eps && k < m);
            return x;
        }
        #endregion
        #region Реализация метода БФГШ
        /// <summary>
        /// Матрица переменной метрики.
        /// </summary>
        Matrix A { get; set; }


        /// <summary>
        /// Формула переменной метрики Бройдена–Флетчера–Гольдфарба–Шенно.
        /// </summary>
        Matrix BFGSH(Vectors x2, Vectors x1)
        {
            Vectors gamma = grad(x2) - grad(x1);
            Vectors S = A * gamma;
            Vectors deltaX = x2 - x1;
            return A + (1 + (S * gamma) / (gamma * deltaX)) * ((Vectors.Multiplication(deltaX, deltaX)) / (deltaX * gamma)) - ((Vectors.Multiplication(deltaX, gamma) + Vectors.Multiplication(gamma, deltaX)) / (deltaX * gamma));
        }


        /// <summary>
        /// Квазиньютоновский алгоритм.
        /// </summary>
        public Vectors getOptimizeBFGSH()
        {
            int k = 1;
            int j = 0;
            int N = x.Size;
            Vectors x1 = x; // Предыдущая точка
            Vectors p = new Vectors(x.Size);
            while (grad(x).Norma > eps && k < m)
            {
                if (k == j * N + 1) { j++; A = Matrix.E(x.Size); } // Шаг 1 Построим корректирующую матрицу
                else { A = BFGSH(x, x1); }
                p = -1 * (A * grad(x)); // Шаг 2 Построим квазиньютоновское направление
                x1 = x;
                x = x + getAlpha.getAlphaZS1(x, p) * p; // Шаг 3 Переход в новую точку
                k++;
            }
            return x;
        }
        #endregion
    }
}
