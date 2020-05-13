using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFourGame
{
    public class GameBoard
    {
        public static readonly int EMPTY_SLOT = 0;
        public static readonly int PLAYER_1_COIN = 1;
        public static readonly int PLAYER_2_COIN = 2;
        public static readonly int COLUMNS = 7;
        public static readonly int ROWS = 6;
        public static readonly int K = 4;
        public bool IsOver { private set; get; }
        public int Winner { private set; get; }
        public int[,] Slots { private set; get; }
        public static readonly IList<Tuple<int, int>[]> WinningSlots = initWinningSlots();
        
        public GameBoard()
        {
            initSlots();
            IsOver = false;
        }

        public GameBoard(GameBoard gameBoard)
        {
            initSlots();

            for(int i = 0; i < ROWS; i++)
            {
                for(int j = 0; j < COLUMNS; j++)
                {
                    Slots[i, j] = gameBoard.Slots[i, j];
                }
            }

            IsOver = gameBoard.IsOver;
        }

        public bool AddCoin(int column, int value)
        {
            if (IsOver)
                return false;

            if (column > COLUMNS)
                return false;

            for(int i = ROWS - 1; i >= 0; i--)
            {
                if(Slots[i, column] == EMPTY_SLOT)
                {
                    Slots[i, column] = value;
                    IsOver = IsGameOver();
                    return true;
                }
            }

            return false;
        }

        public void PrintBoard()
        {
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLUMNS; j++)
                {
                    if (Slots[i, j] == PLAYER_1_COIN)
                        Console.ForegroundColor = ConsoleColor.Red;
                    else if (Slots[i, j] == PLAYER_2_COIN)
                        Console.ForegroundColor = ConsoleColor.DarkYellow;

                    Console.Write(Slots[i, j] + " ");

                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }

        public bool IsGameOver()
        {
            foreach(var winningSlot in WinningSlots)
            {
                int playerChecked = Slots[winningSlot[0].Item1, winningSlot[0].Item2];
                bool flag = true;
                if (playerChecked != EMPTY_SLOT)
                {
                    foreach(var slot in winningSlot)
                    {
                        if(Slots[slot.Item1, slot.Item2] != playerChecked)
                        {
                            flag = false;
                            break;
                        }
                    }

                    if (flag)
                    {
                        Winner = playerChecked;
                        return true;
                    }
                }
            }

            return false;
        }

        public IList<int> GetPossibleMoves()
        {
            IList<int> possibleMoves = new List<int>();

            for(int i = 0; i < COLUMNS; i++)
            {
                if (Slots[0, i] == EMPTY_SLOT)
                    possibleMoves.Add(i);
            }

            return possibleMoves;
        }

        public override string ToString()
        {
            string str = "";
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLUMNS; j++)
                {
                    str += Slots[i, j] + " ";
                }
                str += "\n";
            }
            return str;
        }

        public override bool Equals(object obj)
        {
            if (!obj.GetType().Equals(GetType()))
                return false;

            GameBoard gameBoard = (GameBoard)obj;

            for(int i = 0; i < ROWS; i++)
            {
                for(int j = 0; j < COLUMNS; j++)
                {
                    if (Slots[i, j] != gameBoard.Slots[i, j])
                        return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            int value = 0;
            for(int i = 0; i < ROWS; i++)
            {
                for(int j = 0; j < COLUMNS; j++)
                {
                    value = value ^ Slots[i, j];
                }
            }

            return value;
        }

        private void initSlots()
        {
            Slots = new int[ROWS, COLUMNS];

            for(int i = 0; i < ROWS; i++)
            {
                for(int j = 0; j < COLUMNS; j++)
                {
                    Slots[i, j] = EMPTY_SLOT;
                }
            }
        }

        private static IList<Tuple<int, int>[]> initWinningSlots()
        {
            IList<Tuple<int, int>[]> winningSlots = new List<Tuple<int, int>[]>();

            for(int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j <= COLUMNS - K; j++)
                {
                    Tuple<int, int>[] newWinningSlot = new Tuple<int, int>[K];
                    for(int k = 0; k < K; k++)
                    {
                        newWinningSlot[k] = new Tuple<int, int>(i, j+k);
                    }
                    winningSlots.Add(newWinningSlot);
                }
            } // horizontal

            for(int i = 0; i < COLUMNS; i++)
            {
                for(int j = 0; j <= ROWS - K; j++)
                {
                    Tuple<int, int>[] newWinningSlot = new Tuple<int, int>[K];
                    for (int k = 0; k < K; k++)
                    {
                        newWinningSlot[k] = new Tuple<int, int>(j+k, i);
                    }
                    winningSlots.Add(newWinningSlot);
                }
            } // vertical
            
            for(int i = 0; i <= ROWS - K; i++)
            {
                for (int j = 0; j <= COLUMNS - K; j++)
                {
                    Tuple<int, int>[] newWinningSlot = new Tuple<int, int>[K];
                    for (int k = 0; k < K; k++)
                    {
                        newWinningSlot[k] = new Tuple<int, int>(i+k, j+k);
                    }
                    winningSlots.Add(newWinningSlot);
                }
            } // diagonal left to right

            for (int i = 0; i <= ROWS - K; i++)
            {
                for (int j = K-1; j < COLUMNS; j++)
                {
                    Tuple<int, int>[] newWinningSlot = new Tuple<int, int>[K];
                    for (int k = 0; k < K; k++)
                    {
                        newWinningSlot[k] = new Tuple<int, int>(i+k, j-k);
                    }
                    winningSlots.Add(newWinningSlot);
                }
            } // diagonal right to left

            //foreach (var winningSlot in winningSlots)
            //{
            //    foreach (var slot in winningSlot)
            //    {
            //        Console.Write(slot + " ");
            //    }
            //    Console.WriteLine();
            //}

            return winningSlots;
        }
    }

    class GameBoardComparer : IEqualityComparer<GameBoard>
    {
        public bool Equals(GameBoard x, GameBoard y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(GameBoard obj)
        {
            return obj.GetHashCode();
        }
    }
}
