using System;
using myVector;
namespace myMatrix
{
    public class Matrix
    {
        /// <summary>
        /// Двумерный массив.
        /// </summary>
        private double[,] matrix;


        /// <summary>
        /// Задает пустую матрицу размером N.
        /// </summary>
        public Matrix(int n)
        {
            matrix = new double[n, n];
        }


        /// <summary>
        /// Задает матрицу с помощью двумерного массива.
        /// </summary>
        public Matrix(double[,] matrix)
        {
            this.matrix = matrix;
        }


        /// <summary>
        /// Получение значение ячейки матрицы.
        /// </summary>
        public double this[int row, int column]
        {
            get { return matrix[row, column]; } /// Аксессор для получения данных
            set { matrix[row, column] = value; } /// Аксессор для установки данных
        }


        /// <summary>
        /// Сложение матриц.
        /// </summary>
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            Matrix m3 = new Matrix(m1.Size);
            for (int i = 0; i < m3.Size; i++)
                for (int j = 0; j < m3.Size; j++)
                    m3[i, j] = m1[i, j] + m2[i, j];
            return m3;
        }


        /// <summary>
        /// Разность матриц.
        /// </summary>
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            Matrix m3 = new Matrix(m1.Size);
            for (int i = 0; i < m3.Size; i++)
                for (int j = 0; j < m3.Size; j++)
                    m3[i, j] = m1[i, j] - m2[i, j];
            return m3;
        }


        /// <summary>
        /// Умножение матрицы на число.
        /// </summary>
        public static Matrix operator *(double C, Matrix m1)
        {
            Matrix m3 = new Matrix(m1.Size);
            for (int i = 0; i < m3.Size; i++)
                for (int j = 0; j < m3.Size; j++)
                    m3[i, j] = C * m1[i, j];
            return m3;
        }


        /// <summary>
        /// Умножение матрицы на вектор.
        /// </summary>
        public static Vectors operator *(Matrix M, Vectors V)
        {
            if (M.Size != V.Size) throw new ArgumentException("Число столбцов матрицы А не равно числу элементов вектора В.");
            Vectors vector = new Vectors(V.Size);
            for (int i = 0; i < vector.Size; i++)
                for (int j = 0; j < vector.Size; j++)
                    vector[i] += M[i, j] * V[j];
            return vector;
        }

        /// <summary>
        /// Умножение матриц.
        /// </summary>
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            Matrix matrix = new Matrix(m1.Size);
            for (int i = 0; i < matrix.Size; i++)
                for (int j = 0; j < matrix.Size; j++)
                    for (int k = 0; k < matrix.Size; k++)
                        matrix[i, j] += m1[i, k] * m2[k, j];
            return matrix;
        }

        /// <summary>
        /// Деление матрицы на константу.
        /// </summary>
        public static Matrix operator /(Matrix M, double C)
        {
            Matrix matrix = new Matrix(M.Size);
            for (int i = 0; i < M.Size; i++)
                for (int j = 0; j < M.Size; j++)
                    matrix[i, j] = M[i, j] / C;
            return matrix;
        }

        /// <summary>
        /// Возвращает матрицу без указанных строки и столбца. Исходная матрица не изменяется.
        /// </summary>
        private Matrix Exclude(int row, int column)
        {
            if (row > Size || column > Size) throw new IndexOutOfRangeException("Строка или столбец не принадлежат матрице.");
            Matrix m1 = new Matrix(Size - 1);
            int X = 0;
            for (int i = 0; i < Size; i++)
            {
                int Y = 0;
                if (i == row) { X++; continue; }
                for ( int j = 0; j < Size; j++)
                {
                    if (j == column) { Y++; continue; }
                    m1[i - X, j - Y] = this[i, j];
                }
            }
            return m1;
        }

        /// <summary>
        /// Единичная матрица размером NxN.
        /// </summary>
        public static Matrix E(int N)
        {
            Matrix matrix = new Matrix(N);
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    matrix[i, j] = 1;
            return matrix;
        }

        /// <summary>
        /// Детерминант матрицы.
        /// </summary>
        public double Determinant
        {
            get
            {
                Matrix m1 = this;
                if (m1.Size == 1) return m1[0, 0];
                if (m1.Size == 2) return m1[0, 0] * m1[1, 1] - m1[0, 1] * m1[1, 0];
                if (m1.Size == 3) return m1[0, 0] * m1[1, 1] * m1[2, 2] + m1[0, 1] * m1[1, 2] * m1[2, 0] + m1[0, 2] * m1[1, 0] * m1[2, 1] - m1[0, 2] * m1[1, 1] * m1[2, 0] - m1[0, 0] * m1[1, 2] * m1[2, 1] - m1[0, 1] * m1[1, 0] * m1[2, 2];
                double det = 0;
                for (int i = 0; i < m1.Size; i++)
                {
                    det += Math.Pow(-1, i) * m1[0, i] * m1.Exclude(0, i).Determinant;
                }
                return det;
            }
        }


        /// <summary>
        /// Определитель матрицы.
        /// </summary>
        public static double determinant(Matrix m1)
        {
            if (m1.Size == 1) return m1[0, 0];
            if (m1.Size == 2) return m1[0, 0] * m1[1, 1] - m1[0, 1] * m1[1, 0];
            if (m1.Size == 3) return m1[0, 0] * m1[1, 1] * m1[2, 2] + m1[0, 1] * m1[1, 2] * m1[2, 0] + m1[0, 2] * m1[1, 0] * m1[2, 1] - m1[0, 2] * m1[1, 1] * m1[2, 0] - m1[0, 0] * m1[1, 2] * m1[2, 1] - m1[0, 1] * m1[1, 0] * m1[2, 2];
            double det = 0;
            for (int i = 0; i < m1.Size; i++)
            {
                det += Math.Pow(-1, i) * m1[0, i] * determinant(m1.Exclude(0, i));
            }
            return det;
        }


        /// <summary>
        /// Обратная матрица. Обратная матрица существует только для квадратных, невырожденных, матриц.
        /// </summary>
        public Matrix Inverse
        {
            get
            {
                Matrix matrix = new Matrix(Size);
                double determinant = Determinant;
                if (determinant == 0) return matrix; /// Если определитель == 0 - матрица вырожденная
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        Matrix tmp = Exclude(i, j);
                        matrix[j, i] = (Math.Pow(-1, i + 1 + j + 1) / determinant) * tmp.Determinant;
                    }
                }
                return matrix;
            }
        }


        /// <summary>
        /// Обратная матрица. Обратная матрица существует только для квадратных, невырожденных, матриц.
        /// </summary>
        public static Matrix inverse(Matrix M, int round = 0)
        {
            Matrix matrix = new Matrix(M.Size);
            double determinant = M.Determinant;
            if (determinant == 0) return matrix; //Если определитель == 0 - матрица вырожденная
            for (int i = 0; i < M.Size; i++)
            {
                for (int j = 0; j < M.Size; j++)
                {
                    Matrix tmp = M.Exclude(i, j);
                    matrix[j, i] = round == 0 ? (Math.Pow(-1, i + 1 + j + 1) / determinant) * tmp.Determinant : Math.Round(((1 / determinant) * tmp.Determinant), round, MidpointRounding.ToEven);
                }
            }
            return matrix;
        }


        /// <summary>
        /// Транспонирование матицы.
        /// </summary>
        public Matrix Transpose
        {
            get
            {
                Matrix matrix = new Matrix(Size);
                for (int i = 0; i < Size; i++)
                    for (int j = 0; j < Size; j++)
                        matrix[j, i] = this[i, j];
                return matrix;
            }
        }


        /// <summary>
        /// Транспонирование матицы.
        /// </summary>
        public Matrix transpose(Matrix M)
        {
            Matrix matrix = new Matrix(M.Size);
            for (int i = 0; i < M.Size; i++)
                for (int j = 0; j < M.Size; j++)
                    matrix[j, i] = M[i, j];
            return matrix;
        }

        /// <summary>
        /// Размерность матрицы.
        /// </summary>
        public int Size { get { return matrix.GetLength(0); } }

    }
}
