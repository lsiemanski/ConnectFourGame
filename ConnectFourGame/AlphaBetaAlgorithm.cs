using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFourGame
{
    class AlphaBetaAlgorithm : Algorithm
    {
        public override double Run(GameBoard gameBoard, int depth, int currentPlayer, Score scoreFunction)
        {
            return AlphaBeta(gameBoard, depth, double.MinValue, double.MaxValue, currentPlayer, scoreFunction);
        }

        private double AlphaBeta(GameBoard gameBoard, int depth, double alpha, double beta, int currentPlayer, Score scoreFunction)
        {
            if (depth == 0 || gameBoard.IsOver || gameBoard.GetPossibleMoves().Count == 0)
                    return scoreFunction(gameBoard, GameBoard.PLAYER_1_COIN);
                
            Dictionary<int, double> moveScores = new Dictionary<int, double>();
            double value;

            if (currentPlayer == GameBoard.PLAYER_1_COIN) // MAX PLAYER
            {
                value = double.MinValue;
                foreach (var move in gameBoard.GetPossibleMoves())
                {
                    GameBoard newGameBoard = new GameBoard(gameBoard); // copy constructor
                    newGameBoard.AddCoin(move, currentPlayer);
                    moveScores[move] = AlphaBeta(newGameBoard, depth - 1, alpha, beta, GameBoard.PLAYER_2_COIN, scoreFunction);
                    value = max(value, moveScores[move]);
                    alpha = max(alpha, value);
                    if (alpha > beta)
                        break;
                }
            }  
            else // MIN PLAYER
            {
                value = double.MaxValue;
                foreach (var move in gameBoard.GetPossibleMoves())
                {
                    GameBoard newGameBoard = new GameBoard(gameBoard); // copy constructor
                    newGameBoard.AddCoin(move, currentPlayer);
                    moveScores[move] = AlphaBeta(newGameBoard, depth - 1, alpha, beta, GameBoard.PLAYER_1_COIN, scoreFunction);
                    value = min(value, moveScores[move]);
                    beta = min(beta, value);
                    if (alpha > beta)
                        break;
                }
            }

            KeyValuePair<int, double> selection = moveScores.Where(item => item.Value == value).OrderBy(item => random.Next()).First();

            SelectedMove = selection.Key;
            return selection.Value;
        }

        private double max(double a, double b)
        {
            return a > b ? a : b;
        }

        private double min(double a, double b)
        {
            return a < b ? a : b;
        }
    }
}
