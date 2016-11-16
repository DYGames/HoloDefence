using UnityEngine;
using System.Collections;

public class Gaze : MonoBehaviour
{

    MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    void Update()
    {
        Vector3 headposition = Camera.main.transform.position;
        Vector3 gazeDirection = Camera.main.transform.forward;

        RaycastHit hitinfo;
        if (Physics.Raycast(headposition, gazeDirection, out hitinfo))
        {
            transform.position = hitinfo.point;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, hitinfo.normal);
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }

    }
}
