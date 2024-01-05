using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048_CON
{
    public class Game
    {
        private const int board_size = 4;
        private int[,] game_board;
        private Random rand = new Random();
        private ConsoleColor current_color = Console.ForegroundColor;
        private ConsoleColor current_color_bg = Console.BackgroundColor;

        public void InitGame()
        {
            game_board = new int[board_size, board_size];

            AddTile();
            AddTile();
        }

        public void RenderGame()
        {
            Console.WriteLine("Score: " + GetScore());

            for (int i = 0; i < board_size; i++)
            {
                for (int j = 0; j < board_size; j++)
                {
                    int tile = game_board[i, j];

                    if (tile == 2)
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    else if (tile == 4)
                        Console.ForegroundColor = ConsoleColor.Gray;
                    else if (tile == 8)
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                    else if (tile == 16)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    else if (tile == 32)
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                    else if (tile == 64)
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    else if (tile == 128)
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    else if (tile == 256)
                        Console.ForegroundColor = ConsoleColor.Blue;
                    else if (tile == 512)
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    else if (tile == 1024)
                        Console.ForegroundColor = ConsoleColor.Magenta;
                    else if (tile == 2048)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }

                    Console.Write(tile == 0 ? "   -" : $"{tile,4}");
                    Console.ForegroundColor = current_color;
                    Console.BackgroundColor = current_color_bg;
                }

                Console.WriteLine();
            }
        }

        public void MoveTiles(ConsoleKey key)
        {
            bool moved = false;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    for (int i = 0; i < board_size; i++)
                        moved |= MoveUp(i);
                    break;
                case ConsoleKey.DownArrow:
                    for (int i = 0; i < board_size; i++)
                        moved |= MoveDown(i);
                    break;
                case ConsoleKey.LeftArrow:
                    for (int i = 0; i < board_size; i++)
                        moved |= MoveLeft(i);
                    break;
                case ConsoleKey.RightArrow:
                    for (int i = 0; i < board_size; i++)
                        moved |= MoveRight(i);
                    break;
            }

            if (moved)
                AddTile();
        }

        public bool GameOver()
        {
            if (game_board.Cast<int>().Any(tile => tile == 0))
                return false;

            for (int i = 0; i < board_size; i++)
                for (int j = 0; j < board_size - 1; j++)
                    if (game_board[i, j] == game_board[i, j + 1])
                        return false;

            for (int i = 0; i < board_size; i++)
                for (int j = 0; j < board_size - 1; j++)
                    if (game_board[j, i] == game_board[j + 1, i])
                        return false;

            return true;
        }

        private void AddTile()
        {
            List<(int, int)> empty_tiles = new List<(int, int)>();

            for (int i = 0; i < board_size; i++)
            {
                for (int j = 0; j < board_size; j++)
                {
                    if (game_board[i, j] == 0)
                        empty_tiles.Add((i, j));
                }
            }

            if (empty_tiles.Count > 0)
            {
                int rand_index = rand.Next(empty_tiles.Count);
                int val = rand.Next(10) == 0 ? 4 : 2;
                var (i, j) = empty_tiles[rand_index];
                game_board[i, j] = val;
            }
        }

        private bool MoveUp(int col)
        {
            int[] column = Enumerable.Range(0, board_size).Select(row => game_board[row, col]).ToArray();
            int[] new_column = MergeTiles(column);

            bool moved = false;

            for (int i = 0; i < board_size; i++)
            {
                if (game_board[i, col] != new_column[i])
                {
                    moved = true;
                    game_board[i, col] = new_column[i];
                }
            }

            return moved;
        }

        private bool MoveDown(int col)
        {
            int[] column = Enumerable.Range(0, board_size).Select(row => game_board[row, col]).Reverse().ToArray();
            int[] new_column = MergeTiles(column).Reverse().ToArray();

            bool moved = false;

            for (int i = 0; i < board_size; i++)
            {
                if (game_board[i, col] != new_column[i])
                {
                    moved = true;
                    game_board[i, col] = new_column[i];
                }
            }

            return moved;
        }

        private bool MoveLeft(int row)
        {
            int[] new_row = Enumerable.Range(0, board_size).Select(col => game_board[row, col]).ToArray();
            int[] new_row_after_merge = MergeTiles(new_row);

            bool moved = false;

            for (int i = 0; i < board_size; i++)
            {
                if (game_board[row, i] != new_row_after_merge[i])
                {
                    moved = true;
                    game_board[row, i] = new_row_after_merge[i];
                }
            }

            return moved;
        }

        private bool MoveRight(int row)
        {
            int[] new_row = Enumerable.Range(0, board_size).Select(col => game_board[row, col]).Reverse().ToArray();
            int[] new_row_after_merge = MergeTiles(new_row).Reverse().ToArray();

            bool moved = false;

            for (int i = 0; i < board_size; i++)
            {
                if (game_board[row, i] != new_row_after_merge[i])
                {
                    moved = true;
                    game_board[row, i] = new_row_after_merge[i];
                }
            }

            return moved;
        }

        private int[] MergeTiles(int[] row)
        {
            List<int> new_row = new List<int>();

            for (int i = 0; i < row.Length; i++)
            {
                if (row[i] == 0)
                    continue;

                if (i < row.Length - 1 && row[i] == row[i + 1])
                {
                    new_row.Add(row[i] * 2);
                    i++;
                }
                else
                    new_row.Add(row[i]);
            }

            new_row.AddRange(Enumerable.Repeat(0, row.Length - new_row.Count));
            return new_row.ToArray();
        }

        private int GetScore()
        {
            int score = 0;

            for (int i = 0; i < board_size; i++)
            {
                for (int j = 0; j < board_size; j++)
                {
                    score += game_board[i, j];
                }
            }

            return score;
        }
    }
}
