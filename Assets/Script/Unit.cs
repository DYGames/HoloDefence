using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    [SerializeField]
    float _hp;
    public float HP
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
        }
    }

    public delegate void HitCallback(float dmg, Transform hitter);
    public HitCallback hitCallbacks;
    public delegate void DieCallback(float dmg, Transform hitter);
    public DieCallback dieCallbacks;

    public void Hit(float dmg, Transform hitter)
    {
        hitCallbacks.Invoke(dmg, hitter);
        HP -= dmg;
        if(HP <= 0)
        {
            HP = 0;
            dieCallbacks.Invoke(dmg, hitter);
        }
    }

}