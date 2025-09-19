using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PathFinder
{
    private MazeGeneration maze;
    public List<(int x, int y)> SolutionPath { get; private set; } = new List<(int x, int y)>();

    public PathFinder(MazeGeneration maze)
    {
        this.maze = maze;
    }

    public void FindSolution(int startX, int startY, int endX, int endY)
    {
        SolutionPath.Clear();
        var queue = new Queue<List<(int x, int y)>>();
        var visited = new bool[maze.Width, maze.Height];

        var startPath = new List<(int x, int y)> { (startX, startY) };
        queue.Enqueue(startPath);
        visited[startX, startY] = true;

        while (queue.Count > 0)
        {
            var currentPath = queue.Dequeue();
            var (currentX, currentY) = currentPath.Last();

            if (currentX == endX && currentY == endY)
            {
                SolutionPath = currentPath;
                return;
            }

            int[] dx = { 0, 1, 0, -1 };
            int[] dy = { -1, 0, 1, 0 };

            for (int i = 0; i < 4; i++)
            {
                int newX = currentX + dx[i];
                int newY = currentY + dy[i];

                if (newX >= 0 && newX < maze.Width && newY >= 0 && newY < maze.Height &&
                    !visited[newX, newY] &&
                    (maze.GetCell(newX, newY) == ' ' || maze.GetCell(newX, newY) == 'E'))
                {
                    visited[newX, newY] = true;
                    var newPath = new List<(int x, int y)>(currentPath);
                    newPath.Add((newX, newY));
                    queue.Enqueue(newPath);
                }
            }
        }
    }
}