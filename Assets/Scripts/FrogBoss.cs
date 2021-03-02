using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBoss : MonoBehaviour
{
    [SerializeField] private float leftBorder;
    [SerializeField] private float rightBorder;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private LayerMask ground;
    [SerializeField] public int hP;
    [SerializeField] public float startTimeBtwShots;
    [SerializeField] public GameObject bossBullet, BossCheckPoint;
    [SerializeField] private AudioSource BossBulletSound;
    private float timeBtwShots;
    public bool startFight = false;
    private float originalSpeed;
    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D coll;
    private bool facingLeft = true;
    

    
    public bool StartFight
    {
        get { return startFight; }
        set { startFight = value; }
    }
    public int HP
    {
        get { return hP; }
        set { hP = value; }
    }
    public float OriginalSpeed
    {
        get { return originalSpeed; }
        set { originalSpeed = value; }
    }
    public float MovementSpeed
    {
        get { return movementSpeed; }
        set { movementSpeed = value; }
    }
    private void BlaBlaBla()
    {
        movementSpeed = originalSpeed;
    }

    private void Update()
    {
        if(anim.GetBool("Jumping"))
        {
            if(rb.velocity.y < 0.1)
            {
                anim.SetBool("Falling", true);
                anim.SetBool("Jumping", false);
            }
        }

        if(coll.IsTouchingLayers(ground) && anim.GetBool("Falling"))
        {
            anim.SetBool("Falling", false);
            anim.SetBool("Jumping", false);
        }

        if (startFight)
        {
            
            if (timeBtwShots <= 0)
            {
                                             
                BossBulletSound.Play();
                Instantiate(bossBullet, transform.position, Quaternion.identity);                                  
                timeBtwShots = startTimeBtwShots;
            }
            else
            {
                timeBtwShots -= Time.deltaTime;
            }
        }
    }

    public void Move()
    {
        if (movementSpeed == 0f)
        {
            Invoke("BlaBlaBla", 2.2f);
        }

        if (facingLeft)
        {
            if (transform.position.x > leftBorder)
            {
                if (transform.localScale.x != -1.924955f)
                {
                    transform.localScale = new Vector3(-1.924955f, 1.924955f);
                }

                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(-movementSpeed, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightBorder)
            {
                if (transform.localScale.x != 1.924955f)
                {
                    transform.localScale = new Vector3(1.924955f, 1.924955f);
                }

                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(movementSpeed, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = true;
            }
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        timeBtwShots = startTimeBtwShots;
    }
}
