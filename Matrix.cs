namespace RotatingWalkMatrix
{
    using System;
    using System.Text;

    public class Matrix
    {
        private int size;
        private int[,] matrix;

        public Matrix(int size)
        {
            this.Size = size;
            this.matrix = this.FillMatrix();
        }

        public int Size
        {
            get
            {
                return this.size;
            }

            private set
            {
                if (value < 1 || value > 100)
                {
                    throw new ArgumentException("The number must be between 1 and 100");
                }
                else
                {
                    this.size = value;
                }
            }
        }

        public override string ToString()
        {
            StringBuilder matrixString = new StringBuilder();

            for (int row = 0; row < this.Size; row++)
            {
                for (int col = 0; col < this.Size; col++)
                {
                    matrixString.AppendFormat("{0,3}", this.matrix[row, col]);
                }

                matrixString.AppendLine();
            }

            return matrixString.ToString();
        }

        private static void ChangeDirection(ref int directionX, ref int directionY)
        {
            int[] dirX = { 1, 1, 1, 0, -1, -1, -1, 0 };
            int[] dirY = { 1, 0, -1, -1, -1, 0, 1, 1 };

            int currDirection = 0;
            for (int dirIndex = 0; dirIndex < 8; dirIndex++)
            {
                if (dirX[dirIndex] == directionX && dirY[dirIndex] == directionY)
                {
                    currDirection = dirIndex;
                    break;
                }
            }

            if (currDirection == 7)
            {
                directionX = dirX[0];
                directionY = dirY[0];
                return;
            }

            directionX = dirX[currDirection + 1];
            directionY = dirY[currDirection + 1];
        }

        private static bool IsDirectionPossible(int[,] matrix, int row, int col)
        {
            int[] dirX = { 1, 1, 1, 0, -1, -1, -1, 0 };
            int[] dirY = { 1, 0, -1, -1, -1, 0, 1, 1 };
            int directionsCount = dirX.Length;

            for (int i = 0; i < directionsCount; i++)
            {
                if (row + dirX[i] >= matrix.GetLength(0) || row + dirX[i] < 0)
                { // If the row will be out of the matrix, don't move horizontally
                    dirX[i] = 0;
                }

                if (col + dirY[i] >= matrix.GetLength(0) || col + dirY[i] < 0)
                { // If the col will be out of the matrix, don't move vertically
                    dirY[i] = 0;
                }
            }

            // Check if the position is empty
            for (int i = 0; i < directionsCount; i++)
            {
                if (matrix[row + dirX[i], col + dirY[i]] == 0)
                {
                    return true;
                }
            }

            return false;
        }

        private void FindEmptyCell(int[,] matrix, out int row, out int col)
        {
            row = 0;
            col = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] == 0)
                    {
                        row = i;
                        col = j;
                        return;
                    }
                }
            }
        }

        private int[,] FillMatrix()
        {
            int[,] currMatrix = new int[this.Size, this.Size];

            int currNum = 1;
            int row = 0;
            int col = 0;
            int directionX = 1;
            int directionY = 1;

            int iterations = 1; // How many times the matrix should be traversed
            if (this.Size > 3)
            {
                iterations = 2;
            }

            do
            {
                while (true)
                {
                    currMatrix[row, col] = currNum;

                    if (!IsDirectionPossible(currMatrix, row, col))
                    {
                        currNum++;
                        break;  // No more space
                    }

                    while (
                        !this.IsPositionInside(row, col, directionX, directionY) ||
                        !this.IsCellEmpty(currMatrix, row, col, directionX, directionY))
                    {
                        ChangeDirection(ref directionX, ref directionY);
                    }

                    row += directionX;
                    col += directionY;
                    currNum++;
                }

                directionX = 1;
                directionY = 1;
                this.FindEmptyCell(currMatrix, out row, out col);

                iterations--;
            }
            while (iterations > 0);

            return currMatrix;
        }

        private bool IsCellEmpty(int[,] currMatrix, int row, int col, int directionX, int directionY)
        {
            return currMatrix[row + directionX, col + directionY] == 0;
        }

        private bool IsPositionInside(int row, int col, int directionX, int directionY)
        {
            bool result = !(row + directionX >= this.Size ||
                row + directionX < 0 ||
                col + directionY >= this.Size ||
                col + directionY < 0);

            return result;

            // bool inRow = row + directionX < this.Size && row + directionX > 0;
            // bool inCol = col + directionY < this.Size && col + directionY > 0;

            // if (!inRow || !inCol)
            // {
            //    return false;
            // }

            // return true;
        }
    }
}
