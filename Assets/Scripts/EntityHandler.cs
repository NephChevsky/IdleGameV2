using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityHandler : MonoBehaviour
{
	public float LifeRatio;
	public bool IsPlayer;

	void Start()
    {
        
    }

    void Update()
    {
		Image image = GetComponent<Image>();
		image.color = IsPlayer ? new Color(1f - LifeRatio, LifeRatio, 0) : new Color(LifeRatio, LifeRatio, LifeRatio);
		TMP_Text text = GetComponentInChildren<TMP_Text>();
		text.text = $"{Mathf.RoundToInt(LifeRatio * 100f)} %";
	}
}
