using UnityEngine;
using System.Collections;

public class GunMng : MonoBehaviour
{
    [HideInInspector]
    public bool isFocusOnMonster;
    Monster targetMonster;

    Vector3 gunposition;
    Quaternion gunrotation;

    float shootdmg;

    public Animation muzzleflash;
    public Animation TurretMenu;
    Animator gunAnimation;
    BoxCollider gunCollider;
    Rigidbody rigidBody;

    MeshRenderer muzzleflashmesh;

    public bool isGunDrop;

    public bool ShootAble;

    float followSpeed;

    void Start()
    {
        followSpeed = 17;
        gunposition = transform.position;
        gunrotation = transform.rotation;
        isGunDrop = false;
        isFocusOnMonster = false;
        targetMonster = null;
        shootdmg = 1;
        ShootAble = true;
        muzzleflashmesh = muzzleflash.GetComponent<MeshRenderer>();
        rigidBody = GetComponent<Rigidbody>();
        gunCollider = GetComponent<BoxCollider>();
        gunAnimation = GetComponent<Animator>();
    }

    void Update()
    {
        transform.parent.parent.rotation = Quaternion.Lerp(transform.parent.parent.rotation, Camera.main.transform.rotation, followSpeed * Time.deltaTime);

        if (ShootAble && isFocusOnMonster && !isGunDrop)
            ShootGun();
    }

    void LateUpdate()
    {
        transform.parent.parent.position = Camera.main.transform.position;
    }

    public void setShootAbleTrue()
    {
        ShootAble = true;
    }

    public void MuzzleFlash()
    {
        muzzleflash.Play("MuzzleFlash");
    }

    public void SetFocus(bool focus, Monster monster)
    {
        isFocusOnMonster = focus;
        targetMonster = monster;
    }

    void ShootGun()
    {
        ShootAble = false;
        gunAnimation.SetTrigger("Shoot");
        targetMonster.Hit(shootdmg, Camera.main.transform);
    }

    public void DropGun()
    {
        if (isGunDrop)
            return;
        isGunDrop = true;
        gameObject.layer = 0;
        transform.parent = null;
        rigidBody.constraints = RigidbodyConstraints.None;
        rigidBody.AddForce((Camera.main.transform.forward + Camera.main.transform.up) * 75);
        OpenTurretBuyMenu();
        gunCollider.enabled = true;
    }
    public void PickGun()
    {
        CloseTurretBuyMenu();
        FindObjectOfType<Gaze>().setCrossHairMode();
        isGunDrop = false;
        gameObject.layer = 2;
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        transform.parent = Camera.main.transform;

        var p = Camera.main.transform.position;
        var r = Camera.main.transform.rotation;
        Camera.main.transform.position = new Vector3(0, 0, 0);
        Camera.main.transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.rotation = gunrotation;
        transform.position = gunposition;

        Camera.main.transform.position = p;
        Camera.main.transform.rotation = r;
        gunCollider.enabled = false;
    }

    public void OpenTurretBuyMenu()
    {
        TurretMenu.Play("Open");
    }

    public void CloseTurretBuyMenu()
    {
        TurretMenu.Play("Close");
    }
}