using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainMeshGenerator : MonoBehaviour
{
    [SerializeField] private GameObject terrainPrefab;
    [SerializeField] private Material grassMaterial;


    private void Start()
    {
        GenerateTerrainMesh();
    }

    private void GenerateTerrainMesh()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        Mesh terrainMesh = GetScaledMesh(terrainPrefab);

        if (terrainMesh != null)
        {
            Mesh newMesh = new Mesh();
            newMesh.vertices = terrainMesh.vertices;
            newMesh.triangles = terrainMesh.triangles;
            newMesh.normals = terrainMesh.normals;
            newMesh.uv = terrainMesh.uv;

            meshFilter.sharedMesh = newMesh;

            if (grassMaterial != null) 
            {
                meshRenderer.sharedMaterial = grassMaterial;
            }

            transform.position = terrainPrefab.transform.position;
            transform.rotation = terrainPrefab.transform.rotation;
        }
    }


    private Mesh GetScaledMesh(GameObject prefab)
    {
        MeshFilter meshFilter = prefab.GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            Mesh mesh = Instantiate(meshFilter.sharedMesh);
            Vector3 scale = prefab.transform.localScale;
            Vector3[] vertices = mesh.vertices;

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = Vector3.Scale(vertices[i], scale);
            }

            mesh.vertices = vertices;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            return mesh;
        }

        return null;
    }

}
