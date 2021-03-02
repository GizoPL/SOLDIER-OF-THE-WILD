using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Transform bar;
    private int hp;
    [SerializeField] private GameObject enemy;
   
    void Start()
    {
        bar = transform.Find("Bar");
        hp = enemy.GetComponent<AI>().HP;
    }

    // Update is called once per frame
    void Update()
    {
        bar.localScale = new Vector3(enemy.GetComponent<AI>().HP/(float)hp, 1f);
    }
}
