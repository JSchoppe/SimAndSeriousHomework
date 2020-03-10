using UnityEngine;

public static class Probability
{
    public static bool Chance(float likelihood)
    {
        return Random.Range(0f, 1) < likelihood;
    }

    public static float NextGaussian(float min, float max, float standardDeviation, float mean)
    {
        float center = 0.5f * (min + max);
        float num = Random.Range(Random.Range(min, center), Random.Range(center, max));
        return standardDeviation * (num + mean);
    }
}
