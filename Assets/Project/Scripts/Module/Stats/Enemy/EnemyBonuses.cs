using Project.Scripts.Config.Item.WispShop;

namespace Project.Scripts.Module.Stats.Enemy
{
    public class EnemyBonuses
    {
        public float MaxHealth { get; set; }
        public float MoveSpeed { get; set; }
        public float Damage { get; set; }
        public float AttackSpeed { get; set; }
        public float AttackRange { get; set; }
        
        public void ApplyItemBonuses(WispItem item)
        {
            MaxHealth += item.EnemyMaxHealth;
            MoveSpeed += item.EnemyMoveSpeed;
            Damage += item.EnemyDamage;
            AttackSpeed += item.EnemyAttackSpeed;
            AttackRange += item.EnemyAttackRange;
        }
    }
}