using System;
using System.Collections.Generic;
using System.Numerics;

public class Game
{
    private MazeGeneration maze;
    private Player player;
    private PathFinder pathFinder;
    private bool showSolution = false;
    private List<(int x, int y)> previousSolutionPath = new List<(int x, int y)>();

    public void Run()
    {
        Console.CursorVisible = false;
        Console.Title = "Лабиринт";

        while (true)
        {
            InitializeGame();

            if (Play())
            {
                Console.SetCursorPosition(0, maze.Height + 3);
                Console.WriteLine("Поздравляем! Вы нашли выход! Нажмите любую клавишу...");
                Console.ReadKey();
            }
        }
    }

    private void InitializeGame()
    {
        maze = new MazeGeneration(91, 25);
        player = new Player(maze.Width / 2, maze.Height / 2);
        pathFinder = new PathFinder(maze);
        showSolution = false;
        previousSolutionPath.Clear();

        maze.Generate();
        maze.CreateExit();
        UpdateSolutionPath();
        DrawInitialMaze();
        DisplayControls();
    }

    private void DrawInitialMaze()
    {
        Console.Clear();
        maze.Draw(showSolution, pathFinder.SolutionPath);
        player.Draw();
    }

    private void DisplayControls()
    {
        Console.SetCursorPosition(0, maze.Height + 2);
        Console.WriteLine("Управление: стрелки - движение, S - решение, R - новая игра, ESC - выход");
    }

    private bool Play()
    {
        while (true)
        {
            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.DownArrow:
                case ConsoleKey.LeftArrow:
                case ConsoleKey.RightArrow:
                    HandleMovement(key);
                    break;
                case ConsoleKey.S:
                    ToggleSolution();
                    break;
                case ConsoleKey.R:
                    return false;
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
            }

            if (player.X == maze.ExitX && player.Y == maze.ExitY)
            {
                return true;
            }
        }
    }

    private void UpdateSolutionPath()
    {
        pathFinder.FindSolution(player.X, player.Y, maze.ExitX, maze.ExitY);
    }

    private void HandleMovement(ConsoleKey key)
    {
        int newX = player.X;
        int newY = player.Y;

        switch (key)
        {
            case ConsoleKey.UpArrow: newY--; break;
            case ConsoleKey.DownArrow: newY++; break;
            case ConsoleKey.LeftArrow: newX--; break;
            case ConsoleKey.RightArrow: newX++; break;
        }

        if (maze.CanMoveTo(newX, newY))
        {
            
            int oldX = player.X;
            int oldY = player.Y;
            var oldSolutionPath = new List<(int x, int y)>(pathFinder.SolutionPath);

            player.Move(newX, newY, maze, showSolution, pathFinder.SolutionPath);

            if (showSolution)
            {
                UpdateSolutionPath();
                UpdateSolutionPathVisuals(oldSolutionPath, oldX, oldY);
            }
            else
            {
                Console.SetCursorPosition(oldX, oldY);
                Console.Write(maze.GetCell(oldX, oldY));
            }

            player.Draw();
        }
    }

    private void UpdateSolutionPathVisuals(List<(int x, int y)> oldPath, int oldX, int oldY)
    {
        foreach (var (x, y) in oldPath)
        {
            if ((x != player.X || y != player.Y) && (x != maze.ExitX || y != maze.ExitY))
            {
                Console.SetCursorPosition(x, y);
                Console.Write(maze.GetCell(x, y));
            }
        }

        foreach (var (x, y) in pathFinder.SolutionPath)
        {
            if ((x != player.X || y != player.Y) && (x != maze.ExitX || y != maze.ExitY))
            {
                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write('·');
                Console.ResetColor();
            }
        }
    }

    private void ToggleSolution()
    {
        showSolution = !showSolution;

        if (showSolution)
        {
            UpdateSolutionPath();
            foreach (var (x, y) in pathFinder.SolutionPath)
            {
                if ((x != player.X || y != player.Y) && (x != maze.ExitX || y != maze.ExitY))
                {
                    Console.SetCursorPosition(x, y);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write('·');
                    Console.ResetColor();
                }
            }
        }
        else
        {
            foreach (var (x, y) in pathFinder.SolutionPath)
            {
                if ((x != player.X || y != player.Y) && (x != maze.ExitX || y != maze.ExitY))
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(maze.GetCell(x, y));
                }
            }
        }

        player.Draw();
    }
}