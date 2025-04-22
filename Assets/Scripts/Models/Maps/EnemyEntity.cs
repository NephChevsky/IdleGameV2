namespace Assets.Scripts.Models.Maps
{
	public class EnemyEntity : Entity
	{
		public int Id { get; set; }
		public Number XPOnKill { get; set; }

		public EnemyEntity(Number hp, Number attackDamage, Number xpOnKill, int movementSpeed) : base(hp, attackDamage, movementSpeed)
		{
			XPOnKill = xpOnKill;
		}
	}
}
