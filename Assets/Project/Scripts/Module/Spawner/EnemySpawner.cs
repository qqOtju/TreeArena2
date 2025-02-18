using System;
using System.Collections.Generic;
using Project.Scripts.Config.Enemy;
using Project.Scripts.Debug;
using Project.Scripts.GameLogic.Enemy;
using Project.Scripts.GameLogic.Wave;
using Project.Scripts.Module.Factory;
using UnityEngine;
using Zenject;
using LogType = Project.Scripts.Debug.LogType;

namespace Project.Scripts.Module.Spawner
{
    public class EnemySpawner: MonoBehaviour
    {
        [SerializeField] private Collider2D _leftSpawnArea;
        [SerializeField] private Collider2D _botLeftSpawnArea;
        [SerializeField] private Collider2D _rightSpawnArea;
        [SerializeField] private Collider2D _botRightSpawnArea;
        [SerializeField] private Collider2D _bottomSpawnArea;
        
        private readonly List<EnemyBase> _activeEnemies = new ();

        private EnemyFactory _enemyFactory;

        public event Action<EnemyBase> OnEnemySpawn;
        public event Action<EnemyBase> OnEnemyDeath;
        public event Action OnAllEnemiesDead;

        [Inject]
        private void Construct(EnemyFactory enemyFactory)
        {
            _enemyFactory = enemyFactory;
        }
        
        private void RemoveEnemy(EnemyBase enemy)
        {
            if (!_activeEnemies.Contains(enemy)) return;
            _activeEnemies.Remove(enemy);
            if (_activeEnemies.Count == 0)
                OnAllEnemiesDead?.Invoke();
            DebugSystem.Instance.Log(LogType.EnemySpawner,$"Enemy removed. Active enemies count: {_activeEnemies.Count}");
        }
        
        private void OnActiveEnemyDeath(EnemyBase enemy)
        {
            enemy.OnDeath -= OnActiveEnemyDeath;
            OnEnemyDeath?.Invoke(enemy);
            RemoveEnemy(enemy);
        }
        
        private Vector2 GetSpawnPosition(Collider2D spawnArea)
        {
            var bounds = spawnArea.bounds;
            var spawnPosition = new Vector2(
                UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
                UnityEngine.Random.Range(bounds.min.y, bounds.max.y));
            return spawnPosition;
        }
        
        private void SpawnEnemy(Collider2D spawnArea, EnemyData enemyData)
        {
            var enemy = _enemyFactory.Get(enemyData);
            enemy.transform.position = GetSpawnPosition(spawnArea);
            enemy.OnDeath += OnActiveEnemyDeath;
            _activeEnemies.Add(enemy);
            OnEnemySpawn?.Invoke(enemy);
            DebugSystem.Instance.Log(LogType.EnemySpawner, $"Enemy {enemyData.Name} spawned");
        }

        public void SpawnEnemy(SpawnPoint spawnPoint, EnemyData enemyData)
        {
            var areas = new List<Collider2D>();
            if ((spawnPoint & SpawnPoint.Left) == SpawnPoint.Left)
                areas.Add(_leftSpawnArea);
            if ((spawnPoint & SpawnPoint.BotLeft) == SpawnPoint.BotLeft)
                areas.Add(_botLeftSpawnArea);
            if ((spawnPoint & SpawnPoint.Right) == SpawnPoint.Right)
                areas.Add(_rightSpawnArea);
            if ((spawnPoint & SpawnPoint.BotRight) == SpawnPoint.BotRight)
                areas.Add(_botRightSpawnArea);
            if ((spawnPoint & SpawnPoint.Bottom) == SpawnPoint.Bottom)
                areas.Add(_bottomSpawnArea);

            if (areas.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(spawnPoint), "No spawn area selected");
            foreach (var area in areas)
                SpawnEnemy(area, enemyData);
        }
    }
}