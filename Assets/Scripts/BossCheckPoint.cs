using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCheckPoint : MonoBehaviour
{
    [SerializeField] GameObject frogBoss;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            frogBoss.GetComponent<RandomSpawnOfCollectables>().StartSpawning = true;
            frogBoss.GetComponent<FrogBoss>().StartFight = true;
        }
    }
}
