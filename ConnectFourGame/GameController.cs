using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFourGame
{
    public class GameController : IObserver<GameBoard>
    {
        Game GameWindow;
        GameEngine GameEngine; 

        public GameController(GameOptions gameOptions)
        {
            GameEngine = new GameEngine(gameOptions);
        }

        public void StartGame()
        {
            GameWindow = new Game();
            GameWindow.GameClick = addMove;
            GameWindow.GetGameBoard = getGameBoard;
            GameWindow.DrawPossibleMoves(GameEngine.GetPossibleMoves());
            GameWindow.DrawCurrentPlayerBox(GameEngine.GetCurrentPlayer());

            GameWindow.Show();
            GameEngine.GetCurrentGameBoardObservable().Subscribe(this);
            GameEngine.Start();
        }

        private GameBoard getGameBoard(int number)
        {
            return GameEngine.GetGameBoard(number);
        }

        private async void addMove(int column)
        {
            if (GameEngine.GetCurrentPlayerType() == Player.Human)
                await GameEngine.AddMove(column);
        }

        public void OnNext(GameBoard gameBoard)
        {
            GameWindow.DrawBoard(GameEngine.GetCurrentBoard());
            GameWindow.AddMoveToList(GameEngine.GetLastMove(), GameEngine.GetCurrentPlayer(), GameEngine.GetMoveTime());

            if (GameEngine.IsGameOver())
                GameWindow.EndGame(GameEngine.GetCurrentPlayer(), GameEngine.GetWholeGameTime());
            else if (GameEngine.IsGameDrawn())
                GameWindow.EndGameWithDraw(GameEngine.GetWholeGameTime());
            else // play on
            {
                GameWindow.DrawPossibleMoves(GameEngine.GetPossibleMoves());
                GameWindow.DrawCurrentPlayerBox(GameEngine.GetCurrentPlayer());
            }
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {

        }
    }
}
