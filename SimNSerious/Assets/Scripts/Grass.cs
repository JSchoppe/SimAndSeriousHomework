using UnityEngine;

/// <summary>Generates a hilly grass texture using perlin noise</summary>
public sealed class Grass : MonoBehaviour
{
    #region Inspector Fields
    [SerializeField] private Material moundMaterial;
    [SerializeField] private Transform minCorner;
    [SerializeField] private Transform maxCorner;
    [SerializeField][Range(0, 10)] private float detailStep;
    [SerializeField] private float visualTurbulenceStep;
    [SerializeField] private float visualTurbulenceMagnitude;
    #endregion

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

        // Setup the mesh generation.
        int xCuts = Mathf.FloorToInt((max.x - min.x) / detailStep);
        int zCuts = Mathf.FloorToInt((max.z - min.z) / detailStep);
        Mesh surfaceMesh = MeshUtilities.GenerateGrid(xCuts, zCuts, detailStep);

        // Add noise to generate the mounds.
        Vector3[] verts = surfaceMesh.vertices;
        for (int i = 0; i < verts.Length; i++)
        {
            // Use perlin noise relative to location.
            verts[i].y = visualTurbulenceMagnitude * Mathf.PerlinNoise(
                verts[i].x * visualTurbulenceStep,
                verts[i].z * visualTurbulenceStep
            );
        }
        surfaceMesh.vertices = verts;

        // Assign the generated mesh data and create the mesh components.
        MeshRenderer MR = gameObject.AddComponent<MeshRenderer>();
        MeshFilter MF = gameObject.AddComponent<MeshFilter>();
        MR.material = moundMaterial;
        MF.mesh = surfaceMesh;
    }
}
