using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class LightFlicker : MonoBehaviour
{
    Light2D source;

    [Range(0f, 2f)]
    public float minIntensity = 0.1f, maxIntensity = 2f;
    [Range(0f, 1f)]
    public float minTimeChange = 0.1f, maxTimeChange = 0.75f;

    private void Awake()
    {
        source = GetComponent<Light2D>();
    }

    private void Start()
    {
        StartCoroutine(Lerp());
    }

    IEnumerator Lerp()
    {
        float startIntensity = source.intensity;
        float t = 0;
        float deltaTime = 0;

        float targetIntensity = Random.Range(minIntensity, maxIntensity);
        float changeTime = Random.Range(minTimeChange, maxTimeChange);

        while(t < 1)
        {
            deltaTime += Time.deltaTime;
            t = deltaTime / changeTime;

            source.intensity = Mathf.Lerp(startIntensity, targetIntensity, t);
            yield return null;
        }

        source.intensity = targetIntensity;
        yield return null;

        StartCoroutine(Lerp());
    }
}
