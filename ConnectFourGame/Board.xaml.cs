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
    /// Logika interakcji dla klasy Board.xaml
    /// </summary>
    public partial class Board : Window
    {
        Grid gameGrid;
        public Board(GameBoard gameBoard)
        {
            InitializeComponent();
            gameGrid = FindName("GameGrid") as Grid;
            BoardDrawer.GenerateGameCells(gameGrid, GameBoard.ROWS, GameBoard.COLUMNS);
            BoardDrawer.DrawBoard(gameGrid, gameBoard);
        }
    }
}
