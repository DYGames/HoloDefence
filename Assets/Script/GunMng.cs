using UnityEngine;
using System.Collections;

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

    void Start()
    {
        gunposition = transform.localPosition;
        gunrotation = transform.rotation;
        isFocusOnMonster = false;
        targetMonster = null;
        shootdelay = 0;
        shootdmg = 1;
        muzzleflashmesh = muzzleflash.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (isFocusOnMonster)
        {
            //Display Gage
            shootdelay += Time.deltaTime;
            if (shootdelay > 0.2f)
            {
                shootdelay = 0;
                ShootGun();
            }
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
}