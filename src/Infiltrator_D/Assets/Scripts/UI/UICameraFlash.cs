using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICameraFlash : MonoBehaviour
{

    // Duration of flash event
    public float Duration;

    private Image flash;

    // The static tracker
    public static UICameraFlash ActiveInScene;

    // Use this for initialization
    void Start()
    {
        ActiveInScene = this;
        flash = GetComponent<Image>();
        flash.CrossFadeAlpha(0, 0, true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Displays a flash, returns the duration
    public float Show()
    {
        if (!this.isActiveAndEnabled)
        {
            return 0;
        }

        StartCoroutine(CauseFlash());
        return Duration;
    }

    // Hides the flash
    public void Hide()
    {
        if (!this.isActiveAndEnabled)
        {
            return;
        }
        StopAllCoroutines();
        flash.CrossFadeAlpha(0, 0, true);
    }

    IEnumerator CauseFlash()
    {
        flash.CrossFadeAlpha(1, Duration, false);
        yield return new WaitForSeconds(Duration / 2);
        flash.CrossFadeAlpha(0, Duration / 2, false);
    }
}
