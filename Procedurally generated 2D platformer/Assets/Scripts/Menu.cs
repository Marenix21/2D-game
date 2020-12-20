﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Play() {
        Debug.Log(Application.persistentDataPath);
        SceneManager.LoadScene("Game");
    }
    public void Quit() {
        Application.Quit();
    }
}
