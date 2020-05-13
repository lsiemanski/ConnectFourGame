using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFourGame
{
    public enum Player
    {
        Human, MinMax, AlfaBeta
    }

    public enum Function
    {
        WinningPositions, WinLose, PromisingPositions, Weighted
    }
    public class GameOptions
    {
        public Player Player1;
        public Player Player2;
        public AIPlayerOptions AIPlayerOptions1;
        public AIPlayerOptions AIPlayerOptions2;
    }

    public class AIPlayerOptions
    {
        public int Depth;
        public Function Function;
        public double Player1WinningPositions;
        public double Player2WinningPositions;
        public double Player1WinningPositionsPower;
        public double Player2WinningPositionsPower;
        public double Player1PromisingPositionsPower;
        public double Player2ProsimingPositionsPower;
        public double WinningPositionsPercentage;
        public double PromisingPositionsPercentage;
    }
}
