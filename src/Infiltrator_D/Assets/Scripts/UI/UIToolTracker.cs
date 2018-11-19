using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToolTracker : MonoBehaviour
{
    public PlayerController Player;
    private Text _text;

    // Use this for initialization
    void Start()
    {
        _text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player != null)
        {
            _text.text = "" + (Player.CurrentTool == -1 ? "No Tool Equipped" : Player.tools[Player.CurrentTool].GetName());
        }
    }
}
