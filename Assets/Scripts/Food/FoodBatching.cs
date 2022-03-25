using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBatching : MonoBehaviour
{
    List<MeshFilter> meshfilter = new List<MeshFilter>();
    void Update()
    {
        CombineMeshes();
    }

    void CombineMeshes()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        for (int i = 1; i < meshFilters.Length; i++)
            meshfilter.Add(meshFilters[i]);

        CombineInstance[] combine = new CombineInstance[meshfilter.Count];
        int j = 0;
        while(j < meshfilter.Count)
        {
            combine[j].mesh = meshfilter[j].sharedMesh;;
            combine[j].transform = meshfilter[j].transform.localToWorldMatrix;
            meshfilter[j].gameObject.SetActive(false);
            //meshfilter[j].GetComponent<MeshRenderer>().enabled = false;
            j++;
        }

        var meshFilter = transform.GetComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();
        meshFilter.mesh.CombineMeshes(combine);
        transform.gameObject.SetActive(true);

        transform.localScale = new Vector3(1, 1, 1);
        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;
    }
}
