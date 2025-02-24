using System;
using Project.Scripts.Config.Item.WispShop;
using Project.Scripts.Debug;

namespace Project.Scripts.Module.Stats.Wisp
{
    public class WispBonuses
    {
        private float _damage;
        private float _attackSpeed;
        private float _criticalChance;
        private float _criticalDamage;
        private int _piercing;
        private float _bonusEliteDamage;
        private float _bonusBossDamage;
        
        public float Damage
        {
            get => _damage;
            set
            {
                if(value == _damage) return;
                _damage = value;
                DebugSystem.Instance.Log(LogType.WispStats, $"Bonus damage: {_damage}");
                OnDamageChanged?.Invoke(_damage);
            }
        }
        public float AttackSpeed
        {
            get => _attackSpeed;
            set
            {
                if(value == _attackSpeed) return;
                _attackSpeed = value;
                DebugSystem.Instance.Log(LogType.WispStats, $"Bonus attack speed: {_attackSpeed}");
                OnAttackSpeedChanged?.Invoke(_attackSpeed);
            }
        }
        public float CriticalChance
        {
            get => _criticalChance;
            set
            {
                if(value == _criticalChance) return;
                _criticalChance = value;
                DebugSystem.Instance.Log(LogType.WispStats, $"Bonus critical chance: {_criticalChance}");
                OnCriticalChanceChanged?.Invoke(_criticalChance);
            }
        }
        public float CriticalDamage
        {
            get => _criticalDamage;
            set
            {
                if(value == _criticalDamage) return;
                _criticalDamage = value;
                DebugSystem.Instance.Log(LogType.WispStats, $"Bonus critical damage: {_criticalDamage}");
                OnCriticalDamageChanged?.Invoke(_criticalDamage);
            }
        }
        public int Piercing
        {
            get => _piercing;
            set
            {
                if(value == _piercing) return;
                _piercing = value;
                DebugSystem.Instance.Log(LogType.WispStats, $"Bonus piercing: {_piercing}");
                OnPiercingChanged?.Invoke(_piercing);
            }
        }
        public float BonusEliteDamage
        {
            get => _bonusEliteDamage;
            set
            {
                if(value == _bonusEliteDamage) return;
                _bonusEliteDamage = value;
                DebugSystem.Instance.Log(LogType.WispStats, $"Bonus elite damage: {_bonusEliteDamage}");
                OnBonusEliteDamageChanged?.Invoke(_bonusEliteDamage);
            }
        }
        public float BonusBossDamage
        {
            get => _bonusBossDamage;
            set
            {
                if(value == _bonusBossDamage) return;
                _bonusBossDamage = value;
                DebugSystem.Instance.Log(LogType.WispStats, $"Bonus boss damage: {_bonusBossDamage}");
                OnBonusBossDamageChanged?.Invoke(_bonusBossDamage);
            }
        }
        
        public event Action<float> OnDamageChanged;
        public event Action<float> OnAttackSpeedChanged;
        public event Action<float> OnCriticalChanceChanged;
        public event Action<float> OnCriticalDamageChanged;
        public event Action<int> OnPiercingChanged;
        public event Action<float> OnBonusEliteDamageChanged;
        public event Action<float> OnBonusBossDamageChanged;
        
        public void ApplyItemBonuses(WispItem item)
        {
            Damage += item.Damage;
            AttackSpeed += item.AttackSpeed;
            CriticalChance += item.CriticalChance;
            CriticalDamage += item.CriticalDamage;
            Piercing += item.Piercing;
            BonusEliteDamage += item.BonusEliteDamage;
            BonusBossDamage += item.BonusBossDamage;
        }
    }
}