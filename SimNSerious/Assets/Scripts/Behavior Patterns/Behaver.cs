using System.Collections.Generic;
using UnityEngine;

/// <summary>Abstraction of scene entities that behave over time and can be aware of each other</summary>
public abstract class Behaver : MonoBehaviour
{
    private static List<Behaver> SceneBehavers = new List<Behaver>();

    protected Rigidbody body;
    protected void Initialize()
    {
        // Get required components.
        body = gameObject.GetComponent<Rigidbody>();

        SceneBehavers.Add(this);
        RandomizeTraits();
    }

    /// <summary>Rerolls the traits on this behaver</summary>
    public abstract void RandomizeTraits();

    #region Static Utilities
    /// <summary>Chooses a random behaver currently in the scene</summary>
    /// <returns>The random behaver</returns>
    public static Behaver GetRandomInScene()
    {
        return GetRandomInScene(new List<Behaver>());
    }
    /// <summary>Chooses a random behaver currently in the scene</summary>
    /// <param name="excludedBehaver">Behaver that should be excluded</param>
    /// <returns>The random behaver</returns>
    public static Behaver GetRandomInScene(Behaver excludedBehaver)
    {
        return GetRandomInScene(new List<Behaver>(){ excludedBehaver });
    }
    /// <summary>Chooses a random behaver currently in the scene</summary>
    /// <param name="excludedBehavers">Behavers that should be excluded</param>
    /// <returns>The random behaver</returns>
    public static Behaver GetRandomInScene(List<Behaver> excludedBehavers)
    {
        // Get a list of possible selections.
        List<Behaver> includedBehavers =
            ListUtilities.Negation(ref SceneBehavers, ref excludedBehavers);

        // Are there any selections available?
        if(includedBehavers.Count > 0)
        {
            // Shuffle the possible behavers and return the first index.
            ListUtilities.Shuffle(ref includedBehavers);
            return includedBehavers[0];
        }
        else
        {
            // Return null and warn if no behavers were found.
#if DEBUG
            Debug.LogWarning("Attempted to get a random behaver, but none were available.");
#endif
            return null;
        }
    }
    #endregion
}
