using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;

namespace UiTools.Av.Demo.Tetris
{
    public partial class TetrisGameControl : UserControl
    {
        // Размеры поля и блоков
        private const int BoardWidth = 10;
        private const int BoardHeight = 20;
        private const int BlockSize = 30; // Размер одного блока в пикселях

        // Игровое поле (0 - пусто, >0 - цвет блока)
        private IImmutableSolidColorBrush?[,] _gameBoard = new IImmutableSolidColorBrush?[BoardWidth, BoardHeight];

        // Текущая фигура
        private Point[] _currentPiece = [];
        private Point _currentPosition;
        private IImmutableSolidColorBrush _currentPieceColor = Brushes.Transparent;
        private int _currentPieceTypeIndex;
        private int _currentRotation;

        // Таймер игры
        private readonly DispatcherTimer _gameTimer;
        private TimeSpan _tickInterval = TimeSpan.FromMilliseconds(500);

        // Состояние игры
        private bool _isGameOver = false;
        private int _score = 0;
        private readonly Random _random = new Random();

        // Определения фигур (каждая фигура - массив точек, каждая точка - смещение от _currentPosition)
        // Каждая фигура имеет 4 варианта вращения
        private readonly Point[][][] _tetrominoes = new Point[][][]
        {
            // I
            new Point[][] {
                [new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(3, 0)],
                [new Point(1, -1), new Point(1, 0), new Point(1, 1), new Point(1, 2)],
                [new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(3, 0)], // Повтор для простоты
                [new Point(1, -1), new Point(1, 0), new Point(1, 1), new Point(1, 2)] // Повтор
            },
            // O
            new Point[][] {
                [new Point(0, 0), new Point(1, 0), new Point(0, 1), new Point(1, 1)],
                [new Point(0, 0), new Point(1, 0), new Point(0, 1), new Point(1, 1)],
                [new Point(0, 0), new Point(1, 0), new Point(0, 1), new Point(1, 1)],
                [new Point(0, 0), new Point(1, 0), new Point(0, 1), new Point(1, 1)]
            },
            // T
            new Point[][] {
                [new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(1, 0)],
                [new Point(1, 0), new Point(1, 1), new Point(1, 2), new Point(0, 1)],
                [new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(1, 1)],
                [new Point(0, 0), new Point(0, 1), new Point(0, 2), new Point(1, 1)]
            },
            // S
            new Point[][] {
                [new Point(1, 0), new Point(2, 0), new Point(0, 1), new Point(1, 1)],
                [new Point(0, 0), new Point(0, 1), new Point(1, 1), new Point(1, 2)],
                [new Point(1, 0), new Point(2, 0), new Point(0, 1), new Point(1, 1)], // Повтор
                [new Point(0, 0), new Point(0, 1), new Point(1, 1), new Point(1, 2)] // Повтор
            },
            // Z
            new Point[][] {
                [new Point(0, 0), new Point(1, 0), new Point(1, 1), new Point(2, 1)],
                [new Point(1, 0), new Point(0, 1), new Point(1, 1), new Point(0, 2)],
                [new Point(0, 0), new Point(1, 0), new Point(1, 1), new Point(2, 1)], // Повтор
                [new Point(1, 0), new Point(0, 1), new Point(1, 1), new Point(0, 2)] // Повтор
            },
            // L
            new Point[][] {
                [new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(2, 0)],
                [new Point(0, 0), new Point(1, 0), new Point(1, 1), new Point(1, 2)],
                [new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(0, 1)],
                [new Point(0, 0), new Point(0, 1), new Point(0, 2), new Point(1, 2)]
            },
            // J
            new Point[][] {
                [new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(2, 1)],
                [new Point(1, 0), new Point(1, 1), new Point(1, 2), new Point(0, 2)],
                [new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(0, 0)],
                [new Point(0, 0), new Point(1, 0), new Point(0, 1), new Point(0, 2)]
            }
        };

        private readonly IImmutableSolidColorBrush[] _tetrominoColors =
        [
            Brushes.Cyan, Brushes.Yellow, Brushes.Purple, Brushes.Green,
            Brushes.Red, Brushes.Orange, Brushes.Blue
        ];

        public TetrisGameControl()
        {
            InitializeComponent();
            _gameTimer = new DispatcherTimer { Interval = _tickInterval };
            _gameTimer.Tick += GameTimer_Tick;
        }

        private void OnControlLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            // Устанавливаем размеры Canvas в зависимости от размеров поля и блоков
            GameCanvas.Width = BoardWidth * BlockSize;
            GameCanvas.Height = BoardHeight * BlockSize;

            // Запрос фокуса, чтобы UserControl мог получать события клавиатуры
            this.Focus();
        }

        private void StartButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            StartGame();
            this.Focus(); // Важно вернуть фокус после клика на кнопку
        }

        private void StartGame()
        {
            _gameBoard = new IImmutableSolidColorBrush?[BoardWidth, BoardHeight];
            _score = 0;
            _isGameOver = false;
            ScoreTextBlock.Text = _score.ToString();
            GameOverTextBlock.IsVisible = false;
            StartButton.Content = "Перезапустить";

            SpawnNewPiece();
            _gameTimer.Start();
            DrawGame();
        }

        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            if (_isGameOver) return;
            MovePiece(0, 1); // Движение вниз
        }

        private void SpawnNewPiece()
        {
            _currentRotation = 0;
            _currentPieceTypeIndex = _random.Next(_tetrominoes.Length);
            _currentPiece = _tetrominoes[_currentPieceTypeIndex][_currentRotation];
            _currentPieceColor = _tetrominoColors[_currentPieceTypeIndex];
            _currentPosition = new Point(BoardWidth / 2.0 - 1, 0); // По центру сверху

            if (CheckCollision(_currentPiece, _currentPosition))
            {
                GameOver();
            }
        }

        private void MovePiece(int dx, int dy)
        {
            if (_isGameOver) return;

            var newPosition = new Point(_currentPosition.X + dx, _currentPosition.Y + dy);
            if (!CheckCollision(_currentPiece, newPosition))
            {
                _currentPosition = newPosition;
            }
            else
            {
                if (dy > 0) // Если двигались вниз и столкнулись
                {
                    LockPiece();
                    ClearLines();
                    SpawnNewPiece();
                    if (_isGameOver) return; // SpawnNewPiece может вызвать GameOver
                }
            }
            DrawGame();
        }

        private void RotatePiece()
        {
            if (_isGameOver) return;

            var nextRotation = (_currentRotation + 1) % _tetrominoes[_currentPieceTypeIndex].Length;
            var rotatedPiece = _tetrominoes[_currentPieceTypeIndex][nextRotation];

            if (!CheckCollision(rotatedPiece, _currentPosition))
            {
                _currentPiece = rotatedPiece;
                _currentRotation = nextRotation;
            }
            // Простая проверка на выход за пределы при вращении (без wall kick)
            else
            {
                // Попробовать сдвинуть на 1 влево
                var shiftedLeft = new Point(_currentPosition.X - 1, _currentPosition.Y);
                if (!CheckCollision(rotatedPiece, shiftedLeft))
                {
                    _currentPiece = rotatedPiece;
                    _currentRotation = nextRotation;
                    _currentPosition = shiftedLeft;
                    DrawGame();
                    return;
                }
                // Попробовать сдвинуть на 1 вправо
                var shiftedRight = new Point(_currentPosition.X + 1, _currentPosition.Y);
                if (!CheckCollision(rotatedPiece, shiftedRight))
                {
                    _currentPiece = rotatedPiece;
                    _currentRotation = nextRotation;
                    _currentPosition = shiftedRight;
                    DrawGame();
                    return;
                }
            }
            DrawGame();
        }

        private void HardDrop()
        {
            if (_isGameOver) return;
            while (!CheckCollision(_currentPiece, new Point(_currentPosition.X, _currentPosition.Y + 1)))
            {
                _currentPosition = new Point(_currentPosition.X, _currentPosition.Y + 1);
            }
            LockPiece();
            ClearLines();
            SpawnNewPiece();
            DrawGame();
        }


        private bool CheckCollision(Point[] piece, Point position)
        {
            foreach (var p in piece)
            {
                var boardX = (int)(position.X + p.X);
                var boardY = (int)(position.Y + p.Y);

                // Проверка границ
                if (boardX < 0 || boardX >= BoardWidth || boardY < 0 || boardY >= BoardHeight)
                {
                    return true; // Столкновение со стеной или дном
                }

                // Проверка столкновения с другими блоками на поле
                if (boardY >= 0 && _gameBoard[boardX, boardY] != null) // boardY >=0 чтобы не проверять "невидимую" часть сверху
                {
                    return true;
                }
            }
            return false;
        }

        private void LockPiece()
        {
            foreach (var p in _currentPiece)
            {
                var boardX = (int)(_currentPosition.X + p.X);
                var boardY = (int)(_currentPosition.Y + p.Y);

                if (boardY >= 0 && boardY < BoardHeight && boardX >= 0 && boardX < BoardWidth) // Убедимся, что не вышли за пределы
                {
                    _gameBoard[boardX, boardY] = _currentPieceColor;
                }
            }
        }

        private void ClearLines()
        {
            var linesCleared = 0;
            for (var y = BoardHeight - 1; y >= 0; y--)
            {
                var lineIsFull = true;
                for (var x = 0; x < BoardWidth; x++)
                {
                    if (_gameBoard[x, y] == null)
                    {
                        lineIsFull = false;
                        break;
                    }
                }

                if (lineIsFull)
                {
                    linesCleared++;
                    // Сдвинуть все строки выше вниз
                    for (var K = y; K > 0; K--)
                    {
                        for (var L = 0; L < BoardWidth; L++)
                        {
                            _gameBoard[L, K] = _gameBoard[L, K - 1];
                        }
                    }
                    // Очистить верхнюю строку
                    for (var L = 0; L < BoardWidth; L++)
                    {
                        _gameBoard[L, 0] = null;
                    }
                    y++; // Проверить эту же строку еще раз, так как она теперь новая
                }
            }

            if (linesCleared > 0)
            {
                _score += linesCleared * 100 * linesCleared; // Бонус за несколько линий
                ScoreTextBlock.Text = _score.ToString();
                // Ускорение игры
                if (_tickInterval.TotalMilliseconds > 200)
                {
                    _tickInterval = TimeSpan.FromMilliseconds(_tickInterval.TotalMilliseconds - linesCleared * 10);
                    _gameTimer.Interval = _tickInterval;
                }
            }
        }

        private void DrawGame()
        {
            GameCanvas.Children.Clear();

            // Рисуем "упавшие" блоки
            for (var x = 0; x < BoardWidth; x++)
            {
                for (var y = 0; y < BoardHeight; y++)
                {
                    if (_gameBoard[x, y] != null)
                    {
                        DrawBlock(x, y, _gameBoard[x, y]!);
                    }
                }
            }

            // Рисуем текущую фигуру
            if (!_isGameOver)
            {
                foreach (var p in _currentPiece)
                {
                    DrawBlock((int)(_currentPosition.X + p.X), (int)(_currentPosition.Y + p.Y), _currentPieceColor);
                }
            }
        }

        private void DrawBlock(int x, int y, IImmutableSolidColorBrush color)
        {
            if (y < 0) return; // Не рисуем блоки выше видимой части поля

            var rect = new Avalonia.Controls.Shapes.Rectangle
            {
                Width = BlockSize - 1, // -1 для видимости сетки
                Height = BlockSize - 1,
                Fill = color,
                Stroke = Brushes.Black, // Обводка для лучшей видимости
                StrokeThickness = 0.5
            };
            Canvas.SetLeft(rect, x * BlockSize);
            Canvas.SetTop(rect, y * BlockSize);
            GameCanvas.Children.Add(rect);
        }

        private void OnGameKeyDown(object? sender, KeyEventArgs e)
        {
            if (_isGameOver)
            {
                if (e.Key == Key.Enter || e.Key == Key.Space) StartGame();
                return;
            }

            switch (e.Key)
            {
                case Key.D4: // Движение влево (NumPad4 или цифра 4)
                case Key.NumPad4:
                case Key.Left: // Добавил стрелку для удобства
                    MovePiece(-1, 0);
                    break;
                case Key.D9: // Движение вправо (NumPad9 или цифра 9)
                case Key.NumPad9:
                case Key.Right: // Добавил стрелку для удобства
                    MovePiece(1, 0);
                    break;
                case Key.D8: // Ускоренное падение (NumPad8 или цифра 8)
                case Key.NumPad8:
                case Key.Down: // Добавил стрелку для удобства
                    MovePiece(0, 1);
                    break;
                case Key.D7: // Вращение (NumPad7 или цифра 7)
                case Key.NumPad7:
                case Key.Up: // Добавил стрелку для удобства
                    RotatePiece();
                    break;
                case Key.Space: // Быстрый сброс фигуры
                    HardDrop();
                    break;
            }
            e.Handled = true; // Предотвращаем дальнейшую обработку события
        }

        private void GameOver()
        {
            _isGameOver = true;
            _gameTimer.Stop();
            GameOverTextBlock.IsVisible = true;
            StartButton.Content = "Играть Снова?"; // Или "Начать заново"
        }
    }
}