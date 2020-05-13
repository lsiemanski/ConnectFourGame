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
    /// Logika interakcji dla klasy DataWindow.xaml
    /// </summary>
    public partial class DataWindow : Window
    {
        public DataWindow(IList<GameEngine> testResults)
        {
            InitializeComponent();
            initLabels(testResults);
        }

        private void initLabels(IList<GameEngine> testResults)
        {
            Label player1Wins = FindName("Player1Wins") as Label;
            Label draws = FindName("Draws") as Label;
            Label player2Wins = FindName("Player2Wins") as Label;
            Label averageMoves = FindName("AverageMoves") as Label;
            Label averagePlayer1MoveTime = FindName("AveragePlayer1MoveTime") as Label;
            Label averagePlayer2MoveTime = FindName("AveragePlayer2MoveTime") as Label;

            player1Wins.Content = string.Format("{0:0.00}%", getPlayer1Wins(testResults));
            draws.Content = string.Format("{0:0.00}%", getDraws(testResults));
            player2Wins.Content = string.Format("{0:0.00}%", getPlayer2Wins(testResults));
            averageMoves.Content += string.Format("{0:0.00}", getAverageMoves(testResults));
            averagePlayer1MoveTime.Content += string.Format("{0:0.00000}", getAveragePlayer1MoveTime(testResults));
            averagePlayer2MoveTime.Content += string.Format("{0:0.00000}", getAveragePlayer2MoveTime(testResults));
        }

        private double getAveragePlayer1MoveTime(IList<GameEngine> testResults)
        {
            return testResults.Average(item => item.GetMoveTimes().Where((elem, index) => index % 2 == 0).Average());
        }

        private double getAveragePlayer2MoveTime(IList<GameEngine> testResults)
        {
            return testResults.Average(item => item.GetMoveTimes().Where((elem, index) => index % 2 == 1).Average());
        }

        private double getAverageMoves(IList<GameEngine> testResults)
        {
            return testResults.Average(item => item.GetMoveTimes().Count);
        }

        private double getPlayer1Wins(IList<GameEngine> testResults)
        {
            return ((double)testResults.Count(item => item.GetMoveTimes().Count < 42 && item.GetCurrentPlayer() == GameBoard.PLAYER_1_COIN)
                / testResults.Count) * 100.0;
        }

        private double getDraws(IList<GameEngine> testResults)
        {
            return ((double)testResults.Count(item => item.GetMoveTimes().Count == 42)
                / testResults.Count) * 100.0;
        }

        private double getPlayer2Wins(IList<GameEngine> testResults)
        {
            return ((double)testResults.Count(item => item.GetMoveTimes().Count < 42 && item.GetCurrentPlayer() == GameBoard.PLAYER_2_COIN)
                / testResults.Count) * 100.0;
        }
    }
}
