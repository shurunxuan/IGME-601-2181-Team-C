using System.Collections.Generic;
using UnityEngine;

public class TopSecretInfo : MonoBehaviour
{

    public enum InfoType
    {
        Visual, Digital
    }

    // Static info handler
    private static List<string> _allInfo;

    // Personal info
    public string Info;
    public InfoType Type;

    static TopSecretInfo()
    {
        _allInfo = new List<string>();
    }

    private void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {
        // Ensure this piece of info is tracked
        if (!_allInfo.Contains(Info))
        {
            _allInfo.Add(Info);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    // STATIC METHODS

    public static void ClearInfo()
    {
        _allInfo.Clear();
    }

    public static int InfoCount()
    {
        return _allInfo.Count;
    }
}
