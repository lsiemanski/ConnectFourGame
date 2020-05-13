using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFourGame
{
    abstract class Algorithm
    {
        public int SelectedMove { protected set; get; }
        protected static Random random = new Random();
        public delegate double Score(GameBoard gameBoard, int player);
        public static int[,] CellScores = initCellScores();

        public double Player1WinningPositions = 1.0;
        public double Player2WinningPositions = 1.0;
        public double Player1WinningPositionsPower = 1.0;
        public double Player2WinningPositionsPower = 1.0;
        public double Player1PromisingPositionsPower = 4.0;
        public double Player2ProsimingPositionsPower = 4.0;
        public double WinningPositionsPercentage = 1.0;
        public double PromisingPositionsPercentage = 1.0;

        public static double WinLoseScore(GameBoard gameBoard, int player)
        {
            if (!gameBoard.IsOver)
                return 0;

            if (gameBoard.Winner == player)
                return 1;

            return -1;
        }

        public double WinningPositionsScore(GameBoard gameBoard, int player)
        {
            if (!gameBoard.IsOver)
            {
                double score = 0;
                foreach(var value in GameBoard.WinningSlots)
                {
                    if (value.Count(item => gameBoard.Slots[item.Item1, item.Item2] ==
                         (player == GameBoard.PLAYER_1_COIN ? GameBoard.PLAYER_2_COIN : GameBoard.PLAYER_1_COIN)) == 0)
                        score += Player1WinningPositions * 
                            Math.Pow(value.Count(item => gameBoard.Slots[item.Item1, item.Item2] == player), 
                                     Player1WinningPositionsPower);

                    if (value.Count(item => gameBoard.Slots[item.Item1, item.Item2] ==
                         (player == GameBoard.PLAYER_1_COIN ? GameBoard.PLAYER_1_COIN : GameBoard.PLAYER_2_COIN)) == 0)
                        score -= Player2WinningPositions * 
                            Math.Pow(value.Count(item => gameBoard.Slots[item.Item1, item.Item2] == player), 
                                     Player2WinningPositionsPower);
                }

                return score;
            }

            if (gameBoard.Winner == player)
                return double.MaxValue;

            return double.MinValue;
        }

        public double PromisingPositionsScore(GameBoard gameBoard, int player)
        {
            if(!gameBoard.IsOver)
            {
                double score = 0;
                for(int i = 0; i < GameBoard.ROWS; i++)
                    for(int j = 0; j < GameBoard.COLUMNS; j++)
                    {
                        if (gameBoard.Slots[i, j] == player)
                            score += Player1PromisingPositionsPower * CellScores[i, j];
                        else if (gameBoard.Slots[i, j] != 0)
                            score -= Player2ProsimingPositionsPower * CellScores[i, j];
                    }

                return score;
            }

            if (gameBoard.Winner == player)
                return double.MaxValue;

            return double.MinValue;
        }

        public double WeightedScore(GameBoard gameBoard, int player)
        {
            if(!gameBoard.IsOver)
            {
                return WinningPositionsPercentage * WinningPositionsScore(gameBoard, player) +
                     PromisingPositionsPercentage * PromisingPositionsScore(gameBoard, player);
            }

            if (gameBoard.Winner == player)
                return double.MaxValue;

            return double.MinValue;
        }

        private static int[,] initCellScores()
        {
            int[,] cellScores = new int[GameBoard.ROWS, GameBoard.COLUMNS];

            foreach (var value in GameBoard.WinningSlots)
                foreach (var item in value)
                    cellScores[item.Item1, item.Item2]++;

            return cellScores;
        }

        public abstract double Run(GameBoard gameBoard, int depth, int currentPlayer, Score scoreFunction);
    }
}
