using System;

namespace Project.Scripts.GameLogic.Wave
{
    [Flags]
    public enum SpawnPoint
    {
        Left = 1 << 0, //1
        BotLeft = 1 << 1, //2
        Right = 1 << 2,  //4
        BotRight = 1 << 3, //8
        Bottom = 1 << 4 //16
    }
}
