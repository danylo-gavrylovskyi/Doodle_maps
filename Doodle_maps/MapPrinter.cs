namespace Kse.Algorithms.Samples
{
    using System;
    using System.Collections.Generic;

    public class MapPrinter
    {
        public void Print(string[,] maze, List<Point> path, Point start, Point goal)
        {
            PrintTopLine();

            for (var row = 0; row < maze.GetLength(1); row++)
            {
                for (var column = 0; column < maze.GetLength(0); column++)
                {
                    if (start.Equals(new Point(row: row, column: column)))
                    {
                        Console.Write("A");
                    }
                    else if (goal.Equals(new Point(row: row, column: column)))
                    {
                        Console.Write("B");
                    }
                    else if (path.Contains(new Point(row: row, column: column)))
                    {
                        Console.Write(".");
                    }
                    else
                    {
                        Console.Write(maze[column, row]);
                    }
                }

                Console.WriteLine();
            }

            void PrintTopLine()
            {
                Console.Write($" \t");
                for (int i = 0; i < maze.GetLength(0); i++)
                {
                    Console.Write(i % 10 == 0? i / 10 : " ");
                }
    
                Console.Write($"\n \t");
                for (int i = 0; i < maze.GetLength(0); i++)
                {
                    Console.Write(i % 10);
                }
    
                Console.WriteLine("\n");
            }
        }
    }
}