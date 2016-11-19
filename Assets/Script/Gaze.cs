using UnityEngine;
using System.Collections;

public class Gaze : MonoBehaviour
{

    MeshRenderer meshRenderer;
    public GunMng Gun;

    public GameObject CrossHair;
    public GameObject Turret;

    void Start()
    {
        meshRenderer = CrossHair.GetComponent<MeshRenderer>();
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

            if (hitinfo.collider.gameObject.CompareTag("Monster"))
            {
                if (!Gun.isFocusOnMonster)
                    Gun.SetFocus(true, hitinfo.collider.gameObject.GetComponent<Monster>());
            }
            if (hitinfo.collider.gameObject.CompareTag("Gun"))
            {
                //if (Input.GetKeyDown(KeyCode.G))
                    Gun.PickGun();
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

    public void setTurretMode()
    {
        meshRenderer.enabled = false;
        meshRenderer = Turret.GetComponent<MeshRenderer>();
    }

    public void setCrossHairMode()
    {
        meshRenderer.enabled = false;
        meshRenderer = CrossHair.GetComponent<MeshRenderer>();
    }
}
