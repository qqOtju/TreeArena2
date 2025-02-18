using Project.Scripts.GameLogic.Wave;
using UnityEngine;

namespace Project.Scripts.Config.Wave
{
    [CreateAssetMenu(menuName = "Config/Wave", fileName = "Wave Config")]
    public class WaveConfig: ScriptableObject
    {
        [SerializeField] private WaveContent[] _spawns;
        
        public WaveContent[] Spawns => _spawns;
    }
}