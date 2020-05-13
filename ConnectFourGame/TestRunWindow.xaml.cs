using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ConnectFourGame
{
    /// <summary>
    /// Logika interakcji dla klasy TestRunWindow.xaml
    /// </summary>
    public partial class TestRunWindow : Window
    {
        public TestController TestController;
        public TestRunWindow(TestController testController)
        {
            TestController = testController;
            InitializeComponent();
        }

        public void StartTests()
        {
            
        }

        public void Update(int number)
        {
            Grid grid = FindName("ScrollViewerTestListGrid") as Grid;
            GameOptions gameOptions = TestController.GetGameOptions(number);
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(35) });
            Label newLabel = new Label();
            newLabel.Content = string.Format("{0}: {1} ({2}, {3}) vs {4} ({5}, {6})",
                number + 1,
                getPlayerString(gameOptions.Player1),
                gameOptions.AIPlayerOptions1.Depth,
                getFunctionString(gameOptions.AIPlayerOptions1.Function),
                getPlayerString(gameOptions.Player2),
                gameOptions.AIPlayerOptions2.Depth,
                getFunctionString(gameOptions.AIPlayerOptions2.Function)
            );
            newLabel.Foreground = Brushes.AntiqueWhite;
            newLabel.FontSize = 16;
            newLabel.FontWeight = FontWeights.Bold;
            newLabel.PreviewMouseDown += openDataWindow;
            grid.Children.Add(newLabel);
            Grid.SetRow(newLabel, number);
        }

        private void openDataWindow(object sender, RoutedEventArgs args)
        {
            Label label = (Label)sender;
            IList<GameEngine> testResults = TestController.GetTestResults(Grid.GetRow(label));
            DataWindow dataWindow = new DataWindow(testResults);
            dataWindow.Owner = this;
            dataWindow.Show();
        }

        private string getPlayerString(Player player)
        {
            return player == Player.MinMax ? "MinMax" : "AlfaBeta";
        }

        private string getFunctionString(Function function)
        {
            switch (function)
            {
                case Function.Weighted:
                    return "Weighted";
                case Function.WinLose:
                    return "WinLose";
                case Function.WinningPositions:
                    return "WinningPositions";
            }

            return "";
        }
    }
}
