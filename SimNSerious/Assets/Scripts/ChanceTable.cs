using System.Collections.Generic;
using UnityEngine;

/// <summary>Stores outcomes and their odds, automates randomization</summary>
/// <typeparam name="T">The type of the outcome when rolled</typeparam>
public sealed class ChanceTable<T>
{
    private List<double> weights;
    private List<T> outcomes;
    private double totalWeight;

    public ChanceTable()
    {
        weights = new List<double>();
        outcomes = new List<T>();
        totalWeight = 0;
    }

    /// <summary>Adds a new possible outcome to the chance table</summary>
    /// <param name="outcome">The actual outcome returned from Roll()</param>
    /// <param name="weight">The relative weight of this outcome being chosen</param>
    public void AddOutcome(T outcome, double weight)
    {
        outcomes.Add(outcome);
        weights.Add(weight);
        totalWeight += weight;
    }

    /// <summary>Removes an outcome from this table</summary>
    /// <param name="outcome">The outcome to be removed</param>
    public void RemoveOutcome(T outcome)
    {
        int targetIndex = outcomes.IndexOf(outcome);
        totalWeight -= weights[targetIndex];
        weights.RemoveAt(targetIndex);
        outcomes.RemoveAt(targetIndex);
    }

    /// <summary>Draws a random outcome from this table, removing it</summary>
    /// <returns>The object that was drawn</returns>
    public T Draw()
    {
        T chosen = Roll();
        RemoveOutcome(chosen);
        return chosen;
    }

    /// <summary>Chooses an element in the table based on weights</summary>
    /// <returns>The object that was rolled</returns>
    public T Roll()
    {
        float randomValue = Random.Range(0, (float)totalWeight);

        // The accumulator goes through the outcomes array and
        // gives each outcome a partition of the total weight.
        double accumulator = 0;
        for(int i = 0; i < weights.Count; i++)
        {
            accumulator += weights[i];
            if (randomValue < accumulator)
            {
                return outcomes[i];
            }
        }
        return default;
    }

    /// <summary>Outputs the current state of the table in percentage chances</summary>
    /// <returns>A comma seperated string of all current values</returns>
    public override string ToString()
    {
        string message = "";
        for (int i = 0; i < weights.Count; i++)
        {
            message += (weights[i] / totalWeight * 100).ToString("f2") + "%: ";
            message += outcomes[i].ToString() + ",\t";
        }
        return message;
    }
}
