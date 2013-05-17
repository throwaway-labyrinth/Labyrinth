using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RotatingWalkMatrix
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Enter size for the matrix: ");
            int num = int.Parse(Console.ReadLine());
            Matrix testmatrix = new Matrix(num);
            Console.WriteLine(testmatrix);
        }
    }
}
