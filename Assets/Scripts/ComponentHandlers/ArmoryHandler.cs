using Assets.Scripts.Engines;
using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmoryHandler : MonoBehaviour
{
    public GameObject ItemPrefab;
	public GameObject AttributeSetterPrefab;

	public GameObject SalvageButton;
	public GameObject AutoSalvageWhiteToggle;
	public GameObject AutoSalvageGreenToggle;
	public GameObject AutoSalvageBlueToggle;
	public GameObject AutoSalvagePurpleToggle;

	public GameObject AttributePointsValue;

	private readonly List<GameObject> Inventory = new();

	private GameObject Helm;
	private GameObject Amulet;
	private GameObject Gloves;
	private GameObject Chest;
	private GameObject RingL;
	private GameObject Belt;
	private GameObject RingR;
	private GameObject Pants;
	private GameObject Boots;
	private GameObject MainHand;
	private GameObject OffHand;

	private readonly List<GameObject> AttributeSetters = new();

	void Start()
    {
        Rect rect = GetComponent<RectTransform>().rect;
		float width = rect.width * 0.95f;
        float height = rect.height * 0.95f;
        float xOffset = 10;
        float yOffset = 32;
        for (int i = 0; i < 90; i++)
        {
            GameObject item = Instantiate(ItemPrefab);
            item.transform.SetParent(transform);
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = new Vector2(-width / 2 + xOffset, height / 2 - yOffset);
            item.name = "Item " + i;
            Inventory.Add(item);
            xOffset += 104;
            if (xOffset > 104 * 10)
            {
                yOffset += 104;
                xOffset = 10;
            }
        }

		Helm = Instantiate(ItemPrefab);
		Amulet = Instantiate(ItemPrefab);
		Gloves = Instantiate(ItemPrefab);
		Chest = Instantiate(ItemPrefab);
		RingL = Instantiate(ItemPrefab);
		Belt = Instantiate(ItemPrefab);
		RingR = Instantiate(ItemPrefab);
		Pants = Instantiate(ItemPrefab);
		Boots = Instantiate(ItemPrefab);
		MainHand = Instantiate(ItemPrefab);
		OffHand = Instantiate(ItemPrefab);

		Helm.name = "Helm";
		Amulet.name = "Amulet";
		Gloves.name = "Gloves";
		Chest.name = "Chest";
		RingL.name = "RingL";
		Belt.name = "Belt";
		RingR.name = "RingR";
		Pants.name = "Pants";
		Boots.name = "Boots";
		MainHand.name = "MainHand";
		OffHand.name = "OffHand";

		Helm.transform.SetParent(transform);
		Amulet.transform.SetParent(transform);
		Gloves.transform.SetParent(transform);
		Chest.transform.SetParent(transform);
		RingL.transform.SetParent(transform);
		Belt.transform.SetParent(transform);
		RingR.transform.SetParent(transform);
		Pants.transform.SetParent(transform);
		Boots.transform.SetParent(transform);
		MainHand.transform.SetParent(transform);
		OffHand.transform.SetParent(transform);

		Helm.transform.localScale = Vector3.one;
		Amulet.transform.localScale = Vector3.one;
		Gloves.transform.localScale = Vector3.one;
		Chest.transform.localScale = Vector3.one;
		RingL.transform.localScale = Vector3.one;
		Belt.transform.localScale = Vector3.one;
		RingR.transform.localScale = Vector3.one;
		Pants.transform.localScale = Vector3.one;
		Boots.transform.localScale = Vector3.one;
		MainHand.transform.localScale = Vector3.one;
		OffHand.transform.localScale = Vector3.one;

		xOffset = width * 3f / 8f;
		yOffset = height / 2 - 32;
		Helm.transform.localPosition = new Vector2(xOffset, yOffset);
		Amulet.transform.localPosition = new Vector2(xOffset + 104, yOffset);
		Gloves.transform.localPosition = new Vector2(xOffset - 104, yOffset - 104);
		Chest.transform.localPosition = new Vector2(xOffset, yOffset - 104);
		RingL.transform.localPosition = new Vector2(xOffset - 104, yOffset - 104 * 2);
		Belt.transform.localPosition = new Vector2(xOffset, yOffset - 104 * 2);
		RingR.transform.localPosition = new Vector2(xOffset + 104, yOffset - 104 * 2);
		Pants.transform.localPosition = new Vector2(xOffset, yOffset - 104 * 3);
		Boots.transform.localPosition = new Vector2(xOffset, yOffset - 104 * 4);
		MainHand.transform.localPosition = new Vector2(xOffset - 104, yOffset - 104 * 3);
		OffHand.transform.localPosition = new Vector2(xOffset + 104, yOffset - 104 * 3);

		SalvageButton.GetComponent<Button>().onClick.AddListener(GameEngine.ToggleSalvageMode);
		AutoSalvageWhiteToggle.GetComponent<Toggle>().isOn = GameEngine.AutoSalvageWhite;
		AutoSalvageGreenToggle.GetComponent<Toggle>().isOn = GameEngine.AutoSalvageGreen;
		AutoSalvageBlueToggle.GetComponent<Toggle>().isOn = GameEngine.AutoSalvageBlue;
		AutoSalvagePurpleToggle.GetComponent<Toggle>().isOn = GameEngine.AutoSalvagePurple;
		AutoSalvageWhiteToggle.GetComponent<Toggle>().onValueChanged.AddListener((value) => GameEngine.ToggleAutoSalvageWhite());
		AutoSalvageGreenToggle.GetComponent<Toggle>().onValueChanged.AddListener((value) => GameEngine.ToggleAutoSalvageGreen());
		AutoSalvageBlueToggle.GetComponent<Toggle>().onValueChanged.AddListener((value) => GameEngine.ToggleAutoSalvageBlue());
		AutoSalvagePurpleToggle.GetComponent<Toggle>().onValueChanged.AddListener((value) => GameEngine.ToggleAutoSalvagePurple());

		xOffset = width * 3f / 8f;
		yOffset = -height / 2 + 200;
		foreach (AffixType affix in Enum.GetValues(typeof(AffixType)))
		{
			GameObject go = Instantiate(AttributeSetterPrefab);
			go.transform.SetParent(transform);
			go.transform.localScale = Vector3.one;
			go.transform.localPosition = new Vector2(xOffset, yOffset);
			AttributeSetterHandler handler = go.GetComponent<AttributeSetterHandler>();
			handler.SetName(affix);
			handler.SetValue(PlayerEngine.AffectedAttributePoints[affix]);
			AttributeSetters.Add(go);
			yOffset -= 35;
		}
	}

    void Update()
    {
        int count = 0;
		foreach (GameObject item in Inventory)
		{
			if (GameEngine.Inventory.Count > count)
			{
				ItemHandler itemScript = item.GetComponent<ItemHandler>();
				itemScript.Item = GameEngine.Inventory[count];
				itemScript.AllowSalvageMode = true;
			}
			else
			{
				item.GetComponent<ItemHandler>().Item = null;
			}
            count++;
		}

		Helm.GetComponent<ItemHandler>().Item = PlayerEngine.Equipment.FirstOrDefault(x => x.Type == ItemType.Helm);
		Amulet.GetComponent<ItemHandler>().Item = PlayerEngine.Equipment.FirstOrDefault(x => x.Type == ItemType.Amulet);
		Gloves.GetComponent<ItemHandler>().Item = PlayerEngine.Equipment.FirstOrDefault(x => x.Type == ItemType.Gloves);
		Chest.GetComponent<ItemHandler>().Item = PlayerEngine.Equipment.FirstOrDefault(x => x.Type == ItemType.Chest);
		RingL.GetComponent<ItemHandler>().Item = PlayerEngine.Equipment.Where(x => x.Type == ItemType.Ring).FirstOrDefault();
		Belt.GetComponent<ItemHandler>().Item = PlayerEngine.Equipment.FirstOrDefault(x => x.Type == ItemType.Belt);
		RingR.GetComponent<ItemHandler>().Item = PlayerEngine.Equipment.Where(x => x.Type == ItemType.Ring).Skip(1).FirstOrDefault();
		Pants.GetComponent<ItemHandler>().Item = PlayerEngine.Equipment.FirstOrDefault(x => x.Type == ItemType.Pants);
		Boots.GetComponent<ItemHandler>().Item = PlayerEngine.Equipment.FirstOrDefault(x => x.Type == ItemType.Boots);
		MainHand.GetComponent<ItemHandler>().Item = PlayerEngine.Equipment.FirstOrDefault(x => x.Type == ItemType.MainHand);
		OffHand.GetComponent<ItemHandler>().Item = PlayerEngine.Equipment.FirstOrDefault(x => x.Type == ItemType.OffHand);

		AttributePointsValue.GetComponent<TMP_Text>().text = (PlayerEngine.Level - PlayerEngine.AffectedAttributePoints.Sum(x => x.Value) - 1).ToString();
	}
}
