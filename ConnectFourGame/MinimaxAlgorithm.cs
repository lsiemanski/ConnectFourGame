using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFourGame
{
    class MinimaxAlgorithm : Algorithm
    {

        public override double Run(GameBoard gameBoard, int depth, int currentPlayer, Score scoreFunction)
        {
            if (depth == 0 || gameBoard.IsOver || gameBoard.GetPossibleMoves().Count == 0)
                return scoreFunction(gameBoard, GameBoard.PLAYER_1_COIN);

            Dictionary<int, double> moveScores = new Dictionary<int, double>();
            foreach(var move in gameBoard.GetPossibleMoves())
            {
                GameBoard newGameBoard = new GameBoard(gameBoard); // copy constructor
                newGameBoard.AddCoin(move, currentPlayer);
                moveScores[move] = Run(newGameBoard, depth - 1, currentPlayer == GameBoard.PLAYER_1_COIN ? GameBoard.PLAYER_2_COIN : GameBoard.PLAYER_1_COIN, scoreFunction);
            }

            KeyValuePair<int, double> selection;

            if (currentPlayer == GameBoard.PLAYER_1_COIN) // MAX PLAYER
                selection = moveScores.Where(item => item.Value == moveScores.Values.Max()).OrderBy(item => random.Next()).First();
            else // MIN PLAYER
                selection = moveScores.Where(item => item.Value == moveScores.Values.Min()).OrderBy(item => random.Next()).First();
            
            SelectedMove = selection.Key;
            return selection.Value;
        }
    }
}
