using Project.Scripts.Config.Wisp;

namespace Project.Scripts.GameLogic.GameCycle
{
    public class GameData
    {
        public WispData ChosenWisp { get; set; }
        
        public GameData(WispData chosenWisp)
        {
            ChosenWisp = chosenWisp;
        }
    }
}