using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject deathMenu;
    public GameObject winScreen;
    public static bool gameIsPaused = false;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    //toggle the state of the pause menu
    public void TogglePauseMenu()
    {
        if (gameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    // player died - pause the game and show menu
    public void ShowDeathScreen()
    {
        deathMenu.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;

    }

    // player won - pause the game and show menu
    public void ShowWinScreen()
    {
        winScreen.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;

    }

    //return time to normal and hide UI
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    //pause game time and show UI
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    //return to title screen
    public void GoToMenu()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        SceneManager.LoadScene("Title");
    }

    //reload the same scene
    public void Retry()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //load the enxt scene in the build
    public void NextScene()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator LoadNextScene()
    {
        TogglePauseMenu();
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
    
    //play a sound when the button is clicked
    public void ButtonSound()
    {
        StartCoroutine(WaitForSound());
    }
    //wait some time for the sound to play out
    IEnumerator WaitForSound()
    {
        AudioManager.instance.Play("Click");
        yield return new WaitForSeconds(1.1f);
    }
}
