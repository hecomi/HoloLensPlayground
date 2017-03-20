using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
public class NearDestructionEffect : MonoBehaviour 
{
    static Dictionary<Mesh, Mesh> destructableMeshTable = new Dictionary<Mesh, Mesh>();

    void Start() 
    {
        var meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = GetDestructableMesh(meshFilter); // clone if needed
        var mat = GetComponent<Renderer>().material; // just clone
	}

    Mesh GetDestructableMesh(MeshFilter meshFilter)
    {
        Mesh mesh;
        destructableMeshTable.TryGetValue(meshFilter.sharedMesh, out mesh);
        if (!mesh) {
            mesh = GenerateDestructableMesh(meshFilter);
            destructableMeshTable.Add(meshFilter.sharedMesh, mesh);
        }
        return mesh;
    }

    Mesh GenerateDestructableMesh(MeshFilter meshFilter)
    {
        var sharedMesh = meshFilter.sharedMesh;
        var sharedIndices = sharedMesh.GetIndices(0);

        var vertices = new List<Vector3>();
        var indices = new int[sharedIndices.Length];
        var normals = new List<Vector3>();
        var tangents = new List<Vector4>();
        var uv1 = new List<Vector2>();
        var uv2 = new List<Vector2>();
        var uv3 = new List<Vector3>();

        for (int i = 0; i < sharedIndices.Length / 3; ++i) {
            for (int j = 0; j < 3; ++j) {
                int n = 3 * i + j;
                int index = sharedIndices[n];
                indices[n] = n;
                vertices.Add(sharedMesh.vertices[index]);
                normals.Add(sharedMesh.normals[index]);
                tangents.Add(sharedMesh.tangents[index]);
                uv1.Add(sharedMesh.uv[index]);
                uv2.Add(new Vector2(i, i));
                uv3.Add((
                    (sharedMesh.vertices[sharedIndices[3 * i + 0]]) + 
                    (sharedMesh.vertices[sharedIndices[3 * i + 1]]) + 
                    (sharedMesh.vertices[sharedIndices[3 * i + 2]])) / 3f);
            }
        }

        var mesh = new Mesh();
        mesh.name = sharedMesh.name + " (Destructable)";
        mesh.SetVertices(vertices);
        mesh.SetIndices(indices, MeshTopology.Triangles, 0);
        mesh.SetNormals(normals);
        mesh.SetTangents(tangents);
        mesh.SetUVs(0, uv1);
        mesh.SetUVs(1, uv2);
        mesh.SetUVs(2, uv3);
        mesh.RecalculateBounds();

        return mesh;
    }
}
