﻿using System;
using System.Threading.Tasks;

namespace Firelink
{
    class Matrix
    {
        //private double[] flatmap;
        private int columns;
        private int rows;
        private double[][] matrix;

        // Constructor:
        public Matrix(double[] flatmap, int columns)
        {
            int rowColFactor = flatmap.Length % columns;
            try
            {
                if (rowColFactor != 0)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("The length of the array ({0}) is not a factor of {1}.", flatmap.Length, columns);
            }

            //this.flatmap = flatmap;
            this.columns = columns;
            rows = flatmap.Length / columns;
            matrix = PopulateMatrix(flatmap);
        }

        public Matrix(double[][] matrix)
        {
            this.matrix = matrix;
            columns = matrix[0].Length;
            rows = matrix.Length;
        }

        private double[][] MatrixCreate()
        {
            double[][] result = new double[Rows][];
            for (int i = 0; i < Rows; ++i)
                result[i] = new double[Columns];

            return result;
        }

        private static double[][] MatrixCreate(int rows, int columns)
        {
            double[][] result = new double[rows][];
            for (int i = 0; i < rows; ++i)
                result[i] = new double[columns];

            return result;
        }

        private double[][] PopulateMatrix(double[] flatmap)
        {
            double[][] result = MatrixCreate();

            Parallel.For(0, Rows, i =>
            {
                int k = i * Columns;
                for (int j = 0; j < Columns; j++)
                {
                    result[i][j] = flatmap[j + k];
                }

            });

            return result;
        }

        public int Columns
        {
            get { return columns; }
        }

        public int Rows
        {
            get { return rows; }
        }

        public double[][] AsMatrix
        {
            get { return matrix; }
        }

        // Printing method:
        public void ShowMatrix()
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                Console.Write("Element({0}): ", i);

                for (int j = 0; j < matrix[i].Length; j++)
                {
                    Console.Write("{0}{1}", matrix[i][j], j == (matrix[i].Length - 1) ? "" : " ");
                }
                Console.WriteLine();
            }
        }

        // Matrix transpose.
        public Matrix Transpose()
        {
            var transpose = MatrixCreate(Columns, Rows);
            Parallel.For(0, Rows, i =>
            { 
                for (int j = 0; j < Columns; j++)
                {
                    transpose[j][i] = matrix[i][j];
                }
            });
            Matrix result = new Matrix(transpose);
            return result;
        }

        // Overload the multiplication for matrices.
        public static Matrix operator *(Matrix matrixA,
                                    Matrix matrixB)
        {
            int aRows = matrixA.Rows; int aCols = matrixA.Columns;
            int bRows = matrixB.Rows; int bCols = matrixB.Columns;
            if (aCols != bRows)
                throw new Exception("xxxx");

            double[][] product = MatrixCreate(aRows, bCols);

            Parallel.For(0, aRows, i =>
            {
                for (int j = 0; j < bCols; ++j) // each col of B
                    for (int k = 0; k < aCols; ++k) // could use k < bRows
                        product[i][j] += matrixA.AsMatrix[i][k] * matrixB.AsMatrix[k][j];
            }
            );

            Matrix result = new Matrix(product);

            return result;
        }
    }
}