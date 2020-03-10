using UnityEngine;

public sealed class ShrinkingNode : MonoBehaviour
{
    private static float shrinkDuration = 1;
    private float spawnTime;
    private Vector3 spawnScale;

    private void Start()
    {
        spawnTime = Time.time;
        spawnScale = transform.localScale;
    }

    private void Update()
    {
        float interpolant = (Time.time - spawnTime) / shrinkDuration;

        if(interpolant > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.localScale = Vector3.Lerp(spawnScale, Vector3.zero, interpolant);
        }
    }
}
