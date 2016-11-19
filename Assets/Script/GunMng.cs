using UnityEngine;
using System.Collections;

//홀로렌즈에서 총주울시 포지션 또 이상함

public class GunMng : MonoBehaviour
{
    [HideInInspector]
    public bool isFocusOnMonster;
    Monster targetMonster;

    Vector3 gunposition;
    Quaternion gunrotation;

    float shootdelay;
    float shootdmg;

    public Animation muzzleflash;
    MeshRenderer muzzleflashmesh;
    public Light ptlight;

    Rigidbody rigidBody;

    public bool isGunDrop;

    public Animation TurretMenu;

    void Start()
    {
        gunposition = transform.position;
        gunrotation = transform.rotation;
        isGunDrop = false;
        isFocusOnMonster = false;
        targetMonster = null;
        shootdelay = 0;
        shootdmg = 1;
        muzzleflashmesh = muzzleflash.GetComponent<MeshRenderer>();
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isFocusOnMonster && !isGunDrop)
        {
            //Display Gage
            shootdelay += Time.deltaTime;
            if (shootdelay > 0.2f)
            {
                shootdelay = 0;
                ShootGun();
            }
        }
        if (Input.GetKey(KeyCode.G))
        {
            if (!isGunDrop)
                DropGun();
        }
    }

    public void SetFocus(bool focus, Monster monster)
    {
        shootdelay = 0;
        isFocusOnMonster = focus;
        targetMonster = monster;
    }

    void ShootGun()
    {
        StartCoroutine(ShootEffecOnce());
        targetMonster.Hit(shootdmg);
    }

    IEnumerator ShootEffecOnce()
    {
        muzzleflash.Play("MuzzleFlash");
        yield return null;
    }

    public void DropGun()
    {
        isGunDrop = true;
        gameObject.layer = 0;
        transform.parent = null;
        rigidBody.constraints = RigidbodyConstraints.None;
        rigidBody.AddForce((Camera.main.transform.forward + Camera.main.transform.up) * 75);
        OpenTurretBuyMenu();
    }

    void OpenTurretBuyMenu()
    {
        TurretMenu.Play("Open");
    }

    public void CloseTurretBuyMenu()
    {
        TurretMenu.Play("Close");
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
    }
}