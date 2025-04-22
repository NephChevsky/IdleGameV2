namespace Assets.Scripts.Models.Maps
{
	public class Entity
	{
		public float Position { get; set; }
		public Number CurrentHP { get; set; }
		public Number MaxHP { get; set; }
		public int AttackSpeed { get; set; }
		public float AttackTimer { get; set; }
		public int AttackRange { get; set; }
		public Number AttackDamage { get; set; }
		public int MovementSpeed { get; set; }

		public Entity(Number baseHP, Number attackDamage, int movementSpeed)
		{
			Position = this is PlayerEntity ? 0 : 1;
			CurrentHP = new(baseHP);
			MaxHP = new(baseHP);
			AttackSpeed = 100;
			AttackTimer = 0f;
			AttackRange = 100;
			AttackDamage = attackDamage;
			MovementSpeed = movementSpeed;
		}
	}
}
