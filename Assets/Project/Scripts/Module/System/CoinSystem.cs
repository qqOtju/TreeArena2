using System;

namespace Project.Scripts.Module.System
{
    public class CoinSystem
    {
        private int _currentGold;
        
        public int CurrentGold
        {
            get => _currentGold;
            set
            {
                if(value < 0) _currentGold = 0;
                else _currentGold = value;
                OnGoldChange?.Invoke(_currentGold);
            }
        }
        
        public event Action<int> OnGoldChange; 
        
        public CoinSystem(int startGold)
        {
            _currentGold = startGold;
        }
    }
}