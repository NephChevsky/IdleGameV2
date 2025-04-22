using Assets.Scripts.Engines;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    public GameObject ButtonPrefab;
    public GameObject AdventureTab;
    public GameObject ArmoryTab;
	public GameObject CraftTab;

	private readonly List<string> Menus = new()
    {
        "Adventure",
        "Armory",
        "Craft"
    };

    void Start()
    {
        float width = GetComponent<RectTransform>().rect.width * 0.95f;

        int offset = 2;
		foreach (string menu in Menus)
        {
            GameObject menuGameObject = Instantiate(ButtonPrefab);
            menuGameObject.GetComponentInChildren<TMP_Text>().text = menu;
            menuGameObject.transform.SetParent(transform);
            menuGameObject.transform.localScale = Vector3.one;
            menuGameObject.transform.localPosition = new Vector2(-width / 2 + offset, 0);
            menuGameObject.GetComponent<Button>().onClick.AddListener(() => GraphicalEngine.SwitchToTab(menu));
            offset += 96;
        }
	}
}
