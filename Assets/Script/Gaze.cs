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

    MeshRenderer Range;

    public GameObject FocusedObject { get; private set; }
    GestureRecognizer recognizer;


    void Start()
    {
        meshRenderer = CrossHair.GetComponent<MeshRenderer>();
        recognizer = new GestureRecognizer();
        recognizer.TappedEvent += (source, tapCount, ray) =>
        {
            OnSelect();
            if (FocusedObject != null)
                FocusedObject.SendMessageUpwards("OnSelect", SendMessageOptions.DontRequireReceiver);
        };
        recognizer.StartCapturingGestures();
        mode = "CrossHair";
        Range = transform.Find("Gaze").Find("Range").GetComponent<MeshRenderer>();
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
            if (mode == "Turret")
                Range.enabled = true;
            FocusedObject = hitinfo.collider.gameObject;

            OnFocus(hitinfo, hitinfo.collider.gameObject.tag);
        }
        else
        {
            meshRenderer.enabled = false;
            Range.enabled = false;
            FocusedObject = null;
        }

        if (FocusedObject != oldFocusObject)
        {
            OffFocus();
            recognizer.CancelGestures();
            recognizer.StartCapturingGestures();
        }
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
        Range.enabled = true;
        Range.transform.localScale = new Vector3(20, 20, 20);
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
            //o.transform.parent = FocusedObject.transform;
            o.GetComponent<Turret>().InitTurret(10, 2, 1);
            Gun.OpenTurretBuyMenu();
            setCrossHairMode();
            Range.enabled = false;
        }
    }
}
