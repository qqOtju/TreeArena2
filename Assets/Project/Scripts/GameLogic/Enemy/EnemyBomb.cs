using Project.Scripts.Entity;

namespace Project.Scripts.GameLogic.Enemy
{
    //ToDo: Maybe make that if it explodes it dont drop any resources
    public class EnemyBomb: EnemyBase
    {
        protected override void Attack(IHealth health)
        {
            base.Attack(health);
            TakeDamage(EnemyValue.MaxHealth);
        }
    }
}