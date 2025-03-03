using Project.Scripts.Config.Coin;
using Project.Scripts.DesignPattern.Factory;
using Project.Scripts.GameLogic.Dropdown;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Module.Factory
{
    public class CoinFactory: ObjectFactory<Coin>
    {
        private const int BasePoolSize = 10;
        
        private readonly DiContainer _diContainer;
        private readonly CoinConfig _coinConfig;
        private readonly Transform _target;
        
        private int _currentWaveIndex;

        public CoinFactory(Coin prefab, Transform container, DiContainer diContainer, CoinConfig coinConfig, Transform target) : base(prefab, container)
        {
            Pool.Initialize(BasePoolSize);
            _diContainer = diContainer;
            _coinConfig = coinConfig;
            _target = target;
        }

        private void OnPlayerHit(Coin coin)
        {
            coin.OnPlayerHit -= OnPlayerHit;
            Release(coin);
        }

        public override void Release(Coin obj)
        {
            Pool.Release(obj);
        }

        public override Coin Get()
        {
            var coin = Pool.Get();
            _diContainer.Inject(coin);
            coin.OnPlayerHit += OnPlayerHit;
            var stats = _coinConfig.Value;
            stats = CoinValue.GetMultipliedValue(stats, _currentWaveIndex);
            coin.Initialize(stats, _target);
            return coin;
        }
        
        public void SetWaveIndex(int index)
        {
            _currentWaveIndex = index;
        }
    }
}