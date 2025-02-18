using System;
using Project.Scripts.Config.Enemy;
using UnityEngine;

namespace Project.Scripts.GameLogic.Wave
{
    [Serializable]
    public struct WaveContent
    {
        [SerializeField] private SpawnPoint _spawnPoint;
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private int _enemyCount;
        [SerializeField] private float _spawnDelay;
        [SerializeField] private float _waveDelay;
        
        public SpawnPoint SpawnPoint => _spawnPoint;
        public EnemyData EnemyData => _enemyData;
        public int EnemyCount => _enemyCount;
        public float SpawnDelay => _spawnDelay;
        public float WaveDelay => _waveDelay;
    }
}