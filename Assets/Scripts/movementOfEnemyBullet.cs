using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementOfEnemyBullet : MonoBehaviour
{
   [SerializeField] private GameObject explosion;
    private int playerHp;
    public float speed;
    private Transform player;
    private Vector2 target;

   
    private void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = new Vector2(player.position.x, player.position.y);
    }

    private void Update()
    {
        playerHp = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().PlayerHealth;

        if (playerHp <= 0)
        {
           DestroyEnemyBullet();
        }
        else 
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

            if (transform.position.x == target.x && transform.position.y == target.y)
            {
                DestroyEnemyBullet();
            }

        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DestroyEnemyBullet();
        }

        if (collision.gameObject.tag != "Player"  && collision.gameObject.tag != "CheckPoint"  && collision.gameObject.tag != "BossCheckPoint" && collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "FallingChecker" && collision.gameObject.tag != "DontFlyToHigh" && collision.gameObject.tag != "Level" && collision.gameObject.tag != "LevelPassedCheckpoint")
        {
            
            Instantiate(explosion, transform.position, transform.rotation);
            DestroyEnemyBullet();
            

            if (collision.gameObject.tag == "Ammo")
            {
                Destroy(collision.gameObject);
            }
            else if(collision.gameObject.tag == "Fuel")
            {
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.tag == "Health")
            {
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.tag == "Bullet")
            {
                Destroy(collision.gameObject);
            }

        }
    }

    private void DestroyEnemyBullet()
    {
        Destroy(gameObject);
    }


}
