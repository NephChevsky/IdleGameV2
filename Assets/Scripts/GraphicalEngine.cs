using Assets.Scripts.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicalEngine : MonoBehaviour
{
    public GameObject EntityPrefab;
	public TMP_Text MapLevelPlaceHolder;
	public TMP_Text PlayerLevelPlaceHolder;
	public GameObject DeathScreen;

	private GameObject Player;
    private List<GameObject> Enemies;

	private float ScreenWidth;

    void Start()
    {
        Player = InstantiateEntity();
		Player.name = "Player";
		Player.tag = "Player";
		Player.GetComponent<EntityHandler>().IsPlayer = true;
        Enemies = new();

		ScreenWidth = GetComponent<RectTransform>().rect.width * 0.95f;
	}

    void Update()
    {
		MapLevelPlaceHolder.text = Game.Map.Level.ToString();
		PlayerLevelPlaceHolder.text = Game.Map.Player.Level.ToString();

		float deathScreenAlphaRatio = 0f;
		if (Game.Map.DeathTimer >= 0f)
		{
			DeathScreen.transform.SetAsLastSibling();
			deathScreenAlphaRatio = 1f;
			if (Game.Map.DeathTimer <= 0.5f * 100f / Settings.GameEngine.TickRate)
			{
				deathScreenAlphaRatio = Game.Map.DeathTimer * 2f;
			}
		}
		Image image = DeathScreen.GetComponentInChildren<Image>();
		Color color = image.color;
		color.a = deathScreenAlphaRatio;
		image.color = color;
		TMP_Text text = DeathScreen.GetComponentInChildren<TMP_Text>();
		color = text.color;
		color.a = deathScreenAlphaRatio;
		text.color = color;

		SetEntityPosition(Player, Game.Map.Player.Position);
		SetEntityLifeRatio(Player, (float) (Game.Map.Player.CurrentHP / Game.Map.Player.MaxHP));

		for (int i = 0; i < Enemies.Count; i++)
		{
			int id = int.Parse(Enemies[i].name.Split(" ")[1]);
			Enemy enemy = Game.Map.SpawnedEnemies.Where(x => x.Id == id).FirstOrDefault();
			if (enemy == null)
			{
				Destroy(Enemies[i]);
				Enemies.RemoveAt(i);
				i--;
			}
		}

		foreach (Enemy enemy in Game.Map.SpawnedEnemies)
		{
			GameObject enemyGameObject = Enemies.Where(x => x.name == $"Enemy {enemy.Id}").FirstOrDefault();
			if (enemyGameObject == null)
			{
				enemyGameObject = InstantiateEntity();
				enemyGameObject.name = $"Enemy {enemy.Id}";
				enemyGameObject.tag = "Enemy";
				Enemies.Add(enemyGameObject);
			}

			SetEntityPosition(enemyGameObject, enemy.Position);
			SetEntityLifeRatio(enemyGameObject, (float) (enemy.CurrentHP / enemy.MaxHP));
		}
	}

	private void SetEntityPosition(GameObject entity, float position)
	{
		entity.transform.localPosition = new Vector2(-(ScreenWidth / 2f) + position * ScreenWidth, 0);
	}

	private void SetEntityLifeRatio(GameObject entity, float lifeRatio)
	{
		entity.GetComponent<EntityHandler>().LifeRatio = lifeRatio;
	}


	private GameObject InstantiateEntity()
    {
        GameObject gameObject = Instantiate(EntityPrefab);
        gameObject.transform.SetParent(this.gameObject.transform);
		gameObject.transform.localScale = Vector3.one /2f;
		return gameObject;
	}
}
