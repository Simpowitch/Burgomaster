using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public Camera c;

    public float cloudStartRender = 5f, cloudFullRender = 10f;
    public SpriteRenderer cloudRenderer;

    private void Update()
    {
        float cameraSize = c.orthographicSize;

        float cloudStrength = 1;
        if (cameraSize < cloudStartRender)
            cloudStrength = 0;
        else if (Utility.Between(cloudFullRender, cloudStartRender, cameraSize))
        {
            float percentage = (cameraSize - cloudStartRender) / (cloudFullRender - cloudStartRender);
            cloudStrength = percentage;
        }

        cloudRenderer.material.SetFloat(Shader.PropertyToID("EffectStrength"), cloudStrength);
    }
}
