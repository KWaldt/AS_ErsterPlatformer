using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// This doesn't necessarily stack well with multiple instances. Use carefully.
public class ChangeSaturationAction : ActionBase
{
    [Range(-100, 100)]
    public float saturationAmount;
    public float fadeDuration = 0.2f;
    public float durationInSeconds = 1f;
    [SerializeField] private PostProcessVolume postProcessVolume = null;

    private float originalSaturation;
    private ColorGrading colorGrading;
     
    public override void DoAction(Collider2D other)
    {
        if (postProcessVolume == null)
            return;
        
        postProcessVolume.profile.TryGetSettings(out colorGrading);
        
        if (colorGrading == null)
            return;

        originalSaturation = GetSaturation();
        StartCoroutine(WaitAndChange());
    }

    private IEnumerator WaitAndChange()
    {
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeDuration)
        {
            float newValue = Mathf.Lerp(GetSaturation(), saturationAmount, t);
            SetSaturation(newValue);
            yield return null;
        }
        
        yield return new WaitForSeconds(durationInSeconds);
        
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeDuration)
        {
            float newValue = Mathf.Lerp(GetSaturation(), originalSaturation, t);
            SetSaturation(newValue);
            yield return null;
        }
        SetSaturation(originalSaturation);
    }

    private float GetSaturation()
    {
        return colorGrading.saturation.value;
    }
    
    private void SetSaturation(float value)
    {
        colorGrading.saturation.value = value;
    }
}
