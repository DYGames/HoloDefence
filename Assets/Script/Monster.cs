using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
    float HP;
    float movespeed;
    float attackpower;

    Rigidbody rigidBody;

    void Start()
    {
        HP = 15;
        movespeed = 1;
        attackpower = 1;
        rigidBody = GetComponent<Rigidbody>();
    }
    
    public void Hit(float dmg)
    {
        HP -= dmg;
        rigidBody.AddForce(Camera.main.transform.forward * 50);
        if (HP <= 0)
        {
            gameObject.layer = 2;
            rigidBody.constraints = RigidbodyConstraints.None;
            Invoke("DestroyMonster", 2);
        }

    }

    void DestroyMonster()
    {
        Destroy(gameObject);
    }
}
