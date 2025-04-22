using Assets.Scripts.Models;
using Assets.Scripts.Models.Items;
using Assets.Scripts.Models.Maps;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Engines
{
	public static class MapEngine
	{
		public static int Level { get; set; }
		public static PlayerEntity Player { get; set; }

		public static List<EnemyEntity> EnemiesToSpawn { get; set; } = new();
		public static List<EnemyEntity> SpawnedEnemies { get; set; } = new();

		public static float DeathTimer { get; set; } = -1f;
		private static float EnemySpawnTimer { get; set; }

		public static void Init()
		{
			Player = new();
			Level = PlayerPrefs.GetInt("Map:Level", 1);
			ResetMap();
		}

		public static void Advance()
		{
			if (DeathTimer >= 0f)
			{
				DeathTimer += 1f / Settings.Game.TickRate;
				if (DeathTimer > 3f * 100f / Settings.Game.TickRate)
				{
					ResetMap();
				}
				return;
			}

			SpawnEnemies();
			Everyone_Move();
			Everyone_Attack();
		}

		private static void SpawnEnemies()
		{
			EnemySpawnTimer += 1f / Settings.Game.TickRate;
			if (EnemySpawnTimer >= (1f * 100f / Settings.Game.TickRate) && EnemiesToSpawn.Count > 0)
			{
				SpawnedEnemies.Add(EnemiesToSpawn[0]);
				EnemiesToSpawn.RemoveAt(0);
				EnemySpawnTimer = 0f;
			}
		}

		private static void Everyone_Move()
		{
			Player_Move();
			Enemies_Move();
		}

		private static void Player_Move()
		{
			Entity_Move(Player);
		}

		private static void Enemies_Move()
		{
			foreach (EnemyEntity enemy in SpawnedEnemies)
			{
				Entity_Move(enemy);
			}
		}

		private static void Entity_Move(Entity entity)
		{
			int direction = entity is PlayerEntity ? 1 : -1;
			Number movementSpeed = entity.MovementSpeed;
			Number movementSpeedBonus = 0;
			if (entity is PlayerEntity)
			{
				movementSpeedBonus = PlayerEngine.GetAffixTypeBonusFromEquipment(AffixType.MovementSpeed);
				movementSpeedBonus += PlayerEngine.GetAffixTypeBonusFromAttributes(AffixType.MovementSpeed);
			}
			float newPosition = entity.Position + direction * movementSpeed * (1 + movementSpeedBonus) / 100000f;

			if (newPosition > 1f)
			{
				newPosition = 1f;
			}

			if (newPosition < 0f)
			{
				newPosition = 0f;
			}

			if (entity is PlayerEntity && SpawnedEnemies.Count > 0 && newPosition >= SpawnedEnemies[0].Position - Settings.Game.EntityCollisionOffset)
			{
				newPosition = SpawnedEnemies[0].Position - Settings.Game.EntityCollisionOffset;
			}

			if (entity is EnemyEntity enemy)
			{
				if (newPosition <= Player.Position + Settings.Game.EntityCollisionOffset)
				{
					newPosition = Player.Position + Settings.Game.EntityCollisionOffset;
				}

				int index = SpawnedEnemies.IndexOf(enemy);
				if (index > 0 && newPosition <= SpawnedEnemies[index - 1].Position + Settings.Game.EntityCollisionOffset)
				{
					newPosition = SpawnedEnemies[index - 1].Position + Settings.Game.EntityCollisionOffset;
				}
			}

			entity.Position = newPosition;
		}

		private static void Everyone_Attack()
		{
			Player_Attack();
			Enemies_Attack();
		}

		private static void Player_Attack()
		{
			Player.AttackTimer += (1f / Settings.Game.TickRate) * Player.AttackSpeed / 100f;
			if (Player.AttackTimer >= (1f * 100f / Settings.Game.TickRate) && SpawnedEnemies.Count > 0 && Player.Position + Player.AttackRange / 2500f >= SpawnedEnemies[0].Position)
			{
				if (Entity_Attack(Player, SpawnedEnemies[0]))
				{
					PlayerEngine.GainXP(SpawnedEnemies[0].XPOnKill);

					bool drop = Random.Range(0f, 1f) >= 0.95f;
					if (drop)
					{
						Item item = Item.Generate();
						if (GameEngine.AutoSalvageWhite && item.Affixes.Count == 1 || GameEngine.AutoSalvageGreen && item.Affixes.Count == 2 || GameEngine.AutoSalvageBlue && item.Affixes.Count == 3 || GameEngine.AutoSalvagePurple && item.Affixes.Count == 4)
						{
							GameEngine.SalvageItem(item);
						}
						else if (GameEngine.Inventory.Count < 90)
						{
							GameEngine.Inventory.Add(item);
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

		private static void Enemies_Attack()
		{
			for (int i = 0; i < SpawnedEnemies.Count; i++)
			{
				SpawnedEnemies[i].AttackTimer += (1f / Settings.Game.TickRate) * Player.AttackSpeed / 100f;
				if (SpawnedEnemies[i].AttackTimer >= (1f * 100f / Settings.Game.TickRate) && SpawnedEnemies[i].Position - SpawnedEnemies[i].AttackRange / 2500f <= Player.Position)
				{
					if (Entity_Attack(SpawnedEnemies[i], Player))
					{
						ShowDeathScreen();
					}
				}
			}
		}

		private static bool Entity_Attack(Entity attacker, Entity defender)
		{
			attacker.AttackTimer = 0f;
			Number dmg = attacker.AttackDamage;
			Number dmgBonus = 0;
			if (attacker is PlayerEntity)
			{
				dmgBonus += PlayerEngine.GetAffixTypeBonusFromEquipment(AffixType.Attack);
				dmgBonus += PlayerEngine.GetAffixTypeBonusFromAttributes(AffixType.Attack);
			}
			Number hpBonus = 0;
			Number defenseBonus = 0;
			if (defender is PlayerEntity)
			{
				hpBonus = PlayerEngine.GetAffixTypeBonusFromEquipment(AffixType.HP);
				hpBonus += PlayerEngine.GetAffixTypeBonusFromAttributes(AffixType.HP);
				defenseBonus = PlayerEngine.GetAffixTypeBonusFromEquipment(AffixType.Defense);
				defenseBonus += PlayerEngine.GetAffixTypeBonusFromAttributes(AffixType.Defense);
			}
			defender.CurrentHP -= dmg * (1 + dmgBonus) / (1 + hpBonus + defenseBonus);
			if (defender.CurrentHP <= 0)
			{
				defender.CurrentHP = 0;
				return true;
			}
			return false;
		}

		private static void ShowDeathScreen()
		{
			DeathTimer = 0f;
		}

		private static void GoToNextLevel()
		{
			Level++;
			ResetMap();
		}

		private static void ResetMap()
		{
			DeathTimer = -1f;
			Player.CurrentHP = new(Player.MaxHP);
			Player.Position = 0;
			EnemiesToSpawn.Clear();
			SpawnedEnemies.Clear();
			EnemySpawnTimer = 1f * 100f / Settings.Game.TickRate;

			int maxEnemy = 9 + Level / 10;
			for (int i = 0; i < maxEnemy; i++)
			{
				EnemyEntity enemy = GenerateEnemy(Level);
				enemy.Id = i;
				EnemiesToSpawn.Add(enemy);
			}

			EnemyEntity boss = GenerateBoss(Level);
			boss.Id = EnemiesToSpawn.Count;
			EnemiesToSpawn.Add(boss);
		}

		private static EnemyEntity GenerateEnemy(int level)
		{
			Number hp = 1 * Mathf.Pow(1.03f, level - 1);
			Number attackDamage = 1 * Mathf.Pow(1.02f, level - 1);
			Number xpOnKill = 1 * (1 + (level - 1) * 0.1f);
			int movementSpeed = 100;
			EnemyEntity enemy = new(hp, attackDamage, xpOnKill, movementSpeed);
			return enemy;
		}

		private static EnemyEntity GenerateBoss(int level)
		{
			EnemyEntity boss = new(5 * Mathf.Pow(1.03f, level - 1), 5 * Mathf.Pow(1.02f, level - 1), 5 * (1 + (level - 1) * 0.1), 0);
			return boss;
		}

		public static void Save()
		{
			PlayerPrefs.SetInt("Map:Level", Level);
			PlayerPrefs.Save();
		}
	}
}
