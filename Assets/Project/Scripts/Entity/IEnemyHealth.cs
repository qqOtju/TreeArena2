namespace Project.Scripts.Entity
{
    public interface IEnemyHealth: IHealth
    {
        public EnemyType Type { get; }
    }
}