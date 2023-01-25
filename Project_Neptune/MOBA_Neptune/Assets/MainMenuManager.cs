using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainPart;
    public GameObject tutorialPart;
    
    public void GoToScene(int sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }

    public void TutorialButton()
    {
        tutorialPart.SetActive(true);
    }

    public void ExitToMenu()
    {
        tutorialPart.SetActive(false);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
