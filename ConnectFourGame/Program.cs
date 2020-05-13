using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFourGame
{
    class Program
    {
        public static void Main(string[] args)
        {
            //Task.Run(async () => { DoYourJob(); }).GetAwaiter().GetResult();
            DoYourJob();
        }

        public static void DoYourJob()
        {
            TestTool testTool = new TestTool();
            IList<GameOptions> gameOptionsList = new List<GameOptions>();
            IList<int> testsList = new List<int>();
            addToLists(gameOptionsList, testsList, testTool.GenerateGameOptions(Player.AlfaBeta, Player.AlfaBeta, 1, 1, Function.WinningPositions, Function.WinningPositions), 20);
            addToLists(gameOptionsList, testsList, testTool.GenerateGameOptions(Player.AlfaBeta, Player.AlfaBeta, 2, 2, Function.WinningPositions, Function.WinningPositions), 20);
            addToLists(gameOptionsList, testsList, testTool.GenerateGameOptions(Player.AlfaBeta, Player.AlfaBeta, 3, 3, Function.WinningPositions, Function.WinningPositions), 20);
            addToLists(gameOptionsList, testsList, testTool.GenerateGameOptions(Player.AlfaBeta, Player.AlfaBeta, 4, 4, Function.WinningPositions, Function.WinningPositions), 20);
            addToLists(gameOptionsList, testsList, testTool.GenerateGameOptions(Player.AlfaBeta, Player.AlfaBeta, 5, 5, Function.WinningPositions, Function.WinningPositions), 20);
            addToLists(gameOptionsList, testsList, testTool.GenerateGameOptions(Player.AlfaBeta, Player.AlfaBeta, 6, 6, Function.WinningPositions, Function.WinningPositions), 20);
            addToLists(gameOptionsList, testsList, testTool.GenerateGameOptions(Player.AlfaBeta, Player.AlfaBeta, 7, 7, Function.WinningPositions, Function.WinningPositions), 20);
            addToLists(gameOptionsList, testsList, testTool.GenerateGameOptions(Player.AlfaBeta, Player.AlfaBeta, 8, 8, Function.WinningPositions, Function.WinningPositions), 20);
            addToLists(gameOptionsList, testsList, testTool.GenerateGameOptions(Player.MinMax, Player.MinMax, 1, 1, Function.WinningPositions, Function.WinningPositions), 20);
            addToLists(gameOptionsList, testsList, testTool.GenerateGameOptions(Player.MinMax, Player.MinMax, 2, 2, Function.WinningPositions, Function.WinningPositions), 20);
            addToLists(gameOptionsList, testsList, testTool.GenerateGameOptions(Player.MinMax, Player.MinMax, 3, 3, Function.WinningPositions, Function.WinningPositions), 20);
            addToLists(gameOptionsList, testsList, testTool.GenerateGameOptions(Player.MinMax, Player.MinMax, 4, 4, Function.WinningPositions, Function.WinningPositions), 20);
            addToLists(gameOptionsList, testsList, testTool.GenerateGameOptions(Player.MinMax, Player.MinMax, 5, 5, Function.WinningPositions, Function.WinningPositions), 20);
            addToLists(gameOptionsList, testsList, testTool.GenerateGameOptions(Player.MinMax, Player.MinMax, 6, 6, Function.WinningPositions, Function.WinningPositions), 20);
            addToLists(gameOptionsList, testsList, testTool.GenerateGameOptions(Player.MinMax, Player.MinMax, 7, 7, Function.WinningPositions, Function.WinningPositions), 20);
            addToLists(gameOptionsList, testsList, testTool.GenerateGameOptions(Player.MinMax, Player.MinMax, 8, 8, Function.WinningPositions, Function.WinningPositions), 1);
            addToLists(gameOptionsList, testsList, testTool.GenerateGameOptions(Player.AlfaBeta, Player.AlfaBeta, 9, 9, Function.WinningPositions, Function.WinningPositions), 20);
            addToLists(gameOptionsList, testsList, testTool.GenerateGameOptions(Player.AlfaBeta, Player.AlfaBeta, 10, 10, Function.WinningPositions, Function.WinningPositions), 1);
            //TestToolObserver testToolObserver = new TestToolObserver(testTool, gameOptionsList, testsList);
            //testTool.Subscribe(testToolObserver);
            //testToolObserver.Start();
            Console.ReadKey();
        }

        private static void addToLists(IList<GameOptions> gameOptionsList, IList<int> testsList, GameOptions gameOptions, int tests)
        {
            gameOptionsList.Add(gameOptions);
            testsList.Add(tests);
        }
    }
}
