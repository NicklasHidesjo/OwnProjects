using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;

public class MeshCombiner : MonoBehaviour
{
    public void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.indexFormat = IndexFormat.UInt32;

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            if (meshFilters[i].sharedMesh == null) continue;
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            Destroy(meshFilters[i].gameObject);
        }

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        GetComponent<MeshCollider>().sharedMesh = mesh;

        foreach (Transform trans in transform)
        {
            Destroy(trans.gameObject);
        }

        Destroy(this);
    }
}
