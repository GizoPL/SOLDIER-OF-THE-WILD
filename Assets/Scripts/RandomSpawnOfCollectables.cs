using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnOfCollectables : MonoBehaviour
{
    [SerializeField] private float xStart, xEnd, y;
    [SerializeField] private GameObject heart, ammo, fuel;
    private int whatToSpawn;
    public bool startSpawning = false;


    public bool StartSpawning
    {
        get { return startSpawning; }
        set { startSpawning = value; }
    }
    void Update()
    {

        if (startSpawning)
        {
            StartCoroutine("DoCheck");
        }

    }

    IEnumerator DoCheck()
    {
        for (; ; )
        {
            spawnCollectible();
            yield return new WaitForSeconds(9f);
        }
    }


    private void spawnCollectible()
    {
        var clone = new GameObject();
        startSpawning = false;
        whatToSpawn = Random.Range(1, 4);
        switch (whatToSpawn)
        {
            case 1:

                clone = Instantiate(heart, new Vector3(Random.Range(xStart, xEnd), y), transform.rotation);

                break;
            case 2:

                clone = Instantiate(ammo, new Vector3(Random.Range(xStart, xEnd), y), transform.rotation);
                break;
            case 3:

                clone = Instantiate(fuel, new Vector3(Random.Range(xStart, xEnd), y), transform.rotation);
                break;
        }
        Destroy(clone, 7f);

    }
}
