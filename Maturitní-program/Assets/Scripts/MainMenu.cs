using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync (1);
        PlayerPrefs.DeleteKey("SavedTime");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
