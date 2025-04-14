using Assets.Scripts.Models;
using System.Collections.Generic;
using UnityEngine;

public static class Game
{
	public static Player Player { get; set; }
	public static List<Enemy> EnemiesToSpawn { get; set; }
    public static List<Enemy> SpawnedEnemies { get; set; }

    private static float TickTime = 1f / Settings.GameEngine.TickRate;
    private static float CurrentTime { get; set; } = 0f;
    private static float EnemySpawnTimer { get; set; } = 0f;

    public static void Init()
    {
        Player = new(10, 100);
		EnemiesToSpawn = new();
        SpawnedEnemies = new();

        for (int i = 0; i < 10; i++)
        {
			EnemiesToSpawn.Add(new Enemy(i, i == 9 ? 5 : 1, i == 9 ? 0 : 100));
        }
    }

    public static void Advance(float elapsedTime)
    {
		while (CurrentTime + elapsedTime >= TickTime)
        {
			SpawnEnemies();

			MovePlayer();

            MoveEnemies();

			CurrentTime += TickTime;
            CurrentTime %= TickTime;
            elapsedTime -= TickTime;
		}

        CurrentTime += elapsedTime;
	}

    private static void MoveEntity(Entity entity)
    {
		int direction = entity is Player ? 1 : -1;
        float newPosition = entity.Position + direction * entity.MovementSpeed / 100000f;
        if (newPosition > 1f)
        {
            newPosition = 1f;
        }
        entity.Position = newPosition;
	}

    private static void MovePlayer()
    {
        MoveEntity(Player);
    }

    private static void MoveEnemies()
    {
        foreach (Enemy enemy in SpawnedEnemies)
        {
            MoveEntity(enemy);
        }
    }

    private static void SpawnEnemies()
    {
		EnemySpawnTimer += TickTime;
		if (EnemySpawnTimer > 1f && EnemiesToSpawn.Count > 0)
		{
            SpawnedEnemies.Add(EnemiesToSpawn[0]);
            EnemiesToSpawn.RemoveAt(0);
			EnemySpawnTimer -= 1f;
		}
	}
}
