using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        GameManager.Instance.executeOnLoad = Random.Range(0, 2);
        Debug.Log(GameManager.Instance.executeOnLoad);
        SceneManager.LoadScene("MainScene");
    }

    public void QuitGame()
    {
        Debug.Log("quit game");
        Application.Quit();
    }


}
