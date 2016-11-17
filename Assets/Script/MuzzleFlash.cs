using UnityEngine;
using System.Collections;

public class MuzzleFlash : MonoBehaviour
{

    public void RandomRotation()
    {
        Transform parent = transform.parent;
        transform.parent = null;
        transform.localScale = Vector3.one;
        //transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        transform.parent = parent;
    }
}
