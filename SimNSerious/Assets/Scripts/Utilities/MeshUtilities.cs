using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshUtilities
{
    public static Mesh GenerateGrid(int xCuts, int zCuts, float cutWidth)
    {
        // Allocate space for the grid of vertices.
        Vector3[] verts = new Vector3[(xCuts + 1) * (zCuts + 1)];
        // Allocate space for the grid of tris(3 indices per tri, 2 tris per cell).
        int[] tris = new int[6 * xCuts * zCuts];

        // Generate the mesh in a flat state.
        int i = 0;
        for (int x = 0; x < xCuts + 1; x++)
        {
            for (int z = 0; z < zCuts + 1; z++)
            {
                // Generate the vertices in a grid pattern.
                verts[i] = new Vector3(x * cutWidth, 0, z * cutWidth);
                i++;
            }
        }
        i = 0;
        for (int x = 0; x < xCuts; x++)
        {
            for (int z = 0; z < zCuts; z++)
            {
                // Generate the triangles in a cell pattern.
                //  z-->
                // x
                // |  a--b    0--1--2--3--4
                // V  |\ |    |\ |\ |\ |\ |
                //    | \|    | \| \| \| \|
                //    c--d    5--6--7--8--9
                int a = x * (zCuts + 1) + z;
                int b = a + 1;
                int c = a + (zCuts + 1);
                int d = c + 1;
                // Connect the dots.
                tris[i] = a;
                tris[i + 1] = b;
                tris[i + 2] = d;
                i += 3;
                tris[i] = a;
                tris[i + 1] = d;
                tris[i + 2] = c;
                i += 3;
            }
        }

        // Make the mesh.
        Mesh meshGrid = new Mesh();
        meshGrid.vertices = verts;
        meshGrid.triangles = tris;
        return meshGrid;
    }
}
