using Assets.Scripts.Models;
using System.Collections.Generic;
using UnityEngine;

public static class Game
{
	public static Player Player { get; set; }
	public static List<Enemy> EnemiesToSpawn { get; set; } = new();
	public static List<Enemy> SpawnedEnemies { get; set; } = new();

    private static readonly float TickTime = 1f / Settings.GameEngine.TickRate;
    private static float CurrentTime { get; set; }
    private static float EnemySpawnTimer { get; set; }

    public static void Init()
    {
		Player = new(20, 100);
		ResetMap();
    }

    public static void Advance(float elapsedTime)
    {
		while (CurrentTime + elapsedTime >= TickTime)
        {
			SpawnEnemies();

			Everyone_Move();

			Everyone_Attack();

			CurrentTime += TickTime;
            CurrentTime %= TickTime;
            elapsedTime -= TickTime;
		}

        CurrentTime += elapsedTime;
	}

	private static void SpawnEnemies()
	{
		EnemySpawnTimer += TickTime;
		if (EnemySpawnTimer >= 1f && EnemiesToSpawn.Count > 0)
		{
			SpawnedEnemies.Add(EnemiesToSpawn[0]);
			EnemiesToSpawn.RemoveAt(0);
			EnemySpawnTimer %= 1f;
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
		foreach (Enemy enemy in SpawnedEnemies)
		{
			Entity_Move(enemy);
		}
	}

	private static void Entity_Move(Entity entity)
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
			if (index > 0 && newPosition <= SpawnedEnemies[index-1].Position + Settings.GameEngine.EntityCollisionOffset)
			{
				newPosition = SpawnedEnemies[index - 1].Position + Settings.GameEngine.EntityCollisionOffset;
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
		Player.AttackTimer += TickTime * Player.AttackSpeed / 100f;
		if (Player.AttackTimer >= 1f && SpawnedEnemies.Count > 0 && Player.Position + Player.AttackRange / 2500f >= SpawnedEnemies[0].Position)
		{
			if (Entity_Attack(Player, SpawnedEnemies[0]))
			{
				SpawnedEnemies.Remove(SpawnedEnemies[0]);
				if (SpawnedEnemies.Count == 0)
				{
					ResetMap();
				}
			}
		}
	}

	private static void Enemies_Attack()
	{
		for(int i = 0; i < SpawnedEnemies.Count; i++)
		{
			SpawnedEnemies[i].AttackTimer += TickTime * Player.AttackSpeed / 100f;
			if (SpawnedEnemies[i].AttackTimer >= 1f && SpawnedEnemies[i].Position - SpawnedEnemies[i].AttackRange / 2500f <= Player.Position)
			{
				if (Entity_Attack(SpawnedEnemies[i], Player))
				{
					ResetMap();
				}
			}
		}
	}

	private static bool Entity_Attack(Entity attacker, Entity defender)
	{
		attacker.AttackTimer %= TickTime;
		defender.CurrentHP -= attacker.AttackDamage;
		if (defender.CurrentHP <= 0)
		{
			return true;
		}
		return false;
	}

	private static void ResetMap()
	{
		Player.CurrentHP = Player.MaxHP;
		Player.Position = 0;
		EnemiesToSpawn.Clear();
		SpawnedEnemies.Clear();
		EnemySpawnTimer = 1f;

		for (int i = 0; i < 10; i++)
		{
			EnemiesToSpawn.Add(new Enemy(i, i == 9 ? 5 : 1, i == 9 ? 0 : 100));
		}
	}
}
