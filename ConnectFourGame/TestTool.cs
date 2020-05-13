using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConnectFourGame
{
    class TestTool : IObservable<TestTool>
    {
        class GameBoardObserver : IObserver<GameBoard>
        {
            public GameEngine Current;
            public GameEngine Next;
            public int Number;
            public Logger logger;

            public GameBoardObserver(GameEngine current, GameEngine next, int number)
            {
                Current = current;
                Next = next;
                Number = number;
            }
            public void OnCompleted()
            {
                //Console.WriteLine(Number);
                //Console.WriteLine(Current.GetMoveTimes().Count);
                if (Next != null)
                    Next.Start();
                else
                    logger.LogToFile();
            }

            public void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public void OnNext(GameBoard gameBoard)
            {
                    
            }
        }

        public IObserver<TestTool> observer;
        public IList<GameEngine> testResults;

        public void Run(GameOptions gameOptions, int tests)
        {
            testResults = new List<GameEngine>();
            for (int i = 0; i < tests; i++)
            {
                GameEngine gameEngine = new GameEngine(gameOptions);
                testResults.Add(gameEngine);
            }

            GameBoardObserver gameBoardObserver;

            for (int i = 0; i < tests - 1; i++)
            {
                gameBoardObserver = new GameBoardObserver(testResults[i], testResults[i + 1], i);
                testResults[i].GetCurrentGameBoardObservable().Subscribe(gameBoardObserver);
            }

            gameBoardObserver = new GameBoardObserver(testResults[tests - 1], null, tests - 1);
            gameBoardObserver.logger = new Logger(gameOptions, testResults, observer);
            testResults[tests - 1].GetCurrentGameBoardObservable().Subscribe(gameBoardObserver);

            testResults[0].Start();
        }

        class Logger
        {
            GameOptions gameOptions;
            IList<GameEngine> engines;
            IObserver<TestTool> observer;
            public Logger(GameOptions gameOptions, IList<GameEngine> engines, IObserver<TestTool> observer)
            {
                this.gameOptions = gameOptions;
                this.engines = engines;
                this.observer = observer;
            }

            public void LogToFile()
            {
                string filename = string.Format("{0}_{1}({2},{3})vs{4}({5},{6}).txt",
                    DateTime.Now.ToString("yyyyMMddhhmmss"),
                    getPlayerString(gameOptions.Player1),
                    gameOptions.AIPlayerOptions1.Depth,
                    getFunctionString(gameOptions.AIPlayerOptions1.Function),
                    getPlayerString(gameOptions.Player2),
                    gameOptions.AIPlayerOptions2.Depth,
                    getFunctionString(gameOptions.AIPlayerOptions2.Function));

                using (StreamWriter sw = new StreamWriter(filename))
                {
                    for (int i = 0; i < engines.Count; i++)
                    {
                        GameEngine current = engines[i];
                        IList<double> moveTimes = current.GetMoveTimes();
                        moveTimes.Where((item, index) => index % 2 == 0).Average();
                        sw.WriteLine(string.Format("{0};{1};{2:0.00000};{3:0.00000};{4}",
                            i + 1,
                            moveTimes.Count == 42 ? 0 : current.GetCurrentPlayer(),
                            moveTimes.Where((item, index) => index % 2 == 0).Average(),
                            moveTimes.Where((item, index) => index % 2 == 1).Average(),
                            moveTimes.Count));
                    }
                }

                Console.WriteLine("DONE {0}", filename);
                observer.OnCompleted();
            }

            private string getPlayerString(Player player)
            {
                return player == Player.MinMax ? "MinMax" : "AlfaBeta";
            }

            private string getFunctionString(Function function)
            {
                switch(function)
                {
                    case Function.Weighted:
                        return "Weighted";
                    case Function.PromisingPositions:
                        return "PromisingPositions";
                    case Function.WinLose:
                        return "WinLose";
                    case Function.WinningPositions:
                        return "WinningPositions";
                }

                return "";
            }
        }
        

        public GameOptions GenerateGameOptions(Player player1, Player player2, int depth1, int depth2, Function function1, Function function2)
        {
            return new GameOptions
            {
                Player1 = player1,
                Player2 = player2,
                AIPlayerOptions1 = new AIPlayerOptions
                {
                    Depth = depth1,
                    Function = function1
                },
                AIPlayerOptions2 = new AIPlayerOptions
                {
                    Depth = depth2,
                    Function = function2
                }
            };
        }

        public IDisposable Subscribe(IObserver<TestTool> observer)
        {
            this.observer = observer;
            return null;
        }
    }
}
