using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConnectFourGame
{
    class BoardDrawer
    {
        public static void GenerateGameCells(Grid gameGrid, int rows, int columns)
        {
            for (int i = 1; i <= rows; i++)
            {
                for (int j = 1; j <= columns; j++)
                {
                    addDataGridCell(gameGrid, i, j);
                }
            }
        }

        private static void addDataGridCell(Grid grid, int row, int column)
        {
            DataGridCell dataGridCell = new DataGridCell
            {
                BorderBrush = Brushes.LightSkyBlue,
                BorderThickness = new Thickness(7)
            };

            grid.Children.Add(dataGridCell);
            Grid.SetColumn(dataGridCell, column);
            Grid.SetRow(dataGridCell, row);
        }

        public static void DrawBoard(Grid gameGrid, GameBoard gameBoard)
        {
            IList<DataGridCell> cells = gameGrid.Children.OfType<DataGridCell>().ToList();
            for (int i = 0; i < GameBoard.ROWS; i++)
            {
                for (int j = 0; j < GameBoard.COLUMNS; j++)
                {
                    DataGridCell cell = cells.Where(item => Grid.GetRow(item) == i + 1 && Grid.GetColumn(item) == j + 1).First();
                    if (gameBoard.Slots[i, j] == GameBoard.PLAYER_1_COIN)
                        cell.Background = Brushes.Red;
                    else if (gameBoard.Slots[i, j] == GameBoard.PLAYER_2_COIN)
                        cell.Background = Brushes.Yellow;
                }
            }
        }
    }
}
