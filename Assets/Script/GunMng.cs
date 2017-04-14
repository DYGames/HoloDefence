using UnityEngine;
using System.Collections;

public class GunMng : MonoBehaviour
{
    [HideInInspector]
    public bool _isFocusOnUnit;
    public bool _isGunDrop;
    public bool _shootAble;

    float _followSpeed;
    float _shootDmg;

    Vector3 _gunPosition;
    Quaternion _gunRotation;

    Unit _targetUnit;

    public Animation _muzzleFlash;
    public Animation _turretMenu;
    Animator _gunAnimation;
    BoxCollider _gunCollider;
    Rigidbody _rigidBody;

    void Start()
    {
        _followSpeed = 17;
        _gunPosition = transform.position;
        _gunRotation = transform.rotation;
        _isGunDrop = false;
        _isFocusOnUnit = false;
        _targetUnit = null;
        _shootDmg = 5;
        _shootAble = true;
        _rigidBody = GetComponent<Rigidbody>();
        _gunCollider = GetComponent<BoxCollider>();
        _gunAnimation = GetComponent<Animator>();
    }

    void Update()
    {
        if (!_isGunDrop)
            transform.parent.parent.rotation = Quaternion.Lerp(transform.parent.parent.rotation, Camera.main.transform.rotation, _followSpeed * Time.deltaTime);

        if (_shootAble && _isFocusOnUnit && !_isGunDrop)
            ShootGun();
    }

    void LateUpdate()
    {
        if (!_isGunDrop)
            transform.parent.parent.position = Camera.main.transform.position;
    }

    public void setShootAbleTrue()
    {
        _shootAble = true;
    }

    public void MuzzleFlash()
    {
        _muzzleFlash.Play("MuzzleFlash");
    }

    public void SetFocus(bool focus, Unit unit)
    {
        _isFocusOnUnit = focus;
        _targetUnit = unit;
    }

    void ShootGun()
    {
        if (_targetUnit.HP <= 0)
            return;
        _shootAble = false;
        _gunAnimation.SetTrigger("Shoot");
        Invoke("setShootAbleTrue", 1);
        _targetUnit.GetComponent<Unit>().Hit(_shootDmg, Camera.main.transform);
    }

    public void DropGun()
    {
        if (_isGunDrop)
            return;
        _isGunDrop = true;
        gameObject.layer = 0;
        transform.parent = null;
        _rigidBody.constraints = RigidbodyConstraints.None;
        _rigidBody.AddForce((Camera.main.transform.forward + Camera.main.transform.up) * 75);
        OpenTurretBuyMenu();
        _gunCollider.enabled = true;
    }
    public void PickGun()
    {
        CloseTurretBuyMenu();
        FindObjectOfType<Gaze>().setCrossHairMode();
        _isGunDrop = false;
        gameObject.layer = 2;
        _rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        transform.parent = Camera.main.transform;

        var p = Camera.main.transform.position;
        var r = Camera.main.transform.rotation;
        Camera.main.transform.position = new Vector3(0, 0, 0);
        Camera.main.transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.rotation = _gunRotation;
        transform.position = _gunPosition;

        Camera.main.transform.position = p;
        Camera.main.transform.rotation = r;
        _gunCollider.enabled = false;
    }

    public void OpenTurretBuyMenu()
    {
        _turretMenu.Play("Open");
    }

    public void CloseTurretBuyMenu()
    {
        _turretMenu.Play("Close");
    }
}