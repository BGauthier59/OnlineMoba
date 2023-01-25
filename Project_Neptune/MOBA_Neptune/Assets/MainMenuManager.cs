using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainPart;
    public GameObject tutorialPart;
    
    public void NewGameButton()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void TutorialButton()
    {
        
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
