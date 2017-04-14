using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA;
using HoloToolkit.Unity.SpatialMapping;
using System;

public class CreateSpawner : MonoBehaviour
{
    public Material defaultMaterial;

    public bool limitScanningByTime = true;

    public float scanTime = 30.0f;

    public Material secondaryMaterial;

    public uint minimumFloors = 1;

    public uint minimumWalls = 1;

    private bool meshesProcessed = false;

    public GameObject spawnerPrefab;
     
    void Start()
    {
        SpatialMappingManager.Instance.SetSurfaceMaterial(defaultMaterial);

        SurfaceMeshesToPlanes.Instance.MakePlanesComplete += SurfaceMeshesToPlanes_MakePlanesComplete;
    }

    void Update()
    {
        if (!meshesProcessed && limitScanningByTime)
        {
            if (limitScanningByTime && ((Time.time - SpatialMappingManager.Instance.StartTime) < scanTime))
            {
            }
            else
            {
                if (SpatialMappingManager.Instance.IsObserverRunning())
                {
                    SpatialMappingManager.Instance.StopObserver();
                }

                CreatePlanes();

                meshesProcessed = true;
            }
        }
    }

    private void SurfaceMeshesToPlanes_MakePlanesComplete(object source, System.EventArgs args)
    {
        List<GameObject> horizontal = new List<GameObject>();

        List<GameObject> vertical = new List<GameObject>();

        horizontal = SurfaceMeshesToPlanes.Instance.GetActivePlanes(PlaneTypes.Table | PlaneTypes.Floor);

        vertical = SurfaceMeshesToPlanes.Instance.GetActivePlanes(PlaneTypes.Wall);

        if (horizontal.Count >= minimumFloors && vertical.Count >= minimumWalls)
        {
            RemoveVertices(SurfaceMeshesToPlanes.Instance.ActivePlanes);

            //SpatialMappingManager.Instance.SetSurfaceMaterial(secondaryMaterial);

            List<GameObject> spawners = new List<GameObject>();

            for (int i = 0; i < SurfaceMeshesToPlanes.Instance.ActivePlanes.Count; i++)
            {
                GameObject plane = SurfaceMeshesToPlanes.Instance.ActivePlanes[i];

                spawners.Add(Instantiate(spawnerPrefab));
                spawners[i].transform.position = plane.transform.position;
                //spawners.Count - 1 > 0 ? spawners.Count - 1 : 0
                plane.GetComponent<MeshRenderer>().enabled = false;
            }

            //StartGame

        }
        else
        {
            SpatialMappingManager.Instance.StartObserver();

            meshesProcessed = false;
        }
    }

    private void CreatePlanes()
    {
        SurfaceMeshesToPlanes surfaceToPlanes = SurfaceMeshesToPlanes.Instance;
        if (surfaceToPlanes != null && surfaceToPlanes.enabled)
        {
            surfaceToPlanes.MakePlanes();
        }
    }

    private void RemoveVertices(IEnumerable<GameObject> boundingObjects)
    {
        RemoveSurfaceVertices removeVerts = RemoveSurfaceVertices.Instance;
        if (removeVerts != null && removeVerts.enabled)
        {
            removeVerts.RemoveSurfaceVerticesWithinBounds(boundingObjects);
        }
    }

    private void OnDestroy()
    {
        if (SurfaceMeshesToPlanes.Instance != null)
        {
            SurfaceMeshesToPlanes.Instance.MakePlanesComplete -= SurfaceMeshesToPlanes_MakePlanesComplete;
        }
    }

}