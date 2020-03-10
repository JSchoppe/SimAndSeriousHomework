using System.Collections;
using UnityEngine;

public sealed class Exercise1 : MonoBehaviour
{
    // Get scene references.
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private Walker walker;
    [SerializeField] private float iterationStep;

    private void Start()
    {
        StartCoroutine(UpdateWalker());
    }

    // Move the walker every iterationStep seconds.
    private IEnumerator UpdateWalker()
    {
        while(true)
        {
            walker.Step();

            // Create a node to trace the movement of the walker.
            Instantiate(nodePrefab, walker.transform.position, Quaternion.identity);

            yield return new WaitForSeconds(iterationStep);
        }
    }
}
