using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;

public class Player
{
    public int X { get; private set; }
    public int Y { get; private set; }
    private int oldX;
    private int oldY;

    public Player(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void Move(int newX, int newY, MazeGeneration maze, bool showSolution, List<(int x, int y)> solutionPath)
    {
        oldX = X;
        oldY = Y;
        X = newX;
        Y = newY;

        EraseOldPosition(maze, showSolution, solutionPath);

        Draw();
    }

    private void EraseOldPosition(MazeGeneration maze, bool showSolution, List<(int x, int y)> solutionPath)
    {
        Console.SetCursorPosition(oldX, oldY);

        if (showSolution && solutionPath.Contains((oldX, oldY)) && !(oldX == maze.ExitX && oldY == maze.ExitY))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write('·');
            Console.ResetColor();
        }
        else
        {
            Console.Write(maze.GetCell(oldX, oldY));
        }
    }

    public void Draw()
    {
        Console.SetCursorPosition(X, Y);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write('☻');
        Console.ResetColor();
    }
}