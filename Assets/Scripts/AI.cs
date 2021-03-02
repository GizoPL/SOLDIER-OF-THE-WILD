using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField] private float leftBorder;
    [SerializeField] private float rightBorder;
    [SerializeField] private float movementSpeed;
    [SerializeField] public int hP;
    private float originalSpeed;
    private Rigidbody2D rb;
    private bool facingLeft = true;

    public int HP
    {
        get { return hP; }
        set { hP  = value; }
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
       
        if(movementSpeed == 0f)
        {
            Invoke("BlaBlaBla", 3f);       
        }

        if (facingLeft)
        {
            if (transform.position.x > leftBorder)
            {
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }

                rb.velocity = new Vector2(-movementSpeed, 0f);
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
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }

                rb.velocity = new Vector2(movementSpeed, 0f);
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
        
    }
}
