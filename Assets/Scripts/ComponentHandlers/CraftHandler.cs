using Assets.Scripts.Engines;
using Assets.Scripts.Models.Items;
using System;
using System.Linq;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class CraftHandler : MonoBehaviour
{
    public GameObject Item;
    public GameObject Affix1Text;
	public GameObject Affix2Text;
	public GameObject Affix3Text;
	public GameObject Affix4Text;
	public GameObject Affix5Text;
	public GameObject SelectItemToCraftButton;
	public GameObject DoneButton;

    void Start()
    {
        SelectItemToCraftButton.GetComponent<Button>().onClick.AddListener(OnClick_SelectItemToCraft);
		DoneButton.GetComponent<Button>().onClick.AddListener(OnClick_CraftingDone);
    }

    void Update()
    {
        if (GraphicalEngine.ItemToCraft != Guid.Empty)
        {
			Item item = GameEngine.Inventory.Where(x => x.Id == GraphicalEngine.ItemToCraft).FirstOrDefault();
			item ??= PlayerEngine.Equipment.Where(x => x.Id == GraphicalEngine.ItemToCraft).FirstOrDefault();
			if (item != null)
			{
				ItemHandler itemHandler = Item.GetComponent<ItemHandler>();
				itemHandler.Item = item;
				itemHandler.AllowSalvageMode = false;
				itemHandler.ShowToolTip = false;

				if (item.Affixes.Count > 0)
				{
					Affix1Text.SetActive(true);
					Affix1Text.GetComponent<TMP_Text>().text = $"{item.Affixes[0].Type}: +{item.Affixes[0].Value * 100}%";
				}
				if (item.Affixes.Count > 1)
				{
					Affix2Text.SetActive(true);
					Affix2Text.GetComponent<TMP_Text>().text = $"{item.Affixes[1].Type}: +{item.Affixes[1].Value * 100}%";
				}
				if (item.Affixes.Count > 2)
				{
					Affix3Text.SetActive(true);
					Affix3Text.GetComponent<TMP_Text>().text = $"{item.Affixes[2].Type}: +{item.Affixes[2].Value * 100}%";
				}
				if (item.Affixes.Count > 3)
				{
					Affix4Text.SetActive(true);
					Affix4Text.GetComponent<TMP_Text>().text = $"{item.Affixes[3].Type}: +{item.Affixes[3].Value * 100}%";
				}
				if (item.Affixes.Count > 4)
				{
					Affix5Text.SetActive(true);
					Affix5Text.GetComponent<TMP_Text>().text = $"{item.Affixes[4].Type}: +{item.Affixes[4].Value * 100}%";
				}


				SelectItemToCraftButton.SetActive(false);
				DoneButton.SetActive(true);
			}
			else
			{
				GraphicalEngine.ItemToCraft = Guid.Empty;
			}
		}
        else
        {
			ItemHandler itemHandler = Item.GetComponent<ItemHandler>();
			itemHandler.Item = null;

			Affix1Text.SetActive(false);
			Affix2Text.SetActive(false);
			Affix3Text.SetActive(false);
			Affix4Text.SetActive(false);
			Affix5Text.SetActive(false);

			SelectItemToCraftButton.SetActive(true);
			DoneButton.SetActive(false);
		}
    }

    private void OnClick_SelectItemToCraft()
    {
        GraphicalEngine.SelectItemToCraftMode = true;
		GraphicalEngine.SwitchToTab("Armory");
    }

	private void OnClick_CraftingDone()
	{
		GraphicalEngine.ItemToCraft = Guid.Empty;
	}
}
