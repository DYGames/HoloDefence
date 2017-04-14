using UnityEngine;
using System.Collections;
using UnityEngine.VR.WSA.Input;
using System;

public class Gaze : MonoBehaviour
{
    string _mode;

    MeshRenderer _meshRenderer;

    public GunMng _gun;

    public GameObject _crossHair;
    public GameObject _turret;
    public GameObject _turretPrefab;

    MeshRenderer _range;

    public GameObject _focusedObject { get; private set; }
    GestureRecognizer _recognizer;


    void Start()
    {
        _meshRenderer = _crossHair.GetComponent<MeshRenderer>();
        _recognizer = new GestureRecognizer();
        _recognizer.TappedEvent += OnSelect;
        _recognizer.StartCapturingGestures();
        _mode = "CrossHair";
        _range = transform.Find("Gaze").Find("Range").GetComponent<MeshRenderer>();
    }

    void Update()
    {
        GameObject oldFocusObject = _focusedObject;

        Vector3 headposition = Camera.main.transform.position;
        Vector3 gazeDirection = Camera.main.transform.forward;

        RaycastHit hitinfo;
        if (Physics.Raycast(headposition, gazeDirection, out hitinfo))
        {
            transform.position = hitinfo.point;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, hitinfo.normal);
            _meshRenderer.enabled = true;
            if (_mode == "Turret")
                _range.enabled = true;
            _focusedObject = hitinfo.collider.gameObject;

            OnFocus(hitinfo, hitinfo.collider.gameObject.tag);
        }
        else
        {
            _meshRenderer.enabled = false;
            _range.enabled = false;
            _focusedObject = null;
        }

        if (_focusedObject != oldFocusObject)
        {
            OffFocus();
            _recognizer.CancelGestures();
            _recognizer.StartCapturingGestures();
        }
    }

    public void OnFocus(RaycastHit hitinfo, string tag)
    {
        if (tag == "Monster")
        {
            if (!_gun._isFocusOnUnit)
                _gun.SetFocus(true, hitinfo.collider.gameObject.GetComponent<Unit>());
        }
        else if (tag == "Gun")
        {
            _gun.PickGun();
        }
    }

    public void OffFocus()
    {
        if (_gun._isFocusOnUnit)
        {
            _gun.SetFocus(false, null);
        }
    }

    public void setTurretMode()
    {
        _meshRenderer.enabled = false;
        _meshRenderer = _turret.GetComponent<MeshRenderer>();
        _mode = "Turret";
        _range.enabled = true;
        _range.transform.localScale = new Vector3(20, 20, 20);
    }

    public void setCrossHairMode()
    {
        _meshRenderer.enabled = false;
        _meshRenderer = _crossHair.GetComponent<MeshRenderer>();
        _mode = "CrossHair";
    }

    public void OnSelect(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        if (_focusedObject == null)
            return;

        if (_mode == "Turret")
        {
            GameObject o = Instantiate(_turretPrefab);
            o.transform.position = transform.FindChild("Gaze").position;
            o.transform.rotation = transform.FindChild("Gaze").rotation;
            o.GetComponent<Turret>().InitTurret(10, 2, 1);
            _gun.OpenTurretBuyMenu();
            setCrossHairMode();
            _range.enabled = false;
        }

        _focusedObject.SendMessageUpwards("OnSelect", SendMessageOptions.DontRequireReceiver);
    }
}
