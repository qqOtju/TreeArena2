using Project.Scripts.Config.Wisp;

namespace Project.Scripts.Module.Stats
{        
    //ToDo: Change formulas to calculate stats
    public class WispStats
    {
        private readonly WispConfig _config;
        private readonly WispBonuses _bonuses;

        public float Damage {get; private set;}
        public float AttackSpeed {get; private set;}
        public float CriticalChance {get; private set;}
        public float CriticalDamage {get; private set;}
        public int Piercing {get; private set;}
        public float BonusEliteDamage {get; private set;}
        public float BonusBossDamage {get; private set;}
        
        public float DamageWithElite { get; private set; }
        public float DamageWithBoss { get; private set; }
        public float DamageWithCrit { get; private set; }
        public float DamageWithCritAndElite { get; private set; }
        public float DamageWithCritAndBoss { get; private set; }

        public WispStats(WispConfig config, WispBonuses bonuses)
        {
            _config = config;
            _bonuses = bonuses;
            Damage = CalculateDamage();
            AttackSpeed = CalculateAttackSpeed();
            CriticalChance = CalculateCriticalChance();
            CriticalDamage = CalculateCriticalDamage();
            Piercing = CalculatePiercing();
            BonusEliteDamage = CalculateBonusEliteDamage();
            BonusBossDamage = CalculateBonusBossDamage();
            CalculateDamageWithElite();
            CalculateDamageWithBoss();
            CalculateDamageWithCritical();
            CalculateDamageWithCritAndElite();
            CalculateDamageWithCritAndBoss();
            _bonuses.OnDamageChanged += _ => CalculateAllDamage();
            _bonuses.OnAttackSpeedChanged += _ => AttackSpeed = CalculateAttackSpeed();
            _bonuses.OnCriticalChanceChanged += _ => CriticalChance = CalculateCriticalChance();
            _bonuses.OnCriticalDamageChanged += _ => CalculateAllCriticalDamage();
            _bonuses.OnPiercingChanged += _ => Piercing = CalculatePiercing();
            _bonuses.OnBonusEliteDamageChanged += _ => CalculateAllEliteDamage();
            _bonuses.OnBonusBossDamageChanged += _ => CalculateAllBossDamage();
        }

        private void CalculateAllDamage()
        {
            Damage = CalculateDamage();
            CalculateDamageWithElite();
            CalculateDamageWithBoss();
            CalculateDamageWithCritical();
            CalculateDamageWithCritAndElite();
            CalculateDamageWithCritAndBoss();
        }
        
        private void CalculateAllCriticalDamage()
        {
            CriticalDamage = CalculateCriticalDamage();
            CalculateDamageWithCritical();
            CalculateDamageWithCritAndElite();
            CalculateDamageWithCritAndBoss();
        }
        
        private void CalculateAllEliteDamage()
        {
            BonusEliteDamage = CalculateBonusEliteDamage();
            CalculateDamageWithElite();
            CalculateDamageWithCritAndElite();
        }
        
        private void CalculateAllBossDamage()
        {
            BonusBossDamage = CalculateBonusBossDamage();
            CalculateDamageWithBoss();
            CalculateDamageWithCritAndBoss();
        }

        private float CalculateDamage()
        { 
            return _config.Damage * (1 + _bonuses.Damage/100);// 100 * (1 + 10/100) = 100 * 1.1
        }
        
        private float CalculateAttackSpeed()
        {
            return _config.AttackSpeed / (1 + _bonuses.AttackSpeed/100);
        }
        
        private float CalculateCriticalChance()
        {
            return _config.CriticalChance + _bonuses.CriticalChance;
        }
        
        private float CalculateCriticalDamage()
        {
            return _config.CriticalDamage + _bonuses.CriticalDamage;
        }
        
        private int CalculatePiercing()
        {
            return _config.Piercing + _bonuses.Piercing;
        }
        
        private float CalculateBonusEliteDamage()
        {
            return _config.BonusEliteDamage + _bonuses.BonusEliteDamage;
        }
        
        private float CalculateBonusBossDamage()
        {
            return _config.BonusBossDamage + _bonuses.BonusBossDamage;
        }

        private void CalculateDamageWithElite()
        {
            DamageWithElite = Damage * (1 + BonusEliteDamage / 100);
        }

        private void CalculateDamageWithBoss()
        {
            DamageWithBoss = Damage * (1 + BonusBossDamage / 100);
        }

        private void CalculateDamageWithCritical()
        {
            DamageWithCrit = Damage * (1 + CriticalDamage / 100);
        }

        private void CalculateDamageWithCritAndElite()
        {
            DamageWithCritAndElite = DamageWithCrit * (1 + BonusEliteDamage / 100);
        }

        private void CalculateDamageWithCritAndBoss()
        {
            DamageWithCritAndBoss = DamageWithCrit * (1 + BonusBossDamage / 100);
        }

    }
}