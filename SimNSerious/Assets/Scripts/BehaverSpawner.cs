using UnityEngine;

/// <summary>Spawns instances of behavers on demand in a specified region</summary>
public sealed class BehaverSpawner : MonoBehaviour
{
    #region Inspector Fields
    [SerializeField] private GameObject behaverPrefab;
    [SerializeField] private Transform minCorner;
    [SerializeField] private Transform maxCorner;
    [SerializeField] private bool oneTimeSpawn = true;
    [SerializeField][Range(1, 100)] private int minQuantity = 1;
    [SerializeField][Range(1, 100)] private int maxQuantity = 10;
    #endregion

    #region MonoBehaviour
    private void Start()
    {
        // Reorient the transforms if they are not on the diagonal
        // pointing towards the world origin.
        Vector3 min, max;
        min.x = Mathf.Min(minCorner.position.x, maxCorner.position.x);
        min.y = Mathf.Min(minCorner.position.y, maxCorner.position.y);
        min.z = Mathf.Min(minCorner.position.z, maxCorner.position.z);
        max.x = Mathf.Max(minCorner.position.x, maxCorner.position.x);
        max.y = Mathf.Max(minCorner.position.y, maxCorner.position.y);
        max.z = Mathf.Max(minCorner.position.z, maxCorner.position.z);
        minCorner.position = min;
        maxCorner.position = max;

        if(oneTimeSpawn)
        {
            // Spawn the behavers and allow this instance
            // to be garbage collected.
            SpawnWave();
            Destroy(this);
        }
    }
    #endregion

    /// <summary>Instantaneously spawns a number of behavers randomly in the spawn range</summary>
    public void SpawnWave()
    {
        // Spawn a random quantity of behavers at random positions inside the region.
        int quantity = Random.Range(minQuantity, maxQuantity + 1);
        for(int i = 0; i < quantity; i++)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(minCorner.position.x, maxCorner.position.x),
                Random.Range(minCorner.position.y, maxCorner.position.y),
                Random.Range(minCorner.position.z, maxCorner.position.z)
            );
            Instantiate(behaverPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
