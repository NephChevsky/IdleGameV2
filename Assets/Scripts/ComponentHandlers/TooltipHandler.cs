using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipHandler : MonoBehaviour
{
	public static TooltipHandler _instance;
	public GameObject Second;
	public GameObject SecondHeader;

	private float VisibilityTimer = 0f;

	private readonly List<GameObject> MainLines = new();
	private readonly List<GameObject> SecondLines = new();

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			_instance = this;
		}
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void ResetStatics()
	{
		_instance = null;
	}

	void Start()
	{
		Cursor.visible = true;
		gameObject.SetActive(false);

	}

	void Update()
	{
		if (gameObject.activeSelf && VisibilityTimer > 0.5f)
		{
			UpdatePosition();
		}
		else
		{
			VisibilityTimer += Time.deltaTime;
		}
	}

	public void SetAndShowTooltip(List<string> mainContent, List<string> secondContent = null, string secondHeaderText = null)
	{
		VisibilityTimer = 0f;
		transform.SetAsLastSibling();
		gameObject.SetActive(true);
		transform.localPosition = new Vector2(-1000, -1000);

		RectTransform rt = GetComponent<RectTransform>();

		if (secondContent == null || secondContent.Count == 0)
		{
			Second.SetActive(false);
			SecondHeader.SetActive(false);
		}
		else
		{
			Second.SetActive(true);
			SecondHeader.SetActive(!string.IsNullOrEmpty(secondHeaderText));
			SecondHeader.GetComponentInChildren<TMP_Text>().text = secondHeaderText;

			for (int i = 0; i < secondContent.Count; i++)
			{
				GameObject go = new($"Line {i}");
				go.transform.SetParent(transform);
				go.transform.localScale = Vector3.one;
				go.transform.localPosition = new Vector2(0, 150 - i * 50);

				TextMeshProUGUI text = go.AddComponent<TextMeshProUGUI>();
				text.rectTransform.anchoredPosition = new Vector2(0, 0);
				text.rectTransform.sizeDelta = new Vector2(240, 25);
				text.transform.localPosition = new Vector2( rt.rect.width / 4, 50 - i * 25);
				text.text = secondContent[i];
				text.fontSize = 20;
				text.verticalAlignment = VerticalAlignmentOptions.Middle;
				MainLines.Add(go);
			}
		}

		for (int i = 0; i < mainContent.Count; i++)
		{
			GameObject go = new($"Line {i}");
			go.transform.SetParent(transform);
			go.transform.localScale = Vector3.one;
			go.transform.localPosition = new Vector2(0, 150 - i * 50);

			TextMeshProUGUI text = go.AddComponent<TextMeshProUGUI>();
			text.rectTransform.anchoredPosition = new Vector2(0, 0);
			text.rectTransform.sizeDelta = new Vector2(240, 25);
			text.transform.localPosition = new Vector2(-rt.rect.width / 4, 50 - i * 25);
			text.text = mainContent[i];
			text.fontSize = 20;
			text.verticalAlignment = VerticalAlignmentOptions.Middle;
			MainLines.Add(go);
		}
	}

	public void HideTooltip()
	{
		MainLines.ForEach(line =>
		{
			Destroy(line);
		});
		SecondLines.ForEach(line =>
		{
			Destroy(line);
		});
		MainLines.Clear();
		SecondLines.Clear();
		gameObject.SetActive(false);
	}

	private void UpdatePosition()
	{
		RectTransform canvasRT = gameObject.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRT, Input.mousePosition, Camera.main, out Vector2 localPoint))
		{
			RectTransform rt = GetComponent<RectTransform>();

			float xPos = localPoint.x + rt.rect.width / 2 + 5;
			float yPos = localPoint.y + rt.rect.height / 2 + 5;

			if (yPos + rt.rect.height / 2 > canvasRT.rect.height / 2 - 5)
			{
				xPos += 20;
				yPos = localPoint.y - rt.rect.height / 2 - 5;
			}

			if (xPos + (Second.activeInHierarchy ? rt.rect.width / 2 : 0) > canvasRT.rect.width / 2 - 5)
			{
				xPos = localPoint.x - (Second.activeInHierarchy ? rt.rect.width / 2 : 0) - 5;
			}

			transform.localPosition = new Vector2(xPos, yPos);
		}
	}
}
