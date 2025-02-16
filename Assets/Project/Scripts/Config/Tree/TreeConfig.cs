using UnityEngine;

namespace Project.Scripts.Config.Tree
{
    [CreateAssetMenu(menuName = "Config/Tree Config", fileName = "Tree Config")]
    public class TreeConfig: ScriptableObject
    {
        [Min(1)]
        [SerializeField] private int _maxHealth;
        [Min(0)]
        [SerializeField] private float _regen;
        [Min(0)]
        [SerializeField] private int _armor;//тепер це як хп але без регену
        [Min(0)] 
        [SerializeField] private float _absorption;
        
        public int MaxHealth => _maxHealth;
        public float Regen => _regen;       
        public int Armor => _armor;
        public float Absorption => _absorption;
    }
}