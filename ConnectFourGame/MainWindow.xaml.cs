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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConnectFourGame
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string AI_PLAYER = "AI";
        private static string HUMAN_PLAYER = "Człowiek";
        private static string AI_OPTIONS1 = "AIOptions1";
        private static string AI_OPTIONS2 = "AIOptions2";
        private static string PLAYER1_BUTTON_NAME = "Player1Button";
        private static string PLAYER2_BUTTON_NAME = "Player2Button";

        private TestController testController;
        public MainWindow()
        {
            testController = new TestController();
            InitializeComponent();
        }

        void PlayerSelectButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if(button.Content.Equals(HUMAN_PLAYER))
                button.Content = AI_PLAYER;
            else
                button.Content = HUMAN_PLAYER;

            if (button.Name.Equals(PLAYER1_BUTTON_NAME))
                toggleOptionsPanel(AI_OPTIONS1);
            else
                toggleOptionsPanel(AI_OPTIONS2);
            
        }

        void AIOptionsMinusClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            TextBlock textBlock;
            if (button.Name.Equals("AIOptions1Minus"))
                textBlock = FindName("AIOptions1Depth") as TextBlock;
            else
                textBlock = FindName("AIOptions2Depth") as TextBlock;

            int depthValue = int.Parse(textBlock.Text);

            if(depthValue > 1)
                textBlock.Text = (depthValue - 1).ToString();
            
        }

        void AIOptionsPlusClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            TextBlock textBlock;
            if (button.Name.Equals("AIOptions1Plus"))
                textBlock = FindName("AIOptions1Depth") as TextBlock;
            else
                textBlock = FindName("AIOptions2Depth") as TextBlock;

            int depthValue = int.Parse(textBlock.Text);
            textBlock.Text = (depthValue + 1).ToString();
        }

        void TestNumberMinusClick(object sender, RoutedEventArgs e)
        {
            TextBlock textBlock = FindName("TestNumber") as TextBlock;
            int value = int.Parse(textBlock.Text);

            if(value > 1)
                textBlock.Text = (value - 1).ToString();
        }
        void TestNumberPlusClick(object sender, RoutedEventArgs e)
        {
            TextBlock textBlock = FindName("TestNumber") as TextBlock;
            int value = int.Parse(textBlock.Text);
            textBlock.Text = (value + 1).ToString();
        }
        
        void StartClick(object sender, RoutedEventArgs e)
        {
            GameController gameController = new GameController(createGameOptions());
            gameController.StartGame();
        }

        void AddTestClick(object sender, RoutedEventArgs e)
        {
            TextBlock textBlock = FindName("TestNumber") as TextBlock;
            testController.AddTest(createGameOptions(), int.Parse(textBlock.Text));
        }

        void ShowTestsClick(object sender, RoutedEventArgs e)
        {
            TestListWindow testListWindow = new TestListWindow(testController);
            testListWindow.Show();
        }

        void RunTestsClick(object sender, RoutedEventArgs e)
        {
            testController.RunTests();
        }

        void Function1Changed(object sender, RoutedEventArgs e)
        {
            if (!(sender as ComboBox).IsDropDownOpen)
                return;
            
            Function function = getFunction(1);
            DockPanel winningPositionsWeights = FindName("Options1WinningPositionsWeights") as DockPanel;
            DockPanel promisingPositionsWeights = FindName("Options1PromisingPositionsWeights") as DockPanel;
            DockPanel weightedWeights = FindName("Options1WeightedWeights") as DockPanel;
            switch(function)
            {
                case Function.WinLose:
                    winningPositionsWeights.Visibility = Visibility.Hidden;
                    promisingPositionsWeights.Visibility = Visibility.Hidden;
                    weightedWeights.Visibility = Visibility.Hidden;
                    break;
                case Function.PromisingPositions:
                    winningPositionsWeights.Visibility = Visibility.Hidden;
                    promisingPositionsWeights.Visibility = Visibility.Visible;
                    weightedWeights.Visibility = Visibility.Hidden;
                    break;
                case Function.WinningPositions:
                    winningPositionsWeights.Visibility = Visibility.Visible;
                    promisingPositionsWeights.Visibility = Visibility.Hidden;
                    weightedWeights.Visibility = Visibility.Hidden;
                    break;
                case Function.Weighted:
                    winningPositionsWeights.Visibility = Visibility.Visible;
                    promisingPositionsWeights.Visibility = Visibility.Visible;
                    weightedWeights.Visibility = Visibility.Visible;
                    break;
            }
        }

        void Function2Changed(object sender, RoutedEventArgs e)
        {
            if (!(sender as ComboBox).IsDropDownOpen)
                return;

            Function function = getFunction(2);
            DockPanel winningPositionsWeights = FindName("Options2WinningPositionsWeights") as DockPanel;
            DockPanel promisingPositionsWeights = FindName("Options2PromisingPositionsWeights") as DockPanel;
            DockPanel weightedWeights = FindName("Options2WeightedWeights") as DockPanel;
            switch (function)
            {
                case Function.WinLose:
                    winningPositionsWeights.Visibility = Visibility.Hidden;
                    promisingPositionsWeights.Visibility = Visibility.Hidden;
                    weightedWeights.Visibility = Visibility.Hidden;
                    break;
                case Function.PromisingPositions:
                    winningPositionsWeights.Visibility = Visibility.Hidden;
                    promisingPositionsWeights.Visibility = Visibility.Visible;
                    weightedWeights.Visibility = Visibility.Hidden;
                    break;
                case Function.WinningPositions:
                    winningPositionsWeights.Visibility = Visibility.Visible;
                    promisingPositionsWeights.Visibility = Visibility.Hidden;
                    weightedWeights.Visibility = Visibility.Hidden;
                    break;
                case Function.Weighted:
                    winningPositionsWeights.Visibility = Visibility.Visible;
                    promisingPositionsWeights.Visibility = Visibility.Visible;
                    weightedWeights.Visibility = Visibility.Visible;
                    break;
            }
        }
        private void toggleOptionsPanel(string name)
        {
            DockPanel dockPanel = FindName(name) as DockPanel;
            if (dockPanel.Visibility == Visibility.Visible)
                dockPanel.Visibility = Visibility.Hidden;
            else
                dockPanel.Visibility = Visibility.Visible;
        }

        private Player getPlayer(int playerNumber)
        {
            if (playerNumber > 2)
                return Player.Human;

            if(playerNumber == 1)
            {
                return findPlayer(PLAYER1_BUTTON_NAME, "AlgorithmOptions1");
            }
            else
            {
                return findPlayer(PLAYER2_BUTTON_NAME, "AlgorithmOptions2");
            }
        }

        private Player findPlayer(string playerButtonName, string comboBoxName)
        {
            if ((FindName(playerButtonName) as Button).Content.Equals(HUMAN_PLAYER))
                return Player.Human;
            else
            {
                string selectedAlgorithm = ((FindName(comboBoxName) as ComboBox).SelectedItem as ComboBoxItem).Content.ToString();
                if (selectedAlgorithm.Equals("MinMax"))
                    return Player.MinMax;
                else if (selectedAlgorithm.Equals("Alfa Beta"))
                    return Player.AlfaBeta;
                else
                    return Player.MinMax;
            }
        }

        private int getDepth(int optionsNumber)
        {
            if (optionsNumber > 2)
                return 0;

            TextBlock textBlock;

            if(optionsNumber == 1)
                textBlock = FindName("AIOptions1Depth") as TextBlock;
            else
                textBlock = FindName("AIOptions2Depth") as TextBlock;

            return int.Parse(textBlock.Text);
        }

        private GameOptions createGameOptions()
        {
            Player player1 = getPlayer(1);
            Player player2 = getPlayer(2);

            GameOptions gameOptions = new GameOptions()
            {
                Player1 = getPlayer(1),
                Player2 = getPlayer(2)
            };

            if (player1 != Player.Human)
                gameOptions.AIPlayerOptions1 = generateAIPlayerOptions(1);

            if (player2 != Player.Human)
                gameOptions.AIPlayerOptions2 = generateAIPlayerOptions(2);

            return gameOptions;
        }

        private AIPlayerOptions generateAIPlayerOptions(int playerNumber)
        {
            return new AIPlayerOptions()
            {
                Depth = getDepth(playerNumber),
                Function = getFunction(playerNumber),
                Player1PromisingPositionsPower = getPlayerPromisingPositionsPower(playerNumber, 1),
                Player2ProsimingPositionsPower = getPlayerPromisingPositionsPower(playerNumber, 2),
                Player1WinningPositions = getPlayerWinningPositionsWeight(playerNumber, 1),
                Player2WinningPositions = getPlayerWinningPositionsWeight(playerNumber, 2),
                Player1WinningPositionsPower = getPlayerWinningPositionsPower(playerNumber, 1),
                Player2WinningPositionsPower = getPlayerWinningPositionsPower(playerNumber, 2),
                WinningPositionsPercentage = getWinningPositionsPercentage(playerNumber),
                PromisingPositionsPercentage = getPromisingPositionsPercentage(playerNumber)
            };
        }

        private double getPromisingPositionsPercentage(int playerNumber)
        {
            TextBox textBox = FindName(string.Format("Options{0}PromisingPositionsWeight", playerNumber)) as TextBox;
            return double.Parse(textBox.Text, System.Globalization.CultureInfo.InvariantCulture);
        }

        private double getWinningPositionsPercentage(int playerNumber)
        {
            TextBox textBox = FindName(string.Format("Options{0}WinningPositionsWeight", playerNumber)) as TextBox;
            return double.Parse(textBox.Text, System.Globalization.CultureInfo.InvariantCulture);
        }

        private double getPlayerWinningPositionsPower(int optionsNumber, int playerNumber)
        {
            TextBox textBox = FindName(string.Format("Options{0}Player{1}Power", optionsNumber, playerNumber)) as TextBox;
            return double.Parse(textBox.Text, System.Globalization.CultureInfo.InvariantCulture);
        }

        private double getPlayerWinningPositionsWeight(int optionsNumber, int playerNumber)
        {
            TextBox textBox = FindName(string.Format("Options{0}Player{1}Weight", optionsNumber, playerNumber)) as TextBox;
            return double.Parse(textBox.Text, System.Globalization.CultureInfo.InvariantCulture);
        }

        private double getPlayerPromisingPositionsPower(int optionsNumber, int playerNumber)
        {
            TextBox textBox = FindName(string.Format("Options{0}PromisingPlayer{1}Weight", optionsNumber, playerNumber)) as TextBox;
            return double.Parse(textBox.Text, System.Globalization.CultureInfo.InvariantCulture); 
        }

        private Function getFunction(int playerNumber)
        {
            if (playerNumber == 1)
                return getFunction("Function1");
            else if (playerNumber == 2)
                return getFunction("Function2");

            return Function.WinLose;
        }

        private Function getFunction(string comboBoxName)
        {
            string selectedAlgorithm = ((FindName(comboBoxName) as ComboBox).SelectedItem as ComboBoxItem).Content.ToString();
            if (selectedAlgorithm.Equals("Wygrywające pozycje"))
                return Function.WinningPositions;
            else if (selectedAlgorithm.Equals("Wygrana/przegrana"))
                return Function.WinLose;
            else if (selectedAlgorithm.Equals("Obiecujące pozycje"))
                return Function.PromisingPositions;
            else
                return Function.Weighted;
        }
    }
}
