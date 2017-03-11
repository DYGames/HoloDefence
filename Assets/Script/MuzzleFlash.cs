using UnityEngine;
using System.Collections;

public class MuzzleFlash : MonoBehaviour
{
    public void RandomRotation()
    {
        transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
    }
}
