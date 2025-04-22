using Assets.Scripts.Engines;
using Assets.Scripts.Models.Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Item Item;
	public Sprite Helm;
	public Sprite Chest;
	public Sprite Gloves;
	public Sprite Belt;
	public Sprite Pants;
	public Sprite Boots;
	public Sprite Amulet;
	public Sprite Ring;
	public Sprite OffHand;
	public Sprite MainHand;
	public Image Image;

	public MenuHandler MenuHandler;

	public bool AllowSalvageMode { get; set; } = false;
	public bool ShowToolTip { get; set; } = false;

	private float DoubleClickTimer = -1f;

    void Update()
    {
		if (Item != null)
		{
			Image panel = GetComponentInChildren<Image>();

			if (GraphicalEngine.SalvageMode && AllowSalvageMode)
			{
				panel.color = new Color(1f, 0f, 0f);
			}
			else if (Item.Affixes.Count == 1)
			{
				panel.color = new Color(1f, 1f, 1f);
			}
			else if (Item.Affixes.Count == 2)
			{
				panel.color = new Color(0f, 1f, 0f);
			}
			else if (Item.Affixes.Count == 3)
			{
				panel.color = new Color(0f, 0f, 1f);
			}
			else if (Item.Affixes.Count == 4)
			{
				panel.color = new Color(1f, 0f, 1f);
			}
			else if (Item.Affixes.Count == 5)
			{
				panel.color = new Color(1f, 1f, 0f);
			}

			switch (Item.Type)
			{
				case ItemType.Helm:
					Image.sprite = Helm;
					break;
				case ItemType.Chest:
					Image.sprite = Chest;
					break;
				case ItemType.Gloves:
					Image.sprite = Gloves;
					break;
				case ItemType.Belt:
					Image.sprite = Belt;
					break;
				case ItemType.Pants:
					Image.sprite = Pants;
					break;
				case ItemType.Boots:
					Image.sprite = Boots;
					break;
				case ItemType.Amulet:
					Image.sprite = Amulet;
					break;
				case ItemType.Ring:
					Image.sprite = Ring;
					break;
				case ItemType.MainHand:
					Image.sprite = MainHand;
					break;
				case ItemType.OffHand:
					Image.sprite = OffHand;
					break;
			}
			Image.color = new Color(0f, 0f, 0f, 1f);
		}
		else
		{
			Image panel = GetComponentInChildren<Image>();
			panel.color = new Color(0.8f, 0.8f, 0.8f);

			Image.sprite = null;
			Image.color = new Color(0f, 0f, 0f, 0f);
		}

		if (DoubleClickTimer > 0.5f)
		{
			DoubleClickTimer = -1f;
		}
		else if (DoubleClickTimer >= 0f)
		{
			DoubleClickTimer += Time.deltaTime;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (ShowToolTip && Item != null)
		{
			List<string> mainContent = new();
			foreach (Affix affix in Item.Affixes)
			{
				mainContent.Add($"{affix.Type}: +{affix.Value * 100}%");
			}
			List<string> secondContent = new();
			Item equippedItem = PlayerEngine.Equipment.Where(x => x.Type == Item.Type).FirstOrDefault();
			if (equippedItem != null && Item.Id != equippedItem.Id)
			{
				foreach (Affix affix in equippedItem.Affixes)
				{
					secondContent.Add($"{affix.Type}: +{affix.Value * 100}%");
				}
			}

			TooltipHandler._instance.SetAndShowTooltip(mainContent, secondContent, "Equipped");
		}
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		TooltipHandler._instance.HideTooltip();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (Item != null)
		{
			if (GraphicalEngine.SelectItemToCraftMode)
			{
				GraphicalEngine.ItemToCraft = Item.Id;
				GraphicalEngine.SelectItemToCraftMode = false;
				GraphicalEngine.SwitchToTab("Craft");
			}
			else if (DoubleClickTimer >= 0f && DoubleClickTimer <= 0.5f)
			{
				DoubleClickTimer = -1f;
				TooltipHandler._instance.HideTooltip();

				if (GraphicalEngine.SalvageMode)
				{
					GameEngine.SalvageItem(Item);
				}
				else
				{
					GameEngine.EquipOrUnequipItem(Item);
				}
			}
			else
			{
				DoubleClickTimer = 0f;
			}
		}
	}
}
