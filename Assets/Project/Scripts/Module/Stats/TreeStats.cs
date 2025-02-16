using Project.Scripts.Config.Tree;

namespace Project.Scripts.Module.Stats
{
    public class TreeStats
    {
        private readonly TreeConfig _config;
        private readonly TreeBonuses _bonuses;
        
        public int MaxHealth { get; private set; }
        public float Regen { get; private set; }
        public int Armor { get; private set; }
        public float Absorption { get; private set; }
        
        public TreeStats(TreeConfig config, TreeBonuses bonuses)
        {
            _config = config;
            _bonuses = bonuses;
            MaxHealth = CalculateMaxHealth();
            Regen = CalculateRegen();
            Armor = CalculateArmor();
            Absorption = CalculateAbsorption();
            _bonuses.OnMaxHealthChanged += _ => MaxHealth = CalculateMaxHealth();
            _bonuses.OnRegenChanged += _ => Regen = CalculateRegen();
            _bonuses.OnArmorChanged += _ => Armor = CalculateArmor();
            _bonuses.OnResistChanged += _ => Absorption = CalculateAbsorption();
        }

        private int CalculateMaxHealth()
        {
            return _config.MaxHealth + _bonuses.MaxHealth;
        }

        private float CalculateRegen()
        {
            return _config.Regen + _bonuses.Regen;
        }

        private int CalculateArmor()
        {
            return _config.Armor + _bonuses.Armor;
        }

        private float CalculateAbsorption()
        {
            return _config.Absorption + _bonuses.Absorption;
        }
    }
}