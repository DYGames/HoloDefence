using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA;

public class CreateSpawner : MonoBehaviour
{
    List<GameObject> Surfaces;
    void Start()
    {
        Surfaces = new List<GameObject>();
        StartCoroutine(FindSurface());
    }

    IEnumerator FindSurface()
    {
        yield return null;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (!Surfaces.Contains(transform.GetChild(i).gameObject))
            {
                Surfaces.Add(transform.GetChild(i).gameObject);
            }
        }

        StartCoroutine(FindSurface());
    }
}