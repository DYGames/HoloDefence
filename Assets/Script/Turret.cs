using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float Range;
    public float AttackSpeed;
    public int AttackDmg;

    public void InitTurret(float range, float attackspeed, int attackdmg)
    {
        Range = range;
        AttackSpeed = attackspeed;
        AttackDmg = attackdmg;
    }

    void Start()
    {
        StartCoroutine(FindEnemy());
    }

    IEnumerator FindEnemy()
    {
        yield return new WaitForSeconds(AttackSpeed);
        Collider[] enemys = Physics.OverlapSphere(transform.position, Range);

        for (int i = 0; i < enemys.Length; i++)
        {
            if (enemys[i].tag == "Monster")
            {
                Shoot(enemys[i].GetComponent<Monster>());
            }
        }
        StartCoroutine(FindEnemy());
    }

    void Shoot(Monster monster)
    {
        ShootEffect();
        monster.Hit(AttackDmg, transform);
    }

    void ShootEffect()
    {

    }
}
