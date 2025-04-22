using Assets.Scripts.Engines;
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
		MapLevelPlaceHolder.text = MapEngine.Level.ToString();
		PlayerLevelPlaceHolder.text = MapEngine.Player.Level.ToString();

		float deathScreenAlphaRatio = 0f;
		if (MapEngine.DeathTimer >= 0f)
		{
			DeathScreen.transform.SetAsLastSibling();
			deathScreenAlphaRatio = 1f;
			if (MapEngine.DeathTimer <= 0.5f * 100f / Settings.Game.TickRate)
			{
				deathScreenAlphaRatio = MapEngine.DeathTimer * 2f;
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

		SetEntityPosition(Player, MapEngine.Player.Position);
		SetEntityLifeRatio(Player, (float) (MapEngine.Player.CurrentHP / MapEngine.Player.MaxHP));

		for (int i = 0; i < Enemies.Count; i++)
		{
			int id = int.Parse(Enemies[i].name.Split(" ")[1]);
			Enemy enemy = MapEngine.SpawnedEnemies.Where(x => x.Id == id).FirstOrDefault();
			if (enemy == null)
			{
				Destroy(Enemies[i]);
				Enemies.RemoveAt(i);
				i--;
			}
		}

		foreach (Enemy enemy in MapEngine.SpawnedEnemies)
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
