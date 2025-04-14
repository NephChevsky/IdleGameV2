using Assets.Scripts.Models;
using System.Collections.Generic;
using UnityEngine;

public static class Game
{
    private static float CurrentTime { get; set; }
    public static Player Player { get; set; }
    private static List<Enemy> Enemies { get; set; }

    public static void Init()
    {
        Player = new(10);
        Enemies = new();

        for (int i = 0; i < 10; i++)
        {
            Enemies.Add(new Enemy(i == 9 ? 5 : 1));
        }
    }

    public static void Advance(float elapsedTime)
    {
        while (CurrentTime + elapsedTime >= 0.01f)
        {
            MoveEntity(Player);

            CurrentTime += 0.01f;
            CurrentTime %= 0.01f;
            elapsedTime -= 0.01f;
		}

        CurrentTime += elapsedTime;
	}

    private static void MoveEntity(Entity entity)
    {
		int direction = entity is Player ? 1 : -1;
		entity.Position += direction * entity.MovementSpeed / 1000f;
	}
}
