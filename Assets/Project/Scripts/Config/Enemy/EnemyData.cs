using Project.Scripts.GameLogic.Enemy;
using UnityEngine;

namespace Project.Scripts.Config.Enemy
{
    [CreateAssetMenu(menuName = "Data/Enemy", fileName = "Enemy Data")]
    public class EnemyData: ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private EnemyBase _enemyPrefab;
        [SerializeField] private EnemyConfig _enemyConfig;
        
        public Sprite Icon => _icon;
        public string Name => _name;
        public string Description => _description;
        public EnemyBase EnemyPrefab => _enemyPrefab;
        public EnemyConfig EnemyConfig => _enemyConfig;
    }
}