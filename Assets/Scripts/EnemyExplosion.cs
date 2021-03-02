using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosion : MonoBehaviour
{

    void Update()
    {
        Destroy(gameObject, 1f);      
    }
}
