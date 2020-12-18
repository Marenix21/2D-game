﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField, Tooltip("Prefabs for randomly generated platforms.")]
    private GameObject[] _platforms;

    [SerializeField, Tooltip("Probability of enemy spawning on a platform.")]
    private int probabilityToSpawn;

    [SerializeField, Tooltip("Prefabs for randomly generated enemies.")]
    private GameObject[] _enemies;

    [SerializeField, Tooltip("The character prefab for player position.")]
    private GameObject playerCharacter;
    
    private Vector3 lastEndPosition;
    private float distanceFromEnd = 25f;
    private float distanceToDelete = 50f;
    private float distanceToDie = 50f;

    // Start is called before the first frame update
    void Start()
    {   
        lastEndPosition = new Vector3(0, 0, 0);
        SpawnFirst();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(playerCharacter.transform.position, lastEndPosition) < distanceFromEnd) {
            SpawnPlatform();
        }
        foreach (GameObject tmp in UnityEngine.Object.FindObjectsOfType<GameObject>()) 
        {
            if (tmp.transform.position[1] - playerCharacter.transform.position[1] > distanceToDie)
            {
                if (tmp.GetComponent<RectTransform>() == null)
                {
                    endGame();
                }
            }
            if (playerCharacter.transform.position[0] - tmp.transform.position[0] > distanceToDelete)
            {
                DestroyImmediate(tmp, true);
            }
        }
    }

    void SpawnPlatform() {
        float x = UnityEngine.Random.Range(1.0f, 4.0f), y = UnityEngine.Random.Range(-3.0f, 2.0f);
        int i = UnityEngine.Random.Range(0, _platforms.Length);
        Transform platform = Instantiate(_platforms[i].transform, lastEndPosition + new Vector3(x, y, 0), Quaternion.identity);
        lastEndPosition = platform.Find("EndPosition").position;
        if(UnityEngine.Random.Range(0.0f, 1.0f) * 100 < probabilityToSpawn)
        {
            i = UnityEngine.Random.Range(0, _enemies.Length);
            float enemySize = _enemies[i].transform.GetComponent<Renderer>().bounds.size[0];
            float platformSize = platform.Find("Sprite").GetComponent<BoxCollider2D>().bounds.size[0];
            if (platformSize > 3 * enemySize)
            {
                x = UnityEngine.Random.Range(enemySize, platformSize -  2 * enemySize);
                Transform enemy = Instantiate(_enemies[i].transform, lastEndPosition - new Vector3(x, 0, -0.5f), Quaternion.identity);
            }
        }
    }

    void SpawnFirst()
    {
        int i = UnityEngine.Random.Range(0, _platforms.Length);
        Transform platform = Instantiate(_platforms[i].transform, lastEndPosition + new Vector3(-1, 0, 0), Quaternion.identity);
        lastEndPosition = platform.Find("EndPosition").position;
    }
    
    private void endGame()
    {
        SaveSystem.SaveScore(playerCharacter.GetComponent<Character>());
        SceneManager.LoadScene("PlayAgain");
    }
}
