using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public GameObject AdventureTab;
    public GameObject ArmoryTab;
    public GameObject CraftTab;

	void Start()
    {
        GameEngine.Init(AdventureTab, ArmoryTab, CraftTab);
	}

    void FixedUpdate()
    {
        GameEngine.Advance(Time.fixedDeltaTime);
    }
}
