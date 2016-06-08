using System;
using myVector;
namespace Laboratory
{
    /// <summary>
    /// Класс для нахождения Alpha в заданном направлении
    /// </summary>
    class LaboratoryWork
    {
        /// <summary>
        /// Начальная точка
        /// </summary>
        private Vectors _x;


        /// <summary>
        /// Переменная точка. Точка минимума
        /// </summary>
        private Vectors x;


        /// <summary>
        /// Направляющий вектор (антиградиент)
        /// </summary>
        private Vectors p;


        /// <summary>
        /// Шаг в трехмерном пространстве
        /// </summary>
        private double alpha;


        /// <summary>
        /// Начало промежутка
        /// </summary>
        private double a;


        /// <summary>
        /// Конец промежутка
        /// </summary>
        private double b;


        /// <summary>
        /// Максимальное количество итераций
        /// </summary>
        int maxK;


        /// <summary>
        /// Точность поиска
        /// </summary>
        private double eps = 1e-15;


        /// <summary>
        /// Первая точка соотношения золотого сечения 
        /// </summary>
        private static double t1 = (3 - Math.Sqrt(5)) / 2;


        /// <summary>
        /// Вторая точка соотношения золотого сечения 
        /// </summary>   
        private static double t2 = (Math.Sqrt(5) - 1) / 2;


        /// <summary>
        /// Заданная фунция 
        /// </summary>       
        Delegate function;


        /// <summary>
        /// Значение функции в точке
        /// </summary>
        private double y(Vectors x)
        {
            return (double)function.DynamicInvoke(x.vector);
        }


        /// <summary>
        /// Переход к следующей точке
        /// </summary>
        private void point(double alpha)
        {
            x = _x + alpha * p;
        }

        /// <summary>
        /// Производная в точке по направлению
        /// </summary>
        private double dy(double alpha)
        {
            point(alpha);
            return FirstDerivativeSecond * p;
        }


        /// <summary>
        /// Первая формула численного дифференцирования
        /// </summary>
        private Vectors firstderivativefirst()
        {
            Vectors g = new Vectors(x.Size);
            double h = 1e-5;
            for (int i = 0; i < x.Size; i++)
            {
                double[] hi = new double[x.Size];
                hi[i] = h;
                g[i] = (y(x + new Vectors(hi)) - y(x)) / h;
            }
            return g;
        }


        /// <summary>
        /// Вторая формула численного дифференцирования
        /// </summary>
        private Vectors firstderivativesecond()
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
        /// Четвертая формула численного дифференцирования
        /// </summary>
        private Vectors firstderivativefourth()
        {
            Vectors g = new Vectors(x.Size);
            double h = 1e-5;
            for (int i = 0; i < x.Size; i++)
            {
                double[] hi = new double[x.Size];
                double[] hi2 = new double[x.Size];
                hi[i] = h;
                hi2[i] = 2 * h;
                g[i] = (-y(x + new Vectors(hi2)) + 8 * y(x + new Vectors(hi)) - 8 * y(x - new Vectors(hi)) + y(x - new Vectors(hi2))) / (12 * h);
            }
            return g;
        }


        /// <summary>
        /// Значение функции в новой точке
        /// </summary>
        private double f(double alpha)
        {
            point(alpha);
            return y(x);
        }


        /// <summary>
        /// Метод Свенн-1 
        /// </summary>
        private void sven1()
        {
            int k = 1;
            alpha = a = b = 0;
            double h = 1e-2;                         //Шаг в 2 - х мерном пространстве
            if (f(alpha) < f(alpha + h)) h = -h;
            while (f(alpha) > f(alpha + h))
            {
                alpha += h;
                h = 2 * h;
                k++;
            }

            if (h > 0)
            {
                a = alpha - 0.75 * h;
                b = alpha + h;
            }
            else
            {
                a = alpha + h;
                b = alpha - 0.75 * h;
            }
        }


        /// <summary>
        /// Метод Свенна-4 
        /// </summary>
        private void sven4()
        {
            int k = 1;
            alpha = a = b = 0;
            double h = 1e-2;                         //Шаг в 2 - х мерном пространстве
            if (f(alpha) < f(alpha + h)) h = -h;
            while (f(alpha) < f(alpha - 0.5 * h))
            {
                alpha += h;
                h = 2 * h;
                k++;
            }

            if (h > 0)
            {
                a = alpha - 0.75 * h;
                b = alpha;
            }
            else
            {
                a = alpha;
                b = alpha - 0.75 * h;
            }
        }

        /// <summary>
        /// Метод Свен-1
        /// </summary>
        private void swan1()
        {
            double h = 1e-2;
            if (f(alpha) < f(alpha + h)) h = -h;
            while (f(alpha) > f(alpha + h))
            {
                alpha += h;
                h *= 2;
            }
            if (h < 0)
            {
                a = alpha + h;
                b = alpha - h / 2;
            }
            else
            {
                a = alpha - h / 2;
                b = alpha + h;
            }
        }

        /// <summary>
        /// Метод - Больцано 
        /// </summary>
        private void bolzano()
        {
            int k = 1;
            while ((dy((a + b) / 2) > eps) && (Math.Abs(b - a) > eps) || (k <= 10))
            {
                if (dy((a + b) / 2) > 0) b = (a + b) / 2.0;
                else a = (a + b) / 2.0;
                k++;
            }
        }


        /// <summary>
        /// Точка аппроксимации
        /// </summary>
        private double d()
        {
            double z, w, Y;
            try
            {
                z = dy(a) + dy(b) + 3 * (f(a) - f(b)) / (b - a);
                w = Math.Sqrt(z * z - dy(a) * dy(b));
                Y = (z + w - dy(a)) / (dy(b) - dy(a) + 2 * w);
            }
            catch (DivideByZeroException ex)
            {
                throw new DivideByZeroException(ex.Message);
            }
            return a + Y * (b - a);
        }


        /// <summary>
        /// Метод Давидона 
        /// </summary>
        private double bolzano_davidon()
        {
            bolzano();
            int k = 1;
            double d = this.d();
            while (dy(d) > eps && k < maxK)
            {
                if (dy(d) < 0)
                {
                    a = d;
                }
                else
                {
                    b = d;
                }
                k++;
                d = this.d();
            }
            return d;
        }


        /// <summary>
        /// Параметр гамма
        /// </summary>
        private double Y()
        {
            double z, w;
            z = dy(a) + dy(b) + 3 * (f(a) - f(b)) / (b - a);
            w = Math.Sqrt(z * z - dy(a) * dy(b));
            return (z + w - dy(a)) / (dy(b) - dy(a) + 2 * w);
        }


        /// <summary>
        /// Метод золотого сечения - 1
        /// </summary>
        double ZS1()
        {
            int k = 1;
            double a1 = a + t1 * Math.Abs(a - b);
            double b1 = a + t2 * Math.Abs(a - b);
            while (Math.Abs(a - b) > eps && k < maxK)
            {
                if (f(a1) < f(b1))
                {
                    b = b1;
                    b1 = a1;
                    a1 = a + t1 * Math.Abs(a - b);
                }
                else
                {
                    a = a1;
                    a1 = b1;
                    b1 = a + t2 * Math.Abs(a - b);
                }
                k++;
            }
            return (a + b) / 2;
        }

        /// <summary>
        /// Метод золотого ечения - 2
        /// </summary>
        double ZS2()
        {
            int k = 1;
            double a1, b1;
            a1 = a + t2 * Math.Abs(a - b);
            while (eps < Math.Abs(a - b) && k < maxK)
            {
                b1 = a + b - a1;
                if (a1 < b1)
                {
                    if (f(a1) < f(b1))
                        b = b1;
                    else
                    {
                        a = a1;
                        a1 = b1;
                    }
                }
                else
                {
                    if (f(a1) < f(b1))
                        a = b1;
                    else
                    {
                        b = a1;
                        a1 = b1;
                    }
                }
                k++;
            }
            return ((a + b) / 2);
        }

        /// <summary>
        /// Метод дихотомии
        /// </summary>
        double dichotomy()
        {
            double a1, b1;
            int k = 1;
            while (Math.Abs(a - b) > eps && k < maxK)
            {
                a1 = ((a + b) / 2) - eps;
                b1 = ((a + b) / 2) + eps;
                if (f(a1) < f(b1)) b = b1;
                else a = a1;
                k++;
            }
            return (a + b) / 2;
        }


        /// <summary>
        /// Метод Кубической интерполяции
        /// </summary>
        double cubic()
        {
            double min; // Аппроксимирующий минимум
            double gamma; // Значние параметра гамма
            int k = 1;
            do
            {
                gamma = Gamma;
                if (gamma < 0)
                {
                    min = a;
                }
                else
                {
                    if (gamma > 1)
                    {
                        min = b;
                    }
                    else
                    {
                        min = a + gamma * (b - a);
                    }
                }
                if (dy(min) < eps || min == a || min == b)
                {
                    return min;
                }
                else
                {
                    if (k > maxK) return min;
                    if (dy(min) > 0) b = min;
                    else a = min;
                    k++;
                }
            }
            while (true);
        }


        /// <summary>
        /// Метод Пауэлла
        /// </summary>
        double pauell()
        {
            double c = b;
            bool flag = false;
            double d; // Апроксимирующий минимум
            b = (a + b) / 2;
            int k = 1;
            d = (1.0 / 2.0) * (f(a) * (b * b - c * c) + f(b) * (c * c - a * a) + f(c) * (a * a - b * b)) / (f(a) * (b - c) + f(b) * (c - a) + f(c) * (a - b));
            while ((Math.Abs((b - d) / b) > eps) && (k < 50))
            {
                if (k != 1)
                    d = (a + b) / 2 + 1 / 2 * ((f(a) - f(b)) * (f(a) - f(b)) * (b - c) * (c - a)) / (f(a) * (b - c) + f(b) * (c - a) + f(c) * (a - b));
                else
                {
                    d = (1.0 / 2.0) * (f(a) * (b * b - c * c) + f(b) * (c * c - a * a) + f(c) * (a * a - b * b)) / (f(a) * (b - c) + f(b) * (c - a) + f(c) * (a - b));
                }
                if (b < d)
                {
                    if (f(b) < f(d))
                    {
                        c = d;
                        flag = true;
                    }
                    else
                    {
                        a = b;
                        b = d;
                    }
                }
                else
                {
                    if (f(b) < f(d))
                    {
                        a = d;
                        flag = true;
                    }
                    else
                    {
                        c = b;
                        b = d;
                    }
                }
                k++;
                if (flag)
                {
                    flag = false;
                    b = (a + c) / 2;
                }
            }
            return (b + d) / 2;
        }

        /// <summary>
        /// Стандартый конструктор
        /// </summary>
        public LaboratoryWork()
        {

        }


        /// <summary>
        /// Инициализирование функции и размерности векторов
        /// </summary>
        public LaboratoryWork(Delegate function, int value, int maxK = 150, double eps = 1e-15)
        {
            this.function = function;
            _x = new Vectors(value);
            p = new Vectors(value);
            this.maxK = maxK;
            this.eps = eps;
        }


        /// <summary>
        /// Получить альфа методом Больцан-Давидон
        /// </summary>
        public double getAlphaBolzanoDavidon(Vectors x, Vectors p)
        {
            this.x = new Vectors(x.Size);
            this._x = x;
            this.p = p;
            sven4();
            //swan1();
            return BolzanoDavidon;
        }


        /// <summary>
        /// Получить альфа методом Дихотомии
        /// </summary>
        public double getAlphaDihotomy(Vectors x, Vectors p)
        {
            this.x = new Vectors(x.Size);
            this._x = x;
            this.p = p;
            sven4();
            return Dihotomy;
        }


        /// <summary>
        /// Получить альфа методом Золотого сечения - 1
        /// </summary>
        public double getAlphaZS1(Vectors x, Vectors p)
        {
            this.x = new Vectors(x.Size);
            this._x = x;
            this.p = p;
            sven4();
            //swan1();
            return GoldenSection;
        }

        /// <summary>
        /// Получить альфа методом Золотого сечения - 2
        /// </summary>
        public double getAlphaZS2(Vectors x, Vectors p)
        {
            this.x = new Vectors(x.Size);
            this._x = x;
            this.p = p;
            sven4();
            return ZS2();
        }

        /// <summary>
        /// Получить альфа методом Кубической интерполяции
        /// </summary>
        public double getAlphaCubic(Vectors x, Vectors p)
        {
            this.x = new Vectors(x.Size);
            this._x = x;
            this.p = p;
            sven4();
            return CubicIntepolyatsiya;
        }


        /// <summary>
        /// Получить альфа методом больцано
        /// </summary>
        public double getAlphaBolzano(Vectors x, Vectors p)
        {
            this.x = new Vectors(x.Size);
            this._x = x;
            this.p = p;
            sven4();

            int k = 1;
            while ((dy((a + b) / 2) > eps) && (Math.Abs(b - a) > eps))
            {
                if (dy((a + b) / 2) > 0) b = (a + b) / 2.0;
                else a = (a + b) / 2.0;
                k++;
            }
            return (a + b) / 2;
        }


        /// <summary>
        /// Получить альфа методом Пауелл
        /// </summary>
        public double getAlphaPauell(Vectors x, Vectors p)
        {
            this.x = new Vectors(x.Size);
            this._x = x;
            this.p = p;
            //sven4();
            swan1();
            return Pauell;
        }

        /// <summary>
        /// Параметр гамма. Используется в методу пауелл
        /// </summary>
        private double Gamma { get { return Y(); } }
        /// <summary>
        /// Дихотомия
        /// </summary>
        private double Dihotomy { get { return dichotomy(); } }
        /// <summary>
        /// Золотое сечение - 1
        /// </summary>
        private double GoldenSection { get { return ZS1(); } }
        /// <summary>
        /// Кубическая интерполяция
        /// </summary>
        private double CubicIntepolyatsiya { get { return cubic(); } }
        /// <summary>
        /// Больцано - Давидон
        /// </summary>
        private double BolzanoDavidon { get { return bolzano_davidon(); } }
        /// <summary>
        /// Пауелл
        /// </summary>
        private double Pauell { get { return pauell(); } }
        /// <summary>
        /// Первая формула численного дифференцирования
        /// </summary>
        private Vectors FirstDerivativeFirst { get { return firstderivativefirst(); } }
        /// <summary>
        /// Вторая формула численного дифференцирования
        /// </summary>
        private Vectors FirstDerivativeSecond { get { return firstderivativesecond(); } }
        /// <summary>
        /// Четвертая формула численного дифференцирования
        /// </summary>
        private Vectors FirstDerivativeFourth { get { return firstderivativefourth(); } }
    }
}
