using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    public GameObject ButtonPrefab;
    public GameObject AdventureTab;
    public GameObject ArmoryTab;

    private readonly List<string> Menus = new()
    {
        "Adventure",
        "Armory"
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
            menuGameObject.GetComponent<Button>().onClick.AddListener(() => SwitchToTab(menu));
            offset += 96;
        }

		SwitchToTab("Adventure");
	}

    private void SwitchToTab(string name)
    {
        AdventureTab.SetActive(name == "Adventure");
		ArmoryTab.SetActive(name == "Armory");
    }
}
