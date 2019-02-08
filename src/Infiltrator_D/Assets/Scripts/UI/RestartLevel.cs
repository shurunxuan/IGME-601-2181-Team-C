using UnityEngine;

public class RestartLevel : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Backspace))
        {
            MenuManager.ActiveManager.ReloadStage();
        }
    }
}
