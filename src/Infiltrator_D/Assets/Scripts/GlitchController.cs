using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchController : MonoBehaviour {

    [SerializeField, Range(0, 1)]
    private float verticalJump = 0;

    [SerializeField, Range(0, 1)]
    private float scanLineJitter = 0;

    [SerializeField, Range(0, 1)]
    private float horizontalShake = 0;

    [SerializeField, Range(0, 1)]
    private float colorDrift = 0;

    [SerializeField]
    private GlitchSettings[] glitchSettingsPresets;
    private Dictionary<string, GlitchSettings> glitchSettingsPresetsDict;

    private Material material;
    private float verticalJumpTime;
    private string currentMode;

    private void JumpToGlitchMode(string name)
    {
        currentMode = name;
        GlitchSettings gs = glitchSettingsPresetsDict[currentMode];
        verticalJump = gs.verticalJump;
        scanLineJitter = gs.scanLineJitter;
        horizontalShake = gs.horizontalShake;
        colorDrift = gs.colorDrift;
    }

    private IEnumerator TransitionToGlitchMode(string name, float transitionTime)
    {
        GlitchSettings startSettings = glitchSettingsPresetsDict[currentMode];
        GlitchSettings targetSettings = glitchSettingsPresetsDict[name];

        float t = 0;
        while (t < transitionTime)
        {
            t += Time.deltaTime;
            float ratio = t / transitionTime;
            verticalJump = Mathf.Lerp(startSettings.verticalJump, targetSettings.verticalJump, ratio);
            scanLineJitter = Mathf.Lerp(startSettings.scanLineJitter, targetSettings.scanLineJitter, ratio);
            horizontalShake = Mathf.Lerp(startSettings.horizontalShake, targetSettings.horizontalShake, ratio);
            colorDrift = Mathf.Lerp(startSettings.colorDrift, targetSettings.colorDrift, ratio);
            yield return null;
        }

        JumpToGlitchMode(name);
        yield return null;
    }

    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    private void Start()
    {
        glitchSettingsPresetsDict = new Dictionary<string, GlitchSettings>();
        foreach(GlitchSettings gs in glitchSettingsPresets)
        {
            glitchSettingsPresetsDict.Add(gs.name, gs);
        }

        JumpToGlitchMode("None");
    }

    private void Update()
    {
        UpdateGlitch();
    }
    
    public void AnimateGlitch(Texture2D targetTexture)
    {
        StartCoroutine(AnimateGlitchCR(targetTexture));
    }

    private IEnumerator AnimateGlitchCR(Texture2D targetTexture)
    {
        yield return StartCoroutine(TransitionToGlitchMode("Standard", 0.2f));
        yield return StartCoroutine(TransitionToGlitchMode("Lots", 1));
        material.mainTexture = targetTexture;
        yield return StartCoroutine(TransitionToGlitchMode("Standard", 1));
        yield return StartCoroutine(TransitionToGlitchMode("None", 0.2f));
    }

    private void UpdateGlitch()
    {
        if (material == null) return;

        verticalJumpTime += Time.deltaTime * verticalJump * 11.3f;

        var sl_thresh = Mathf.Clamp01(1.0f - scanLineJitter * 1.2f);
        var sl_disp = 0.002f + Mathf.Pow(scanLineJitter, 3) * 0.05f;
        material.SetVector("_ScanLineJitter", new Vector2(sl_disp, sl_thresh));

        var vj = new Vector2(verticalJump, verticalJumpTime);
        material.SetVector("_VerticalJump", vj);

        material.SetFloat("_HorizontalShake", horizontalShake * 0.2f);

        var cd = new Vector2(colorDrift * 0.04f, Time.time * 606.11f);
        material.SetVector("_ColorDrift", cd);
    }

    [Serializable]
    private class GlitchSettings
    {
        public string name;
        public float verticalJump;
        public float scanLineJitter;
        public float horizontalShake;
        public float colorDrift;
    }

}
