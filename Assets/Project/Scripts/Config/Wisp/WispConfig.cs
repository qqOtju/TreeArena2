using UnityEngine;

namespace Project.Scripts.Config.Wisp
{
    [CreateAssetMenu(menuName = "Config/Wisp Config", fileName = "Wisp Config")]
    public class WispConfig: ScriptableObject
    {
        [Min(1)]
        [SerializeField] private float _damage;
        [Min(0.001f)]
        [SerializeField] private float _attackSpeed;
        [Min(0)]
        [SerializeField] private float _criticalChance;
        [Min(0)]
        [SerializeField] private float _criticalDamage;
        [Min(0)]
        [SerializeField] private int _piercing;
        [Min(0)]
        [SerializeField] private float _bonusEliteDamage;
        [Min(0)]
        [SerializeField] private float _bonusBossDamage;
        
        public float Damage => _damage;
        public float AttackSpeed => _attackSpeed;
        public float CriticalChance => _criticalChance;
        public float CriticalDamage => _criticalDamage;
        public int Piercing => _piercing;
        public float BonusEliteDamage => _bonusEliteDamage;
        public float BonusBossDamage => _bonusBossDamage;
    }
}