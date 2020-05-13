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
    /// Logika interakcji dla klasy TestListWindow.xaml
    /// </summary>
    public partial class TestListWindow : Window
    {
        public TestController TestController;
        public TestListWindow(TestController testController)
        {
            TestController = testController;
            InitializeComponent();
            initTestList();
        }

        private void initTestList()
        {
            Grid grid = FindName("ScrollViewerTestListGrid") as Grid;
            for(int i = 0; i < TestController.GetTestNumber(); i++)
            {
                GameOptions gameOptions = TestController.GetGameOptions(i);
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(35) });
                Label newLabel = new Label();
                newLabel.Content = string.Format("{0}: {1} ({2}, {3}) vs {4} ({5}, {6})", 
                    i+1,
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
                grid.Children.Add(newLabel);
                Grid.SetRow(newLabel, i);

                Label testsLabel = new Label();
                testsLabel.Content = string.Format("{0}", TestController.GetTests(i));
                testsLabel.Foreground = Brushes.AntiqueWhite;
                testsLabel.FontSize = 14;
                testsLabel.HorizontalAlignment = HorizontalAlignment.Right;
                testsLabel.FontWeight = FontWeights.Bold;
                grid.Children.Add(testsLabel);
                Grid.SetRow(testsLabel, i);
            }
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
