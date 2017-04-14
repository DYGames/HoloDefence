using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
    Unit _unitBase;

    float _moveSpeed;
    float _attackPower;

    Rigidbody _rigidBody;

    Transform _target;
    public Transform Target
    {
        set
        {
            _target = value;
        }
    }

    void Start()
    {
        _unitBase = GetComponent<Unit>();
        _unitBase.HP = 15;
        _moveSpeed = 1;
        _attackPower = 1;
        _rigidBody = GetComponent<Rigidbody>();

        _unitBase.hitCallbacks += delegate (float dmg, Transform hitter)
        {
            transform.LookAt(hitter);
            transform.rotation *= Quaternion.Euler(0, 180f, 0);
            _rigidBody.AddForce(hitter.forward * 150);
            Invoke("ZeroVelocity", 0.25f);
        };

        _unitBase.dieCallbacks += delegate (float dmg, Transform hitter)
        {
            gameObject.layer = 2;
            _rigidBody.constraints = RigidbodyConstraints.None;
            _rigidBody.AddForce(hitter.forward * 150);
            _rigidBody.AddTorque(Vector3_Random(0, 1), ForceMode.Force);
            Invoke("DestroyMonster", 2);
        };
    }

    void Update()
    {
        if (_target != null)
            transform.Translate(-(transform.position - _target.position) * _moveSpeed * Time.deltaTime);
    }

    void ZeroVelocity()
    {
        _rigidBody.velocity = Vector3.zero;
    }

    void DestroyMonster()
    {
        Destroy(gameObject);
    }

    Vector3 Vector3_Random(float min, float max)
    {
        return new Vector3(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
    }
}
