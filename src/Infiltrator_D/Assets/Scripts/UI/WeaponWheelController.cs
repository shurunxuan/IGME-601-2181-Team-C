using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelController : MonoBehaviour
{

    public Sprite WeaponWheelPart;
    public Sprite WeaponWheelSelectedPart;

    public List<Image> Parts;
    public List<Image> Icons;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Selecting(int tool)
    {
        // Don't need to worry about if tool == -1
        for (int i = 0; i < 4; ++i)
        {
            Parts[i].sprite = (i == tool) ? WeaponWheelSelectedPart : WeaponWheelPart;
        }
    }
}
