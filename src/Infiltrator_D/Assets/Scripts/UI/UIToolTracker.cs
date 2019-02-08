using UnityEngine;
using UnityEngine.UI;

public class UIToolTracker : MonoBehaviour
{
    public RectTransform MaskedImageTransform;
    private ToolComponent currentTool;
    private Image icon;

    // The most recently awoken ToolTracker
    public static UIToolTracker ActiveInScene { get; private set; }

    void Awake()
    {
        ActiveInScene = this;
    }

    void Start()
    {
        icon = gameObject.GetComponent<Image>();
    }


    // Update is called once per frame
    void Update()
    {
        float percentage = Mathf.Lerp(0.0f, -100.0f, currentTool.CooldownPercentage());
        if (float.IsNaN(percentage)) percentage = -100.0f;
        MaskedImageTransform.localPosition = new Vector3(0.0f, percentage, 0.0f);
    }

    public void SetCurrentTool(ToolComponent tool)
    {
        currentTool = tool;
        icon.sprite = currentTool.Icon;
    }
}
