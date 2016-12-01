using UnityEngine;
using System.Collections;
using UnityEngine.VR.WSA.Input;
using System;

public class Gaze : MonoBehaviour
{
    string mode;

    MeshRenderer meshRenderer;

    public GunMng Gun;

    public GameObject CrossHair;
    public GameObject Turret;
    public GameObject TurretPrefab;

    public GameObject FocusedObject { get; private set; }
    GestureRecognizer recognizer;


    void Start()
    {
        meshRenderer = CrossHair.GetComponent<MeshRenderer>();
        recognizer = new GestureRecognizer();
        recognizer.TappedEvent += (source, tapCount, ray) =>
        {
            OnSelect();
            FocusedObject.SendMessageUpwards("OnSelect");
        };
        recognizer.StartCapturingGestures();
    }

    void Update()
    {
        GameObject oldFocusObject = FocusedObject;

        Vector3 headposition = Camera.main.transform.position;
        Vector3 gazeDirection = Camera.main.transform.forward;

        RaycastHit hitinfo;
        if (Physics.Raycast(headposition, gazeDirection, out hitinfo))
        {
            transform.position = hitinfo.point;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, hitinfo.normal);
            meshRenderer.enabled = true;
            FocusedObject = hitinfo.collider.gameObject;

            OnFocus(hitinfo, hitinfo.collider.gameObject.tag);
        }
        else
        {
            meshRenderer.enabled = false;
            FocusedObject = null;

            OffFocus();
        }

        if (FocusedObject != oldFocusObject)
        {
            recognizer.CancelGestures();
            recognizer.StartCapturingGestures();
        }
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            OnSelect();
            if (FocusedObject != null)
            {
                FocusedObject.SendMessageUpwards("OnSelect", SendMessageOptions.DontRequireReceiver);
            }
        }
#endif
    }

    public void OnFocus(RaycastHit hitinfo, string tag)
    {
        if (tag == "Monster")
        {
            if (!Gun.isFocusOnMonster)
                Gun.SetFocus(true, hitinfo.collider.gameObject.GetComponent<Monster>());
        }
        else if (tag == "Gun")
        {
            Gun.PickGun();
        }
    }

    public void OffFocus()
    {
        if (Gun.isFocusOnMonster)
        {
            Gun.SetFocus(false, null);
        }
    }

    public void setTurretMode()
    {
        meshRenderer.enabled = false;
        meshRenderer = Turret.GetComponent<MeshRenderer>();
        mode = "Turret";
    }

    public void setCrossHairMode()
    {
        meshRenderer.enabled = false;
        meshRenderer = CrossHair.GetComponent<MeshRenderer>();
        mode = "CrossHair";
    }

    void OnSelect()
    {
        if (FocusedObject == null)
            return;

        if (mode == "Turret")
        {
            GameObject o = Instantiate(TurretPrefab);
            o.transform.position = transform.FindChild("Gaze").position;
            o.transform.rotation = transform.FindChild("Gaze").rotation;
            o.transform.parent = FocusedObject.transform;
            Gun.OpenTurretBuyMenu();
            setCrossHairMode();
        }
    }
}
