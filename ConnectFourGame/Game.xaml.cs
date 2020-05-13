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
    /// Logika interakcji dla klasy Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        Grid gameGrid;
        Grid scrollViewerGrid;
        ScrollViewer scrollViewer;
        int scrollViewerRows;
        public Action<int> GameClick;
        public Func<int, GameBoard> GetGameBoard;
        public Game()
        {
            InitializeComponent();
            gameGrid = FindName("GameGrid") as Grid;
            scrollViewerGrid = FindName("ScrollViewerGrid") as Grid;
            scrollViewer = FindName("ScrollViewer") as ScrollViewer;
            scrollViewerRows = 0;
            BoardDrawer.GenerateGameCells(gameGrid, GameBoard.ROWS, GameBoard.COLUMNS);
        }

        public void DrawPossibleMoves(IList<int> possibleMoves)
        {
            var images = gameGrid.Children.OfType<Image>().ToList();
            foreach (var arrow in images)
                gameGrid.Children.Remove(arrow);

            foreach (var move in possibleMoves)
            {
                Image image = new Image
                {
                    Source = new BitmapImage(new Uri(@"../../images/arrow.png", UriKind.Relative))
                };

                gameGrid.Children.Add(image);
                Grid.SetRow(image, 0);
                Grid.SetColumn(image, move + 1);
            }
        }

        private void gameGridClick(object sender, MouseButtonEventArgs args)
        {
            var point = Mouse.GetPosition(gameGrid);

            double totalWidth = 0.0;
            totalWidth += gameGrid.ColumnDefinitions[0].ActualWidth;
            int col = 0;

            foreach(var cell in gameGrid.Children.OfType<DataGridCell>())
            {
                totalWidth += cell.ActualWidth;
                if (totalWidth >= point.X)
                    break;
                col++;
            }

            GameClick(col);
        }

        public void DrawCurrentPlayerBox(int currentPlayer)
        {
            DataGridCell dataGridCell = FindName("NextMoveCell") as DataGridCell;
            dataGridCell.Background = currentPlayer == GameBoard.PLAYER_1_COIN ? Brushes.Red : Brushes.Yellow;
        }

        public void DrawBoard(GameBoard gameBoard)
        {
            BoardDrawer.DrawBoard(gameGrid, gameBoard);
        }

        public void EndGame(int winner, double timeElapsed)
        {
            Grid gameGrid = FindName("GameGrid") as Grid;
            var images = gameGrid.Children.OfType<Image>().ToList();
            foreach (var arrow in images)
                gameGrid.Children.Remove(arrow);

            Grid winnerInfoGrid = new Grid();
            winnerInfoGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });
            winnerInfoGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });

            gameGrid.Children.Add(winnerInfoGrid);
            Grid.SetColumn(winnerInfoGrid, 1);
            Grid.SetRow(winnerInfoGrid, 0);
            Grid.SetColumnSpan(winnerInfoGrid, GameBoard.COLUMNS);

            Label label = new Label();
            label.Content = string.Format("Koniec gry! Zwycięża gracz {0}!", winner);
            label.FontSize = 20;
            label.Foreground = winner == GameBoard.PLAYER_1_COIN ? Brushes.Red : Brushes.Yellow;
            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            label.VerticalContentAlignment = VerticalAlignment.Center;
            winnerInfoGrid.Children.Add(label);
            Grid.SetRow(label, 0);

            Label timeLabel = new Label();
            timeLabel.Content = string.Format("Łączny czas gry {0:0.00}s", timeElapsed);
            timeLabel.FontSize = 14;
            timeLabel.Foreground = Brushes.AntiqueWhite;
            timeLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
            timeLabel.VerticalContentAlignment = VerticalAlignment.Center;
            winnerInfoGrid.Children.Add(timeLabel);
            Grid.SetRow(timeLabel, 1);
        }

        public void AddMoveToList(int column, int player, double timeElapsed)
        {
            scrollViewerGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(35) });

            Label newLabel = new Label();
            newLabel.Content = string.Format("Ruch {0}: {1}", scrollViewerRows + 1, column);
            newLabel.Foreground = player == GameBoard.PLAYER_1_COIN ? Brushes.Red : Brushes.Yellow;
            newLabel.FontSize = 16;
            newLabel.FontWeight = FontWeights.Bold;
            newLabel.PreviewMouseDown += openGameStateWindow;
            scrollViewerGrid.Children.Add(newLabel);
            Grid.SetRow(newLabel, scrollViewerRows);

            Label timeLabel = new Label();
            timeLabel.Content = string.Format("{0:0.00}s", timeElapsed);
            timeLabel.Foreground = player == GameBoard.PLAYER_1_COIN ? Brushes.Red : Brushes.Yellow;
            timeLabel.FontSize = 14;
            timeLabel.HorizontalAlignment = HorizontalAlignment.Right;
            timeLabel.FontWeight = FontWeights.Bold;
            timeLabel.PreviewMouseDown += openGameStateWindow;
            scrollViewerGrid.Children.Add(timeLabel);
            Grid.SetRow(timeLabel, scrollViewerRows);

            scrollViewerRows++;
            scrollViewer.ScrollToEnd();
        }

        public void EndGameWithDraw(double timeElapsed)
        {
            Grid gameGrid = FindName("GameGrid") as Grid;
            var images = gameGrid.Children.OfType<Image>().ToList();
            foreach (var arrow in images)
                gameGrid.Children.Remove(arrow);

            Grid winnerInfoGrid = new Grid();
            winnerInfoGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });
            winnerInfoGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });

            gameGrid.Children.Add(winnerInfoGrid);
            Grid.SetColumn(winnerInfoGrid, 1);
            Grid.SetRow(winnerInfoGrid, 0);
            Grid.SetColumnSpan(winnerInfoGrid, GameBoard.COLUMNS);

            Label label = new Label();
            label.Content = string.Format("Gra zakończyła się remisem!");
            label.FontSize = 20;
            label.Foreground = Brushes.AntiqueWhite;
            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            label.VerticalContentAlignment = VerticalAlignment.Center;
            winnerInfoGrid.Children.Add(label);
            Grid.SetRow(label, 0);

            Label timeLabel = new Label();
            timeLabel.Content = string.Format("Łączny czas gry {0:0.00}s", timeElapsed);
            timeLabel.FontSize = 14;
            timeLabel.Foreground = Brushes.AntiqueWhite;
            timeLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
            timeLabel.VerticalContentAlignment = VerticalAlignment.Center;
            winnerInfoGrid.Children.Add(timeLabel);
            Grid.SetRow(timeLabel, 1);
        }

        private void openGameStateWindow(object sender, RoutedEventArgs args)
        {
            Label label = (Label)sender;
            GameBoard gameBoard = GetGameBoard(Grid.GetRow(label) + 1);
            Board board = new Board(gameBoard);
            board.Owner = this;
            board.Show();
        }
    }
}
