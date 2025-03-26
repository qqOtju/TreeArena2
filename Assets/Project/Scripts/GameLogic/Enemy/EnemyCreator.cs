using Project.Scripts.Config.Enemy;
using Project.Scripts.Entity;
using Project.Scripts.Module.Spawner;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GameLogic.Enemy
{
    //Will spawn enemies every few seconds
    public class EnemyCreator: EnemyBase
    {
        [SerializeField] private EnemyData _childEnemyData;
        [SerializeField] private Transform _spawnPoint;

        private EnemySpawner _enemySpawner;
        
        [Inject]
        private void Construct(EnemySpawner enemySpawner)
        {
            _enemySpawner = enemySpawner;
        }
        
        protected override void Attack(IHealth health)
        {
            _enemySpawner.SpawnEnemy(_spawnPoint.transform.position, _childEnemyData);
        }
    }
}