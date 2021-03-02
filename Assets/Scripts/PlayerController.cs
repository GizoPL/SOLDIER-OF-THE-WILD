using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class PlayerController : MonoBehaviour
{
    private Vector2 checkPointPos = new Vector2(10.09f, 11.48f);
    private Rigidbody2D rb;
    private Animator anim;
    private Weapon weapon;
    private float originallSpeed;
    
    private enum State { idle, running, jumping, falling, hurt, dead, fly }
    private State state = State.idle;
    private Collider2D coll;
    private int playerHealth = 100;
    private int playerLifes = 3;
    private int jetpackFuel = 2000;
    private bool FacingRight = true;
    private string playerSate;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Slider slider;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float flyForce = 3.5f;
    [SerializeField] private float hurtForce = 7f;
    [SerializeField] private float bossHurtForce = 10f;
    [SerializeField] private Text fuelText;
    [SerializeField] AudioSource ammoSound, healUpSound, deathSound, hurt1Sound, hurt2Sound, hurt3Sound, hurt4Sound, jump1Sound, jetPackSound, jetPackOffSound, jumpOnTheGroundSound, walkingSound, lowHpSound, checkPointSound, gameOverSound, fuelIsLowSounds, missionFailureSounds, missionCompleteSounds, synchronizeingSounds;
    [SerializeField] GameObject frogBoss;
    [SerializeField] GameObject player;
    public int PlayerHealth { get { return playerHealth; } set { playerHealth = value; } }
    public string PlayerState { get { return playerSate; } set { playerSate = value; } }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        weapon = GetComponent<Weapon>();
        slider.maxValue = playerHealth;
        slider.value = playerHealth;
        playerSate = state.ToString();
       
    }

    private void Update()
    {
     
        if ((state == State.dead || state == State.hurt) && Input.GetKey(KeyCode.W))
        {
            
            jetPackSound.Stop();
            jetPackOffSound.Stop();
        }

        playerSate = state.ToString();

        if (PauseMenu.GamePaused == false)
        {
            if (state != State.hurt && state != State.dead)
            {
                Movement();
            }

            VelocityState();
            anim.SetInteger("state", (int)state);

            hearts[0].enabled = true;
            hearts[1].enabled = true;
            hearts[2].enabled = true;
            if (playerLifes == 3)
            {
                hearts[0].sprite = fullHeart;
                hearts[1].sprite = fullHeart;
                hearts[2].sprite = fullHeart;
            }
            else if (playerLifes == 2)
            {
                hearts[0].sprite = fullHeart;
                hearts[1].sprite = fullHeart;
                hearts[2].sprite = emptyHeart;
            }
            else if (playerLifes == 1)
            {
                hearts[0].sprite = fullHeart;
                hearts[1].sprite = emptyHeart;
                hearts[2].sprite = emptyHeart;
            }
            else
            {
                hearts[0].sprite = emptyHeart;
                hearts[1].sprite = emptyHeart;
                hearts[2].sprite = emptyHeart;
            }

            if (jetpackFuel != 0)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    if (!jetPackSound.isPlaying)
                    {
                        jetPackSound.Play();
                    }
                }

                else if (Input.GetKeyUp(KeyCode.W) || jetpackFuel == 0)
                {

                    jetPackSound.Stop();
                    jetPackOffSound.Play();
                }

            }
            else
            {
                jetPackSound.Stop();
                jetPackOffSound.Play();
            }

            if (state == State.falling)
            {
                if (jetPackSound.isPlaying)
                {
                    jetPackSound.Stop();
                    jetPackOffSound.Play();

                }
            }
        }

        if (jetpackFuel == 2500)
        {
            fuelIsLowSounds.PlayOneShot(fuelIsLowSounds.clip, 0.7F); ;
        }

        if (!GameObject.Find("FrogBOSS"))
        {
            if (!missionCompleteSounds.isPlaying)
                missionCompleteSounds.Play();

            Invoke("LoadLevel2", 4f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collectable)
    {

        if (playerHealth != 100 && collectable.tag == "Health")
        {
            healUpSound.Play();
            Destroy(collectable.gameObject);
            playerHealth += 25;
            slider.value = playerHealth;
            if (playerHealth > 25)
            {
                lowHpSound.Stop();
            }
        }

        if (collectable.tag == "Ammo")
        {
            ammoSound.Play();
            Destroy(collectable.gameObject);
            weapon.Ammo += 15;
            weapon.AmmoText.text = "*" + weapon.Ammo.ToString();
        }

        if(collectable.tag == "Fuel" && jetpackFuel != 2000)
        {
            healUpSound.Play();
            Destroy(collectable.gameObject);

            if (jetpackFuel <= 1500)
                jetpackFuel += 500;
            else
                jetpackFuel = 2000;

            fuelText.text = jetpackFuel/20 + "%";
        }

        if (collectable.gameObject.tag == "CheckPoint")
        {
            checkPointSound.Play();
            checkPointPos = new Vector2(collectable.gameObject.transform.position.x, collectable.gameObject.transform.position.y);
            Destroy(collectable.gameObject);
        }

        if (collectable.gameObject.tag == "BossCheckPoint")
        {
            checkPointSound.Play();
            checkPointPos = new Vector2(collectable.gameObject.transform.position.x, collectable.gameObject.transform.position.y);
            Destroy(collectable.gameObject);
           // frogBoss.GetComponent<RandomSpawnOfCollectables>().StartSpawning = true;
           // frogBoss.GetComponent<FrogBoss>().StartFight = true;
        }


        if (collectable.gameObject.tag == "FallingChecker" && state != State.dead)
        {
            missionFailureSounds.Play();
            lowHpSound.Stop();
            rb.velocity = new Vector2(0f, 0f);
            rb.position = checkPointPos;
            if (state == State.dead)
            {
                playerLifes++;
            }
            playerLifes--;
            playerHealth = 100;
            slider.value = playerHealth;
            if (playerLifes <= 0)
            {
                SceneManager.LoadScene(4);
            }
            collectable.gameObject.GetComponent<PlayerController>().enabled = true;
        }

        if (collectable.gameObject.tag == "GravityHelper")
        {
            missionFailureSounds.Play();
            lowHpSound.Stop();
            if (state == State.dead)
            {
                playerLifes++;
            }
            playerLifes--;
            playerHealth = 100;
            slider.value = playerHealth;
            if (playerLifes <= 0)
            {
                SceneManager.LoadScene(4);
            }
            Invoke("GetBoxColliderBack", 1.5f);
        }

        if (collectable.tag == "BulletEnemy" && (state != State.hurt || state != State.dead))
        {
            if(state != State.hurt)
            {
                if (state != State.dead)
                {
                    playerHealth -= 15;
                    slider.value = playerHealth;
                    if (playerHealth <= 25)
                    {
                        if (!lowHpSound.isPlaying)
                        { lowHpSound.Play(); }

                    }
                    if (playerHealth > 25)
                    {
                        lowHpSound.Stop();
                    }
                    Random random = new Random();
                    int hurtSound = random.Next(0, 4);
                    switch (hurtSound)
                    {
                        case 0:
                            hurt1Sound.Play();
                            break;
                        case 1:
                            hurt2Sound.Play();
                            break;
                        case 2:
                            hurt3Sound.Play();
                            break;
                        case 3:
                            hurt4Sound.Play();
                            break;
                    }
                }

                if (playerHealth <= 0)
                {
                    missionFailureSounds.Play();
                    state = State.dead;

                    if (frogBoss.gameObject.GetComponent<FrogBoss>())
                    {
                        originallSpeed = frogBoss.GetComponent<FrogBoss>().MovementSpeed;
                        frogBoss.GetComponent<FrogBoss>().OriginalSpeed = originallSpeed;
                        frogBoss.GetComponent<FrogBoss>().MovementSpeed = 0f;
                        frogBoss.GetComponent<FrogBoss>().StartFight = false;
                        Invoke("BlaBlaBla2", 2.2f);
                    }

                    lowHpSound.Stop();
                    deathSound.Play();
                    playerHealth = 100;
                    playerLifes--;
                    Invoke("BlaBlaBla2", 2.2f);
                    if (playerLifes <= 0)
                    {
                        SceneManager.LoadScene(4);
                    }
                }
            }
        }

        if (collectable.tag == "GamePassedCheckpoint" && !GameObject.Find("FrogBOSS (1)"))
        {
            if (!missionCompleteSounds.isPlaying)
            {
                missionCompleteSounds.Play();
            } 

            Invoke("LoadFinalScene", 2f);
        }

        if (collectable.tag == "LevelPassedCheckpoint" && !GameObject.Find("FrogBOSS (0)") && !GameObject.Find("FrogBOSS (1)") && !GameObject.Find("FrogBOSS (2)"))
        {
            if (!missionCompleteSounds.isPlaying)
            {
                missionCompleteSounds.Play();
            }

            Invoke("LoadLevel3", 2f);
        }
    }

    private void LoadLevel3() 
    {
        SceneManager.LoadScene(3);
    }
    private void LoadLevel2() 
    {
        SceneManager.LoadScene(2);
    }
    private void LoadFinalScene() 
    {
        SceneManager.LoadScene(5);
    }

    private void OnCollisionEnter2D(Collision2D enemy)
    {
       
        if (enemy.gameObject.tag == "Enemy" && state != State.dead && state != State.hurt && playerHealth > 0)
        {
            
            playerHealth -= 50;
            if (playerHealth <= 25)
            {
                if (!lowHpSound.isPlaying)
                { lowHpSound.Play(); }
            }
            if (playerHealth > 25 )
            {
                lowHpSound.Stop();
            }
            slider.value = playerHealth;
            Random random = new Random();
            int hurtSound = random.Next(0, 4);

            switch (hurtSound)
            {
                case 0:
                    hurt1Sound.Play();
                    break;
                case 1:
                    hurt2Sound.Play();
                    break;
                case 2:
                    hurt3Sound.Play();
                    break;
                case 3:
                    hurt4Sound.Play();
                    break;
            }

 
            if (playerHealth <= 0)
            {
                state = State.dead;
                missionFailureSounds.Play();
                if (enemy.gameObject.GetComponent<FrogBoss>())
                {
                    originallSpeed = enemy.gameObject.GetComponent<FrogBoss>().MovementSpeed;
                    enemy.gameObject.GetComponent<FrogBoss>().OriginalSpeed = originallSpeed;
                    enemy.gameObject.GetComponent<FrogBoss>().MovementSpeed = 0f;
                }
                else
                {
                    originallSpeed = enemy.gameObject.GetComponent<AI>().MovementSpeed;
                    enemy.gameObject.GetComponent<AI>().OriginalSpeed = originallSpeed;
                    enemy.gameObject.GetComponent<AI>().MovementSpeed = 0f;
                }                       
                lowHpSound.Stop();
                deathSound.Play();
                playerLifes--;
                Invoke("BlaBlaBla", 2.2f);            
                if (playerLifes <= 0)
                {              
                    SceneManager.LoadScene(4);
                }
            }

            if(state != State.dead)
                state = State.hurt;

            if (enemy.gameObject.GetComponent<FrogBoss>())
            {
                if (enemy.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-bossHurtForce, bossHurtForce);
                }
                else
                {
                    rb.velocity = new Vector2(bossHurtForce, bossHurtForce);
                }
            }
            else
            {
                if (enemy.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-hurtForce, hurtForce + 1f);
                }
                else
                {
                    rb.velocity = new Vector2(hurtForce, hurtForce + 1f);
                }
            }
        }
    }

    private void BlaBlaBla()
    {
        rb.position = checkPointPos;
        state = State.idle;
        playerHealth = 100;
        slider.value = playerHealth;
    }

    private void GetBoxColliderBack()
    {
        player.GetComponent<BoxCollider2D>().enabled = true;
        player.GetComponent<PlayerController>().enabled = true;
        rb.position = checkPointPos;
    }

    private void BlaBlaBla2()
    {
        rb.position = checkPointPos;
        frogBoss.GetComponent<FrogBoss>().StartFight = true;
        state = State.idle;
        playerHealth = 100;
        slider.value = playerHealth;
    }

    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");
        /*float vDirection = Input.GetAxis("Vertical");*/

        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            if (FacingRight != false)
                Flip();
            
        }
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            if (FacingRight != true)
                Flip();
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && /*coll.IsTouchingLayers(ground)*/ isGrounded() && state != State.hurt && state != State.dead)
        {
            walkingSound.Stop();
            jump1Sound.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
           state = State.jumping;
        }

        if (Input.GetKey(KeyCode.W) && jetpackFuel != 0 && state != State.hurt && state != State.dead )
        {
            if (jetpackFuel == 500)
            {
                jumpOnTheGroundSound.Play();
            }
            walkingSound.Stop();
            state = State.fly;
            rb.velocity = new Vector2(rb.velocity.x, flyForce);
            jetpackFuel--;
            fuelText.text = jetpackFuel/20 + "%";
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.5f, ground);
        return raycastHit.collider != null;
    }

    private void Flip()
    {
        FacingRight = !FacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void Walking() 
    {
        walkingSound.Play();    
    }

    private void VelocityState()
    {
        if (state == State.jumping)
        {
            if (rb.velocity.y < 0.001f)
            {
                state = State.falling;
                
            }
            walkingSound.Stop();
        }
        else if (state == State.fly)
        {
            if (rb.velocity.y < 0.001f)
            {
                state = State.falling;
                walkingSound.Stop();
            }
        }
        else if (state == State.falling)
        {
            if (isGrounded())
            {
                state = State.idle;
                jumpOnTheGroundSound.Play();
            }
        }
        else if (state == State.hurt)
        {
            walkingSound.Stop();
            if (playerHealth > 0)
                Invoke("SwitchToIdle", 0.933f);
            
            /*if (Mathf.Abs(rb.velocity.x) < 0.001f)
            {
                state = State.idle;
            }*/
        }
        else if (state == State.dead)
        {
            walkingSound.Stop();
            if (playerHealth > 0)
                Invoke("SwitchToIdle", 2.2f);
        }
        else if (Mathf.Abs(rb.velocity.x) > 0.0001f && !(Input.GetKey(KeyCode.W) && jetpackFuel != 0) && state != State.dead && state != State.hurt)
        {
            state = State.running;
        }
        else if (Input.GetKey(KeyCode.W) && jetpackFuel != 0 && state != State.dead && state != State.hurt)
        {
            state = State.fly;
            walkingSound.Stop();
        }
        else if(state == State.idle)
        {
            if(playerHealth <= 0)
            {
                state = State.dead;
            }
        }
        else
        {
            if(playerHealth>0)
            {
                state = State.idle;
                walkingSound.Stop();
            }
        }
    }
    private void SwitchToIdle()
    {
        state = State.idle;
    }
}
