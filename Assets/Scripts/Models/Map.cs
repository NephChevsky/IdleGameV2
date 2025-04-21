using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Models
{
	public class Map
	{
		public int Level { get; set; }
		public Player Player { get; set; }
		public List<Enemy> EnemiesToSpawn { get; set; } = new();
		public List<Enemy> SpawnedEnemies { get; set; } = new();
		public float DeathTimer { get; set; } = -1f;
		private float EnemySpawnTimer { get; set; }

		public Map(int mapLevel, int playerLevel)
		{
			Player = new(playerLevel);
			Level = mapLevel;
			ResetMap();
		}

		public void Advance()
		{
			if (DeathTimer >= 0f)
			{
				DeathTimer += 1f / Settings.GameEngine.TickRate;
				if (DeathTimer > 3f * 100f / Settings.GameEngine.TickRate)
				{
					ResetMap();
				}
				return;
			}

			SpawnEnemies();
			Everyone_Move();
			Everyone_Attack();
		}

		private void SpawnEnemies()
		{
			EnemySpawnTimer += 1f / Settings.GameEngine.TickRate;
			if (EnemySpawnTimer >= (1f * 100f / Settings.GameEngine.TickRate) && EnemiesToSpawn.Count > 0)
			{
				SpawnedEnemies.Add(EnemiesToSpawn[0]);
				EnemiesToSpawn.RemoveAt(0);
				EnemySpawnTimer = 0f;
			}
		}

		private void Everyone_Move()
		{
			Player_Move();
			Enemies_Move();
		}

		private void Player_Move()
		{
			Entity_Move(Player);
		}

		private void Enemies_Move()
		{
			foreach (Enemy enemy in SpawnedEnemies)
			{
				Entity_Move(enemy);
			}
		}

		private void Entity_Move(Entity entity)
		{
			int direction = entity is Player ? 1 : -1;
			float newPosition = entity.Position + direction * entity.MovementSpeed / 100000f;

			if (newPosition > 1f)
			{
				newPosition = 1f;
			}

			if (newPosition < 0f)
			{
				newPosition = 0f;
			}

			if (entity is Player && SpawnedEnemies.Count > 0 && newPosition >= SpawnedEnemies[0].Position - Settings.GameEngine.EntityCollisionOffset)
			{
				newPosition = SpawnedEnemies[0].Position - Settings.GameEngine.EntityCollisionOffset;
			}

			if (entity is Enemy enemy)
			{
				if (newPosition <= Player.Position + Settings.GameEngine.EntityCollisionOffset)
				{
					newPosition = Player.Position + Settings.GameEngine.EntityCollisionOffset;
				}

				int index = SpawnedEnemies.IndexOf(enemy);
				if (index > 0 && newPosition <= SpawnedEnemies[index - 1].Position + Settings.GameEngine.EntityCollisionOffset)
				{
					newPosition = SpawnedEnemies[index - 1].Position + Settings.GameEngine.EntityCollisionOffset;
				}
			}

			entity.Position = newPosition;
		}

		private void Everyone_Attack()
		{
			Player_Attack();
			Enemies_Attack();
		}

		private void Player_Attack()
		{
			Player.AttackTimer += (1f / Settings.GameEngine.TickRate) * Player.AttackSpeed / 100f;
			if (Player.AttackTimer >= (1f * 100f / Settings.GameEngine.TickRate) && SpawnedEnemies.Count > 0 && Player.Position + Player.AttackRange / 2500f >= SpawnedEnemies[0].Position)
			{
				if (Entity_Attack(Player, SpawnedEnemies[0]))
				{
					Player.CurrentXP += SpawnedEnemies[0].XPOnKill;
					if (Player.CurrentXP >= Player.MaxXP)
					{
						Player.LevelUp();
					}

					bool drop = Random.Range(0f, 1f) >= 0.95f;
					if (drop)
					{
						Item item = Item.Generate();
						if (Game.AutoSalvageWhite && item.Affixes.Count == 1 || Game.AutoSalvageGreen && item.Affixes.Count == 2 || Game.AutoSalvageBlue && item.Affixes.Count == 3 || Game.AutoSalvagePurple && item.Affixes.Count == 4)
						{
							Game.SalvageItem(item);
						}
						else if (Game.Inventory.Count < 90)
						{
							Game.Inventory.Add(item);
						}
					}

					SpawnedEnemies.Remove(SpawnedEnemies[0]);
					if (SpawnedEnemies.Count == 0)
					{
						GoToNextLevel();
					}
				}
			}
		}

		private void Enemies_Attack()
		{
			for (int i = 0; i < SpawnedEnemies.Count; i++)
			{
				SpawnedEnemies[i].AttackTimer += (1f / Settings.GameEngine.TickRate) * Player.AttackSpeed / 100f;
				if (SpawnedEnemies[i].AttackTimer >= (1f * 100f / Settings.GameEngine.TickRate) && SpawnedEnemies[i].Position - SpawnedEnemies[i].AttackRange / 2500f <= Player.Position)
				{
					if (Entity_Attack(SpawnedEnemies[i], Player))
					{
						ShowDeathScreen();
					}
				}
			}
		}

		private bool Entity_Attack(Entity attacker, Entity defender)
		{
			attacker.AttackTimer = 0f;
			Number dmg = attacker.AttackDamage;
			if (attacker is Player player)
			{
				dmg *= GetAffixTypeBonusFromEquipment(AffixType.Attack);
			}
			defender.CurrentHP -= dmg;
			if (defender.CurrentHP <= 0)
			{
				return true;
			}
			return false;
		}

		private Number GetAffixTypeBonusFromEquipment(AffixType affixType)
		{
			Number value = 1f;
			foreach (Item item in Game.Equipment)
			{
				foreach (Affix affix in item.Affixes)
				{
					if (affix.Type == affixType)
					{
						value *= 1 + affix.Value;
					}
				}
			}
			return value;
		}

		private void ShowDeathScreen()
		{
			DeathTimer = 0f;
		}

		private void ResetMap()
		{
			DeathTimer = -1f;
			Player.Reset();
			EnemiesToSpawn.Clear();
			SpawnedEnemies.Clear();
			EnemySpawnTimer = 1f * 100f / Settings.GameEngine.TickRate;

			int maxEnemy = 9 + Level / 10;
			for (int i = 0; i < maxEnemy; i++)
			{
				Enemy enemy = Enemy.GenerateEnemy(Level);
				enemy.Id = i;
				EnemiesToSpawn.Add(enemy);
			}

			Enemy boss = Enemy.GenerateBoss(Level);
			boss.Id = EnemiesToSpawn.Count;
			EnemiesToSpawn.Add(boss);
		}

		private void GoToNextLevel()
		{
			Level++;
			ResetMap();
		}
	}
}
