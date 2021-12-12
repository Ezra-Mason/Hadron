using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject menuScreen;
    public TitleCameraShake shaker;

    //move to the menu scene
    public void ToMenu()
    {
        titleScreen.SetActive(false);
        menuScreen.SetActive(true);
    }

    //load the next scene after a delay
    public void LoadNextScene()
    {
        StartCoroutine(DelayedLoadNextScene());
    }

    //delay the loading of the scene
    public IEnumerator DelayedLoadNextScene()
    {
        AudioManager.instance.Play("Echoes");
        shaker.Shake();
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
