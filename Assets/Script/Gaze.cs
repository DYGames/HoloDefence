using UnityEngine;
using System.Collections;

public class Gaze : MonoBehaviour
{

    MeshRenderer meshRenderer;
    public GunMng Gun;

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
            //meshRenderer.transform.position += new Vector3(0.01f, 0, 0);
            transform.rotation = Quaternion.FromToRotation(Vector3.up, hitinfo.normal);
            meshRenderer.enabled = true;

            if (hitinfo.collider.gameObject.CompareTag("Monster"))
            {
                if (!Gun.isFocusOnMonster)
                    Gun.SetFocus(true, hitinfo.collider.gameObject.GetComponent<Monster>());
            }
        }
        else
        {
            meshRenderer.enabled = false;
            if (Gun.isFocusOnMonster)
            {
                Gun.SetFocus(false, null);
            }
        }

    }
}
