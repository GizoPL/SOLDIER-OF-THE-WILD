using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    private bool keyIsActive;
    private string typeOfGun = "machineGun";
    private int ammunation = 25;
    private float speed = 1000f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Text ammoText;
    [SerializeField] private Rigidbody2D bullet;
    [SerializeField] private GameObject multiMode;
    [SerializeField] AudioSource shotGun, machineGun,toShotgun, toMachineGun;
    
    public int Ammo { get { return ammunation; } set { ammunation = value; } }
    public Text AmmoText { get { return ammoText; } set { ammoText = value; } }
    private void Start()
    {
        multiMode.SetActive(false);
        keyIsActive = true;
    }

    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().PlayerState == "dead" || GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().PlayerState == "hurt")
        {
            keyIsActive = false;
        }
        else
        {
            keyIsActive = true;
        }

        if (PauseMenu.GamePaused == false)
        {
            if (ammunation < 3)
            {
                typeOfGun = "machineGun";
                multiMode.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                if (typeOfGun == "machineGun" && ammunation >= 3)
                {
                    toShotgun.Play();
                    typeOfGun = "shotGun";
                    multiMode.SetActive(true);
                }
                else
                {
                    toMachineGun.Play();
                    typeOfGun = "machineGun";
                    multiMode.SetActive(false);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && ammunation != 0 && keyIsActive == true)
            {

                if (typeOfGun == "machineGun")
                {
                    machineGun.Play();
                    var spawnedBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
                    spawnedBullet.AddForce(firePoint.right * speed);
                    ammunation--;
                    ammoText.text = "*" + ammunation.ToString();
                }
                else
                {

                    for (int i = 0; i < 3; i++)
                    {
                        shotGun.Play();
                        var spawnedBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
                        switch (i)
                        {
                            case 0:
                                spawnedBullet.AddForce(firePoint.right * speed + new Vector3(0f, -40f, 0f));
                                break;
                            case 1:
                                spawnedBullet.AddForce(firePoint.right * speed + new Vector3(0f, 0f, 0f));
                                break;
                            case 2:
                                spawnedBullet.AddForce(firePoint.right * speed + new Vector3(0f, 40f, 0f));
                                break;
                        }
                    }
                    ammunation = ammunation - 3;
                    ammoText.text = "*" + ammunation.ToString();
                }
            }
        } 
    }
}
