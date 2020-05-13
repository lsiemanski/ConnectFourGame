using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFourGame
{
    public class TestController
    {
        TestTool testTool = new TestTool();
        IList<GameOptions> gameOptionsList = new List<GameOptions>();
        IList<int> testsList = new List<int>();
        IList<IList<GameEngine>> testResults = new List<IList<GameEngine>>();

        public void AddTest(GameOptions gameOptions, int tests)
        {
            gameOptionsList.Add(gameOptions);
            testsList.Add(tests);
        }

        public int GetTestNumber()
        {
            return gameOptionsList.Count;
        }

        public GameOptions GetGameOptions(int i)
        {
            return gameOptionsList[i];
        }

        public int GetTests(int i)
        {
            return testsList[i];
        }

        public void RunTests()
        {
            TestRunWindow testRunWindow = new TestRunWindow(this);
            testRunWindow.Show();
            TestToolObserver testToolObserver = new TestToolObserver(testTool, gameOptionsList, testsList, testRunWindow, this);
            testTool.Subscribe(testToolObserver);
            testToolObserver.Start();
        }

        public IList<GameEngine> GetTestResults(int number)
        {
            return testResults[number];
        }

        public void AddResults(IList<GameEngine> testResults)
        {
            this.testResults.Add(testResults);
        }
    }

    class TestToolObserver : IObserver<TestTool>
    {
        TestTool TestTool;
        IList<GameOptions> GameOptionsList;
        IList<int> TestsList;
        TestRunWindow TestRunWindow;
        TestController TestController;
        int currentOptions = 0;

        public TestToolObserver(TestTool testTool, IList<GameOptions> gameOptionsList, IList<int> testsList, TestRunWindow testRunWindow, TestController testController)
        {
            TestTool = testTool;
            GameOptionsList = gameOptionsList;
            TestsList = testsList;
            TestRunWindow = testRunWindow;
            TestController = testController;
        }

        public void Start()
        {
            TestTool.Run(GameOptionsList[0], TestsList[0]);
        }

        public void OnCompleted()
        {
            TestRunWindow.Update(currentOptions);
            TestController.AddResults(TestTool.testResults);
            if (currentOptions <= GameOptionsList.Count)
                TestTool.Run(GameOptionsList[++currentOptions], TestsList[currentOptions]);
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(TestTool value)
        {
            throw new NotImplementedException();
        }
    }
}
