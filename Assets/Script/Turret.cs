using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float _range;
    public float _attackSpeed;
    public int _attackDmg;

    public void InitTurret(float range, float attackspeed, int attackdmg)
    {
        _range = range;
        _attackSpeed = attackspeed;
        _attackDmg = attackdmg;
    }

    void Start()
    {
        StartCoroutine(FindHitableUnit());
    }

    IEnumerator FindHitableUnit()
    {
        yield return new WaitForSeconds(_attackSpeed);
        Collider[] units = Physics.OverlapSphere(transform.position, _range);

        for (int i = 0; i < units.Length; i++)
        {
            if (units[i].tag == "Monster")
            {
                Shoot(units[i].GetComponent<Unit>());
                break;
            }
        }
        StartCoroutine(FindHitableUnit());
    }

    void Shoot(Unit unit)
    {
        ShootEffect();
        unit.Hit(_attackDmg, transform);
    }

    void ShootEffect()
    {

    }
}
