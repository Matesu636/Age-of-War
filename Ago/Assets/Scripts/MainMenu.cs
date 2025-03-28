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
        PlayerPrefs.DeleteKey("BasesDestroyed");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
