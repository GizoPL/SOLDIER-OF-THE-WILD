using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioSource goodByeSound;
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        goodByeSound.Play();
        Invoke("Quit",1f);
        
    }

    private void Quit()
    {
        Application.Quit();
    }

}
