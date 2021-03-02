using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void PlayAgain()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("LastPlayedLevel"));
    }
    
    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
