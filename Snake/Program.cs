using System;
using System.Collections.Generic;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new SnakeGame();
            game.Start();
        }
    }

    class SnakeGame
    {
        private const int ScreenWidth = 32;
        private const int ScreenHeight = 16;
        private const int InitialScore = 5;
        private const int GameSpeed = 500; // in milliseconds
        private int _score;
        private int _berryX;
        private int _berryY;
        private bool _gameOver;
        private string _movement;
        private List<int> _xposBody;
        private List<int> _yposBody;
        private Pixel _head;
        private Random _random;

        public SnakeGame()
        {
            Console.WindowHeight = ScreenHeight;
            Console.WindowWidth = ScreenWidth;
            _score = InitialScore;
            _gameOver = false;
            _movement = "RIGHT";
            _xposBody = new List<int>();
            _yposBody = new List<int>();
            _head = new Pixel
            {
                xpos = ScreenWidth / 2,
                ypos = ScreenHeight / 2,
                schermkleur = ConsoleColor.Red
            };
            _random = new Random();
            _berryX = _random.Next(0, ScreenWidth);
            _berryY = _random.Next(0, ScreenHeight);
        }

        public void Start()
        {
            while (!_gameOver)
            {
                Render();
                Input();
                Logic();
                System.Threading.Thread.Sleep(GameSpeed);
            }
            GameOver();
        }

        private void Render()
        {
            Console.Clear();
            DrawBorders();
            DrawBerry();
            DrawSnake();
        }

        private void DrawBorders()
        {
            Console.ForegroundColor = ConsoleColor.Green;

            for (int i = 0; i < ScreenWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("■");
                Console.SetCursorPosition(i, ScreenHeight - 1);
                Console.Write("■");
            }

            for (int i = 0; i < ScreenHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                Console.SetCursorPosition(ScreenWidth - 1, i);
                Console.Write("■");
            }
        }

        private void DrawBerry()
        {
            Console.SetCursorPosition(_berryX, _berryY);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("■");
        }

        private void DrawSnake()
        {
            foreach (var (x, y) in _xposBody.Zip(_yposBody, Tuple.Create))
            {
                Console.SetCursorPosition(x, y);
                Console.Write("■");
                if (x == _head.xpos && y == _head.ypos)
                {
                    _gameOver = true;
                }
            }

            Console.SetCursorPosition(_head.xpos, _head.ypos);
            Console.ForegroundColor = _head.schermkleur;
            Console.Write("■");
        }

        private void Input()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow && _movement != "DOWN") _movement = "UP";
                if (key == ConsoleKey.DownArrow && _movement != "UP") _movement = "DOWN";
                if (key == ConsoleKey.LeftArrow && _movement != "RIGHT") _movement = "LEFT";
                if (key == ConsoleKey.RightArrow && _movement != "LEFT") _movement = "RIGHT";
            }
        }

        private void Logic()
        {
            _xposBody.Add(_head.xpos);
            _yposBody.Add(_head.ypos);

            switch (_movement)
            {
                case "UP": _head.ypos--; break;
                case "DOWN": _head.ypos++; break;
                case "LEFT": _head.xpos--; break;
                case "RIGHT": _head.xpos++; break;
            }

            if (_head.xpos == _berryX && _head.ypos == _berryY)
            {
                _score++;
                _berryX = _random.Next(1, ScreenWidth - 2);
                _berryY = _random.Next(1, ScreenHeight - 2);
            }

            if (_xposBody.Count > _score)
            {
                _xposBody.RemoveAt(0);
                _yposBody.RemoveAt(0);
            }

            if (_head.xpos == 0 || _head.xpos == ScreenWidth - 1 || _head.ypos == 0 || _head.ypos == ScreenHeight - 1)
            {
                _gameOver = true;
            }
        }

        private void GameOver()
        {
            Console.SetCursorPosition(ScreenWidth / 5, ScreenHeight / 2);
            Console.WriteLine($"Game over, Score: {_score}");
        }
    }

    class Pixel
    {
        public int xpos { get; set; }
        public int ypos { get; set; }
        public ConsoleColor schermkleur { get; set; }
    }
}
