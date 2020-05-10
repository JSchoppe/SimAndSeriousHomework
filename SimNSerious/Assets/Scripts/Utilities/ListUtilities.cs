using System.Collections.Generic;
using UnityEngine;
using System;

public static class ListUtilities
{
    /// <summary>Shuffles a list in place using the Fisher-Yates algorithm</summary>
    /// <param name="list">The list to shuffle(in place)</param>
    public static void Shuffle<T>(ref List<T> list)
    {
        int i = list.Count;
        while (i > 1)
        {
            i--;
            // Choose a lower element to swap with.
            int j = UnityEngine.Random.Range(0, i + 1);
            // Swap the two elements.
            T value = list[j];
            list[j] = list[i];
            list[i] = value;
        }
    }
    /// <summary>Subtracts items of the right hand argument from the left hand argument</summary>
    /// <param name="LHS">The elements to keep if not present in RHS</param>
    /// <param name="RHS">The elements to be removed if existant in LHS</param>
    /// <returns>A new list containing elements of LHS not in RHS</returns>
    public static List<IEquatable> Negation<IEquatable>(ref List<IEquatable> LHS, ref List<IEquatable> RHS)
    {
        List<IEquatable> result = new List<IEquatable>();
        foreach(IEquatable item in LHS)
        {
            // Check each item and exclude it if it exists in the second argument list.
            bool isExcluded = false;
            foreach(IEquatable exclusion in RHS)
            {
                if(item.Equals(exclusion))
                {
                    isExcluded = true;
                    break;
                }
            }
            if(!isExcluded)
            {
                result.Add(item);
            }
        }
        return result;
    }
}
