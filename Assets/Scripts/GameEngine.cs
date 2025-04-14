using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEngine : MonoBehaviour
{
    void Start()
    {
        Game.Init();
	}

    void FixedUpdate()
    {
        Game.Advance(Time.fixedDeltaTime);
    }
}
