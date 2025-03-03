using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.Config.Coin
{
    [CreateAssetMenu(menuName = "Config/Coin", fileName = "Coin Config")]
    public class CoinConfig: ScriptableObject
    {
        [SerializeField] private CoinValue _value;
        
        public CoinValue Value => _value;
    }
    
    [Serializable]
    public struct CoinValue
    {
        [HorizontalGroup("Split", Width = 0.5f)]
        [BoxGroup("Split/Values")]
        [SerializeField] private int _value;
        [BoxGroup("Split/Bonuses")]
        [SerializeField] private int _valuePerWave;
        
        public int Value => _value;
        public int ValuePerWave => _valuePerWave;
        
        public CoinValue(int value, int valuePerWave)
        {
            _value = value;
            _valuePerWave = valuePerWave;
        }
        
        public static CoinValue GetMultipliedValue(CoinValue coinValue, int wave)
        {
            var value = coinValue._value + coinValue._valuePerWave * wave;
            return new CoinValue
            {
                _value = value
            };
        }
        
        public static CoinValue operator +(CoinValue a, CoinValue b)
        {
            return new CoinValue(a._value + b._value, a._valuePerWave + b._valuePerWave);
        }
    }
}