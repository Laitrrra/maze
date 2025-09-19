using System;
using System.Collections.Generic;

public class MazeGeneration
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int ExitX { get; private set; }
    public int ExitY { get; private set; }

    private char[,] grid;
    private Random random = new Random();

    public MazeGeneration(int width, int height)
    {
        Width = width;
        Height = height;
        grid = new char[width, height];
    }

    public void Generate()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                grid[x, y] = '█';
            }
        }

        int startX = Width / 2;
        int startY = Height / 2;
        grid[startX, startY] = ' ';

        Stack<(int x, int y)> stack = new Stack<(int, int)>();
        stack.Push((startX, startY));

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            var neighbors = GetUnvisitedNeighbors(current.x, current.y);

            if (neighbors.Count > 0)
            {
                stack.Push(current);
                var next = neighbors[random.Next(neighbors.Count)];

                int wallX = (current.x + next.x) / 2;
                int wallY = (current.y + next.y) / 2;
                grid[wallX, wallY] = ' ';

                grid[next.x, next.y] = ' ';
                stack.Push(next);
            }
        }
    }

    private List<(int x, int y)> GetUnvisitedNeighbors(int x, int y)
    {
        var neighbors = new List<(int, int)>();

        int[] dx = { 0, 2, 0, -2 };
        int[] dy = { -2, 0, 2, 0 };

        for (int i = 0; i < 4; i++)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];

            if (nx >= 1 && nx < Width - 1 && ny >= 1 && ny < Height - 1 && grid[nx, ny] == '█')
            {
                neighbors.Add((nx, ny));
            }
        }

        return neighbors;
    }

    public void CreateExit()
    {
        int side = random.Next(4);

        switch (side)
        {
            case 0: 
                ExitX = random.Next(1, Width - 2);
                ExitY = 1;
                break;
            case 1:
                ExitX = Width - 2;
                ExitY = random.Next(1, Height - 2);
                break;
            case 2: 
                ExitX = random.Next(1, Width - 2);
                ExitY = Height - 2;
                break;
            case 3:
                ExitX = 1;
                ExitY = random.Next(1, Height - 2);
                break;
        }

        grid[ExitX, ExitY] = 'E';
        EnsurePathToExit();
    }

    private void EnsurePathToExit()
    {
        int[] dx = { 0, 1, 0, -1 };
        int[] dy = { -1, 0, 1, 0 };

        for (int i = 0; i < 4; i++)
        {
            int nx = ExitX + dx[i];
            int ny = ExitY + dy[i];

            if (nx >= 1 && nx < Width - 1 && ny >= 1 && ny < Height - 1)
            {
                grid[nx, ny] = ' ';
                break;
            }
        }
    }

    public bool CanMoveTo(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height &&
               (grid[x, y] == ' ' || grid[x, y] == 'E');
    }

    public char GetCell(int x, int y)
    {
        return grid[x, y];
    }

    public void Draw(bool showSolution, List<(int x, int y)> solutionPath)
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Console.SetCursorPosition(x, y);

                if (x == ExitX && y == ExitY)
                {
                    Console.Write('E');
                }
                else if (showSolution && solutionPath.Contains((x, y)))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write('·');
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(grid[x, y]);
                }
            }
        }
    }
}