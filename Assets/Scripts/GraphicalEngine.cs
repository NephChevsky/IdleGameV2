using UnityEngine;

public class GraphicalEngine : MonoBehaviour
{
    public GameObject EntityPrefab;

    private GameObject Player;

    void Start()
    {
        Player = InstantiateEntity();
    }

    void Update()
    {
        float width = GetComponent<RectTransform>().rect.width;
        float border = width * 0.025f;
        width -= border * 2;

        Player.transform.localPosition = new Vector2(- (width / 2f) + Game.Player.Position * width, 0);
    }

    private GameObject InstantiateEntity()
    {
        GameObject gameObject = Instantiate(EntityPrefab);
        gameObject.transform.SetParent(this.gameObject.transform);
		gameObject.transform.localScale = Vector3.one /2f;
		return gameObject;
	}
}
