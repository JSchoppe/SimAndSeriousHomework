using System.Collections.Generic;
using UnityEngine;

/// <summary>Represents a liquid volume in 3D space as a world-axis aligned rectangular prism</summary>
public sealed class Liquid : MonoBehaviour
{
    /// <summary>All liquid volumes that have been initialized in the scene</summary>
    public static List<Liquid> SceneLiquids { get; private set; } = new List<Liquid>();

    #region Inspector Fields
    [SerializeField] private Material fluidMaterial;
    [SerializeField] private Transform minCorner;
    [SerializeField] private Transform maxCorner;
    [SerializeField][Range(0,10)] private float detailStep;
    [SerializeField] private float visualTurbulenceStep;
    [SerializeField] private float visualTurbulenceMagnitude;
    [SerializeField] private float visualTurbulenceRate;
    /// <summary>Density of this fluid in mass per unit cubed</summary>
    [Range(0, 1000)] public float Density;
    #endregion
    #region Internal Members
    private Mesh surfaceMesh;
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

        // Setup the mesh generation.
        int xCuts = Mathf.FloorToInt((max.x - min.x) / detailStep);
        int zCuts = Mathf.FloorToInt((max.z - min.z) / detailStep);
        surfaceMesh = MeshUtilities.GenerateGrid(xCuts, zCuts, detailStep);

        // Assign the generated mesh data and create the mesh components.
        MeshRenderer MR = gameObject.AddComponent<MeshRenderer>();
        MeshFilter MF = gameObject.AddComponent<MeshFilter>();
        MR.material = fluidMaterial;
        MF.mesh = surfaceMesh;
        // Make this liquid accessible to other scene objects.
        SceneLiquids.Add(this);
    }
    private void Update()
    {
        // Animate the fluid surface mesh.
        Vector3[] verts = surfaceMesh.vertices;
        for(int i = 0; i < verts.Length; i++)
        {
            // Use perlin noise relative to location and time.
            verts[i].y = visualTurbulenceMagnitude * Mathf.PerlinNoise(
                Time.time * visualTurbulenceRate,
                Mathf.PerlinNoise(
                    verts[i].x * visualTurbulenceStep,
                    verts[i].z * visualTurbulenceStep
                )
            );
        }
        surfaceMesh.vertices = verts;
    }
    #endregion
    #region IsObjectInside
    /// <summary>Checks to see if this object's origin is inside the liquid volume</summary>
    /// <param name="gameObject">Object whose position is checked</param>
    /// <returns>Returns true if the object is inside the volume</returns>
    public bool IsObjectInside(GameObject gameObject)
    {
        return IsObjectInside(gameObject.transform);
    }
    /// <summary>Checks to see if this transform is inside the liquid volume</summary>
    /// <param name="transform">Transform whose position is checked</param>
    /// <returns>Returns true if the transform is inside the volume</returns>
    public bool IsObjectInside(Transform transform)
    {
        return IsObjectInside(transform.position);
    }
    /// <summary>Checks to see if a vector position is inside the liquid volume</summary>
    /// <param name="vector">Vector to check</param>
    /// <returns>Returns true if the vector is inside the volume</returns>
    public bool IsObjectInside(Vector3 vector)
    {
        return vector.x > minCorner.position.x
            && vector.x < maxCorner.position.x
            && vector.y > minCorner.position.y
            && vector.y < maxCorner.position.y
            && vector.z > minCorner.position.z
            && vector.z < maxCorner.position.z;
    }
    #endregion
}
