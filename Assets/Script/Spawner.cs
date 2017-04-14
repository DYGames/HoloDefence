using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] Monsters;
    public void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(5);

        if (FindObjectsOfType<Monster>().Length < 20)
        {
            GameObject monster = GameObject.Instantiate(Monsters[Random.Range(0, Monsters.Length)]);
            monster.transform.position = transform.position;
            Monster monstercomponent = monster.GetComponent<Monster>();
            monstercomponent.Target = Camera.main.transform;
        }

        StartCoroutine(SpawnRoutine());
    }
}
