using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Bullet : MonoBehaviour
{
   
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject enemyExplosion;
    [SerializeField] AudioSource missionCompleteSound;
    private int health;
    

    private void Update()
    {
        Destroy(gameObject, 1f);
    }


    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.tag != "Health" && hitInfo.gameObject.tag != "Ammo" && hitInfo.gameObject.tag != "Bullet" && hitInfo.gameObject.tag != "CheckPoint" && hitInfo.gameObject.tag != "Fuel" && hitInfo.gameObject.tag != "BossCheckPoint" && hitInfo.gameObject.tag != "LevelPassedCheckpoint")
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
           
        
    
            if (hitInfo.gameObject.tag == "Enemy")
            {
                if (hitInfo.gameObject.GetComponent<FrogBoss>())
                {
                    health = hitInfo.gameObject.GetComponent<FrogBoss>().HP;

                    if (health == 1)
                    {
                        Instantiate(explosion, transform.position, transform.rotation);
                        Destroy(hitInfo.gameObject);
                        Instantiate(enemyExplosion, transform.position, transform.rotation);                  
                    }
                    else
                    {
                        health -= 1;
                        hitInfo.gameObject.GetComponent<FrogBoss>().HP = health;
                        Instantiate(explosion, transform.position, transform.rotation);
                    }
                }
                else
                {
                    health = hitInfo.gameObject.GetComponent<AI>().HP;

                    if (health == 1)
                    {
                        Instantiate(explosion, transform.position, transform.rotation);
                        Destroy(hitInfo.gameObject);
                        Instantiate(enemyExplosion, transform.position, transform.rotation);
                    }
                    else
                    {
                        //poprawialem
                        health -= 1;
                        hitInfo.gameObject.GetComponent<AI>().HP = health;
                        Instantiate(explosion, transform.position, transform.rotation);
                    }
                }
            }
        }
    }
}


