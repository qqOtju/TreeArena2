using System.Collections.Generic;
using Project.Scripts.Config.Enemy;
using Project.Scripts.DesignPattern.Pool;
using Project.Scripts.GameLogic.Enemy;
using Project.Scripts.Module.Stats.Enemy;
using UnityEngine;
using Zenject;
using Tree = Project.Scripts.GameLogic.Character.Tree;

namespace Project.Scripts.Module.Factory
{
    public class EnemyFactory
    {
        private readonly Dictionary<EnemyData, MonoBehaviourPool<EnemyBase>> _pools = new();
        //ToDo: Rename
        private readonly Dictionary<EnemyBase, MonoBehaviourPool<EnemyBase>> _poolies = new();
        private readonly Transform _container;
        private readonly DiContainer _diContainer;
        private readonly EnemyBonuses _enemyBonuses;
        private readonly Tree _tree;
        
        private int _currentWaveIndex;

        public EnemyFactory(Transform container, EnemyBonuses enemyBonuses, 
            DiContainer diContainer, Tree tree)
        {
            _container = container;
            _enemyBonuses = enemyBonuses;
            _diContainer = diContainer;
            _tree = tree;
        }

        private void CreatePool(EnemyData enemyData)
        {
            var newContainer = new GameObject("Enemy Pool").transform;
            newContainer.SetParent(_container);
            var newPool = new MonoBehaviourPool<EnemyBase>(enemyData.EnemyPrefab, newContainer);
            _pools.Add(enemyData, newPool);
        }

        private void OnEnemyDeath(EnemyBase obj)
        {
            obj.OnDeath -= OnEnemyDeath;
            Release(obj);
        }

        private void Release(EnemyBase obj)
        {
            _poolies[obj].Release(obj);
        }

        public EnemyBase Get(EnemyData enemyData)
        {
            _pools.TryGetValue(enemyData, out var pool);
            if (pool == null)
            {
                CreatePool(enemyData);
                return Get(enemyData);
            }
            var enemy = pool.Get();
            _poolies.TryAdd(enemy, pool);
            _diContainer.Inject(enemy);
            enemy.OnDeath += OnEnemyDeath;
            var stats = enemyData.EnemyConfig.Value;
            stats = EnemyValue.GetMultipliedValue(stats ,_currentWaveIndex);
            stats = EnemyValue.GetUpgraded(stats, _enemyBonuses);
            enemy.Initialize(stats, enemyData.EnemyType, _tree);
            return enemy;
        }
        
        public void SetWaveIndex(int index)
        {
            _currentWaveIndex = index;
        }
    }
}