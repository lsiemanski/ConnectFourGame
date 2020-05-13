using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConnectFourGame.Algorithm;

namespace ConnectFourGame
{
    public class GameEngine
    {
        GameOptions GameOptions;
        IList<GameBoard> GameBoards;
        IList<double> MoveTimes;
        GameBoard CurrentGameBoard;
        ObservableBoard observableBoard;
        int CurrentPlayer;
        int LastMove;
        Algorithm Player1Algorithm;
        Algorithm Player2Algorithm;
        Stopwatch stopwatch;
        Stopwatch wholeGameStopwatch;
        public GameEngine(GameOptions gameOptions)
        {
            GameOptions = gameOptions;
            GameBoards = new List<GameBoard>();
            MoveTimes = new List<double>();
            CurrentGameBoard = new GameBoard();
            observableBoard = new ObservableBoard();
            GameBoards.Add(CurrentGameBoard);
            CurrentPlayer = GameBoard.PLAYER_1_COIN;

            stopwatch = new Stopwatch();
            wholeGameStopwatch = new Stopwatch();
            initAlgorithms(gameOptions);
        }

        class ObservableBoard : IObservable<GameBoard>, IDisposable
        {
            IObserver<GameBoard> observer;

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public void updateBoard(GameBoard gameBoard)
            {
                observer.OnNext(gameBoard);
            }

            public void gameFinished()
            {
                observer.OnCompleted();
            }

            public IDisposable Subscribe(IObserver<GameBoard> observer)
            {
                this.observer = observer;
                return this;
            }
        }

        public IList<double> GetMoveTimes()
        {
            return MoveTimes;
        }

        public int GetLastMove()
        {
            return LastMove;
        }

        public IObservable<GameBoard> GetCurrentGameBoardObservable()
        {
            return observableBoard;
        }

        public async void Start()
        {
            stopwatch.Start();
            wholeGameStopwatch.Start();

            if (GetCurrentPlayerType() != Player.Human)
            {
                int AIMove = await performAIMove();
                _ = AddMove(AIMove);
            }
        }

        public int GetCurrentPlayer()
        {
            return CurrentPlayer;
        }

        public IList<int> GetPossibleMoves()
        {
            return CurrentGameBoard.GetPossibleMoves();
        }

        public GameBoard GetGameBoard(int number)
        {
            return GameBoards[number];
        }

        public Player GetCurrentPlayerType()
        {
            if (CurrentPlayer == GameBoard.PLAYER_1_COIN)
                return GameOptions.Player1;
            else
                return GameOptions.Player2;
        }

        public async Task<bool> AddMove(int move)
        {
            GameBoard updatedGameBoard = new GameBoard(CurrentGameBoard);
            if (updatedGameBoard.AddCoin(move, CurrentPlayer))
            {
                LastMove = move;
                GameBoards.Add(updatedGameBoard);
                CurrentGameBoard = updatedGameBoard;
                observableBoard.updateBoard(CurrentGameBoard);
                MoveTimes.Add(stopwatch.Elapsed.TotalSeconds);
                stopwatch.Restart();

                if(IsGameOver() || IsGameDrawn())
                {
                    stopwatch.Stop();
                    wholeGameStopwatch.Stop();
                    observableBoard.gameFinished();
                }
                else
                {
                    switchPlayer();
                    if(GetCurrentPlayerType() != Player.Human)
                    {
                        int AIMove = await performAIMove();
                        _ = AddMove(AIMove);
                    }
                }

                return true;
            }

            return false;
        }

        public GameBoard GetCurrentBoard()
        {
            return CurrentGameBoard;
        }

        public double GetMoveTime()
        {
            return stopwatch.Elapsed.TotalSeconds;
        }

        public bool IsGameOver()
        {
            return CurrentGameBoard.IsOver;
        }

        public bool IsGameDrawn()
        {
            return CurrentGameBoard.GetPossibleMoves().Count == 0;
        }

        public double GetWholeGameTime()
        {
            return wholeGameStopwatch.Elapsed.TotalSeconds;
        }

        private void initAlgorithms(GameOptions gameOptions)
        {
            switch (gameOptions.Player1)
            {
                case Player.AlfaBeta:
                    Player1Algorithm = new AlphaBetaAlgorithm();
                    break;
                case Player.MinMax:
                    Player1Algorithm = new MinimaxAlgorithm();
                    break;
            }

            switch (gameOptions.Player2)
            {
                case Player.AlfaBeta:
                    Player2Algorithm = new AlphaBetaAlgorithm();
                    break;
                case Player.MinMax:
                    Player2Algorithm = new MinimaxAlgorithm();
                    break;
            }
        }

        private AIPlayerOptions getAIPlayerOptions()
        {
            if (CurrentPlayer == GameBoard.PLAYER_1_COIN)
                return GameOptions.AIPlayerOptions1;
            else
                return GameOptions.AIPlayerOptions2;
        }

        private Score getHeuristicFunction(Algorithm algorithm)
        {
            AIPlayerOptions aIPlayerOptions = getAIPlayerOptions();
            switch(aIPlayerOptions.Function)
            {
                case Function.WinningPositions:
                    return algorithm.WinningPositionsScore;
                case Function.PromisingPositions:
                    return algorithm.PromisingPositionsScore;
                case Function.WinLose:
                    return WinLoseScore;
                case Function.Weighted:
                    return algorithm.WeightedScore;
            }

            return algorithm.WinningPositionsScore;
        }


        private void switchPlayer()
        {
            CurrentPlayer = CurrentPlayer == GameBoard.PLAYER_1_COIN ? GameBoard.PLAYER_2_COIN : GameBoard.PLAYER_1_COIN;
        }

        private async Task<int> performAIMove()
        {
            return await Task.Run(() =>
            {
                Algorithm currentAlgorithm;

                if (CurrentPlayer == GameBoard.PLAYER_1_COIN)
                    currentAlgorithm = Player1Algorithm;
                else
                    currentAlgorithm = Player2Algorithm;

                currentAlgorithm.Run(CurrentGameBoard, getAIPlayerOptions().Depth, CurrentPlayer, getHeuristicFunction(currentAlgorithm));
                return currentAlgorithm.SelectedMove;
            });
        }
    }
}
