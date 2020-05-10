using UnityEngine;

/// <summary>Represents a one-to-one function where the output is the relative likelihood of the input</summary>
/// <param name="x">A possible value in the distribution</param>
/// <returns>The relative likelihood of the input value occuring</returns>
public delegate float DistributionCurve(float x);

/// <summary>Simulates a distribution using an approximated bar graph to select outcomes</summary>
public sealed class Distribution
{
    // Default distribution settings:
    // Precision controls how accurate the distribution
    // generator is at the cost of efficiency.
    private const int defaultPrecision = 50;

    // Precalculated bar graph components.
    private readonly float min, barWidth;
    private readonly float totalBarHeight;
    private readonly float[] barHeights;

    #region Constructor
    /// <summary>Creates an even disribution between min and max</summary>
    /// <param name="min">The minimum possible value in this distribution</param>
    /// <param name="max">The maximum possible value in this distribution</param>
    public Distribution(float min, float max)
        : this((float x) => { return 1; }, min, max, 1) { }
    /// <summary>Creates an approximate simulation of a distribution between 0 and 1</summary>
    /// <param name="curve">The mathematical function of the distribution(between 0 and 1)</param>
    public Distribution(DistributionCurve curve)
        : this(curve, 0, 1) { }
    /// <summary>Creates an approximate simulation of a distribution</summary>
    /// <param name="curve">The mathematical function of the distribution(between min and max)</param>
    /// <param name="min">The minimum possible value in this distribution</param>
    /// <param name="max">The maximum possible value in this distribution</param>
    public Distribution(DistributionCurve curve, float min, float max)
        : this(curve, min, max, defaultPrecision) { }
    /// <summary>Creates an approximate simulation of a distribution</summary>
    /// <param name="curve">The mathematical function of the distribution(between min and max)</param>
    /// <param name="min">The minimum possible value in this distribution</param>
    /// <param name="max">The maximum possible value in this distribution</param>
    /// <param name="precision">Increasing this will improve the accuracy of the generator at the cost of speed</param>
    public Distribution(DistributionCurve curve, float min, float max, int precision)
    {
#if DEBUG
        // Input checks:
        if(precision < 1)
        {
            Debug.LogError("Distribution constructor called with invalid precision: " + precision);
            Debug.Break();
        }
#endif
        // Define the left starting point and the width of the bars in the bar graph.
        this.min = min;
        this.barWidth = (max - min) / precision;

        // Precalculate a bar graph that will represent the distribution.
        barHeights = new float[precision];
        totalBarHeight = 0;
        for (int i = 0; i < precision; i++)
        {
            // Find the input point at the middle of this bar on the graph.
            float middle = (i + 0.5f) * barWidth;
            // Use the distribution curve to get the height of the bar.
            barHeights[i] = curve(middle);
            totalBarHeight += barHeights[i];
        }
    }
    #endregion

    /// <summary>Generates a new outcome based on the distribution curve</summary>
    /// <returns>A number inside the distribution range</returns>
    public float Next()
    {
        // Stack the bars on top of each other until the target height is reached.
        float targetHeight = Random.Range(0, totalBarHeight);
        float heightSum = 0;
        for(int i = 0; i < barHeights.Length; i++)
        {
            heightSum += barHeights[i];
            if(heightSum > targetHeight)
            {
                // This current stack of bars contains our desired outcome.
                // Choose a random point along the width of this bar.
                return min + (i + Random.Range(0, 1f)) * barWidth;
            }
        }
        // Return the last bar if all else fails.
        return min + barHeights.Length * barWidth;
    }
}
