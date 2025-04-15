using Assets.Scripts.Models;
using System.Net.NetworkInformation;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemHandler : MonoBehaviour
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

    void Update()
    {
		if (Item != null)
		{
			Image panel = GetComponentInChildren<Image>();

			if (Item.Affixes.Count == 1)
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
	}
}
