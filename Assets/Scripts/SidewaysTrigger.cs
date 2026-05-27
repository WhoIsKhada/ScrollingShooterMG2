using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidewaysTrigger : MonoBehaviour
{
    [Header("Transition")]
    public float lerpTime = 2f;

    [Header("Sideways Curve (X axis)")]
    public float minSidewaysStrength = -0.0001f;
    public float maxSidewaysStrength = 0.0001f;

    [Header("Backways Curve (Y axis)")]
    public float minBackwaysStrength = -0.0001f;
    public float maxBackwaysStrength = 0.0001f;

    public float triggerInterval = 50f;
    public TunnelSpawner tunnelSpawner;

    public float currentSideways;
    public float currentBackways;
    private bool isLerping = false;
    private float _lastTriggerZ = 0f;
    private const string PROP_SIDEWAYS = "_Sideways_Strength";
    private const string PROP_BACKWAYS = "_Backways_Strength";

    void Update()
    {
        if (!isLerping && transform.position.z > _lastTriggerZ + triggerInterval)
        {
            _lastTriggerZ = transform.position.z;
            StartCoroutine(ChangeCurveStrength());
        }
    }

    private IEnumerator ChangeCurveStrength()
    {
        isLerping = true;
        float elapsed = 0f;
        float startSideways = currentSideways;
        float startBackways = currentBackways;
        float targetSideways = Random.Range(minSidewaysStrength, maxSidewaysStrength);
        float targetBackways = Random.Range(minBackwaysStrength, maxBackwaysStrength);

        while (elapsed < lerpTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / lerpTime);
            currentSideways = Mathf.Lerp(startSideways, targetSideways, t);
            currentBackways = Mathf.Lerp(startBackways, targetBackways, t);

            foreach (GameObject chunk in tunnelSpawner.GetActiveChunks())
            {
                foreach (Renderer r in chunk.GetComponentsInChildren<Renderer>())
                {
                    r.material.SetFloat(PROP_SIDEWAYS, currentSideways);
                    r.material.SetFloat(PROP_BACKWAYS, currentBackways);
                }
            }

            yield return null;
        }

        currentSideways = targetSideways;
        currentBackways = targetBackways;
        isLerping = false;
    }

    private void OnApplicationQuit() => ResetMaterials();

    private void ResetMaterials()
    {
        if (tunnelSpawner == null) return;
        foreach (GameObject chunk in tunnelSpawner.GetActiveChunks())
        {
            foreach (Renderer r in chunk.GetComponentsInChildren<Renderer>())
            {
                r.material.SetFloat(PROP_SIDEWAYS, 0f);
                r.material.SetFloat(PROP_BACKWAYS, 0f);
            }
        }
    }
}