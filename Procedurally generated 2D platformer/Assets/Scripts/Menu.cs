using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void StartScreen() {
        SceneManager.LoadScene("Start");
    }

    public void PlayFromStart() {
        if(SaveSystem.LoadScore() is null) {
            SceneManager.LoadScene("Tutorial");
        } else {
            SceneManager.LoadScene("Game");
        }
    }

    public void Tutorial() {
        SceneManager.LoadScene("Tutorial");
    }

    public void PlayFromTutorial() {
        SceneManager.LoadScene("Game");
    }

    public void Quit() {
        Application.Quit();
    }
}
