using UnityEngine;

public class GameHandler : MonoBehaviour
{
    void Start()
    {
        GameEngine.Init();
	}

    void FixedUpdate()
    {
        GameEngine.Advance(Time.fixedDeltaTime);
    }
}
