using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ArmoryHandler : MonoBehaviour
{
    public GameObject ItemPrefab;
    
    private readonly List<GameObject> Inventory = new();

    void Start()
    {
        Rect rect = GetComponent<RectTransform>().rect;
		float width = rect.width * 0.95f;
        float height = rect.height * 0.95f;
        int xOffset = 10;
        int yOffset = 32;
        for (int i = 0; i < 50; i++)
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
    }

    void Update()
    {
        
    }
}
