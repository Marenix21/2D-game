using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField, Tooltip("Prefabs for randomly generated platforms.")]
    private GameObject[] _platforms;

    [SerializeField, Tooltip("Probability of enemy spawning on a platform [0-100].")]
    private int probabilityToSpawn;

    [SerializeField, Tooltip("Prefabs for randomly generated enemies.")]
    private GameObject[] _enemies;

    [SerializeField, Tooltip("The character prefab for player position.")]
    private GameObject playerCharacter;

    [SerializeField, Tooltip("Background prefab.")]
    private GameObject background;

    [SerializeField, Tooltip("Probability of spawning a power-up on a platform [0-100].")]
    private int powerUpProbability;

    [SerializeField, Tooltip("Prefabs for randomly generated power-ups.")]
    private GameObject[] _powerUps;


    
    private Vector3 lastEndPosition;
    private Vector3 lastEndPositionBackground;
    private float distanceFromEnd = 25f;
    private float distanceToDelete = 50f;
    private float distanceToDie = 50f;
    private float backgroundX = 49f;
    // Start is called before the first frame update
    void Start()
    {   
        lastEndPosition = new Vector3(0, 0, 0);
        lastEndPositionBackground = new Vector3(0, 0, 10);
        SpawnFirst();
        SpawnFirstBackground();

    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(playerCharacter.transform.position, lastEndPosition) < distanceFromEnd) {
            SpawnPlatform();
        }
        if(lastEndPositionBackground[0] - playerCharacter.transform.position[0] < distanceToDelete)
        {
            SpawnBackground();
        }
        foreach (GameObject tmp in UnityEngine.Object.FindObjectsOfType<GameObject>()) 
        {
            if (tmp.transform.position[1] - playerCharacter.transform.position[1] > distanceToDie)
            {
                if (tmp.GetComponent<RectTransform>() == null)
                {
                    SaveSystem.endGame(playerCharacter.GetComponent<Character>());
                }
            }
            if (playerCharacter.transform.position[0] - tmp.transform.position[0] > distanceToDelete)
            {
                if(tmp.tag == "DoNotDelete" || tmp.name == "Hills" || tmp.name == "Background" || tmp.name == "Background(Clone)" && playerCharacter.transform.position[0] - tmp.transform.position[0] < distanceToDelete * 2)
                {
                    continue;
                }
                DestroyImmediate(tmp, true);
            }
        }
    }

    void SpawnPlatform() {
        float x = UnityEngine.Random.Range(1.0f, 4.0f), y = UnityEngine.Random.Range(-3.0f, 2.0f);
        int i = UnityEngine.Random.Range(0, _platforms.Length);
        Transform platform = Instantiate(_platforms[i].transform, lastEndPosition + new Vector3(x, y, 0), Quaternion.identity);
        lastEndPosition = platform.Find("EndPosition").position;
        bool enemySpawned = false;

        if(UnityEngine.Random.Range(0.0f, 1.0f) * 100 < probabilityToSpawn)
        {
            i = UnityEngine.Random.Range(0, _enemies.Length);
            float enemySize = _enemies[i].transform.GetComponent<Renderer>().bounds.size[0];
            float platformSize = platform.Find("Sprite").GetComponent<BoxCollider2D>().bounds.size[0];
            if (platformSize > 3 * enemySize)
            {
                enemySpawned = true;
                x = UnityEngine.Random.Range(enemySize, platformSize -  2 * enemySize);
                Transform enemy = Instantiate(_enemies[i].transform, lastEndPosition - new Vector3(x, 0, -0.5f), Quaternion.identity);
            }
        }

        if(UnityEngine.Random.Range(0.0f, 1.0f) * 100 < powerUpProbability)
        {
            float platformSize = platform.Find("Sprite").GetComponent<BoxCollider2D>().bounds.size[0];
            i = UnityEngine.Random.Range(0, _powerUps.Length);
            x = UnityEngine.Random.Range(0, platformSize);
            if(enemySpawned)
            {
                y = UnityEngine.Random.Range(2.5f, 3.5f);
            }
            else
            {
                y = UnityEngine.Random.Range(0.5f, 3.5f);
            }
            Transform powerUp = Instantiate(_powerUps[i].transform, lastEndPosition - new Vector3(x, -y, 0), Quaternion.identity);
        }
    }

    void SpawnBackground()
    {
        Transform backgroundTmp = Instantiate(background.transform, lastEndPositionBackground + new Vector3(backgroundX, 0, 0), Quaternion.identity);
        lastEndPositionBackground = backgroundTmp.Find("EndPosition").position;
        Debug.Log("Spawning at: " + lastEndPositionBackground);
    }

    void SpawnFirstBackground()
    {
        Transform backgroundTmp = Instantiate(background.transform, lastEndPositionBackground + new Vector3(0, 0, 0), Quaternion.identity);
        lastEndPositionBackground = backgroundTmp.Find("EndPosition").position;
    }

    void SpawnFirst()
    {
        int i = UnityEngine.Random.Range(0, _platforms.Length);
        Transform platform = Instantiate(_platforms[i].transform, lastEndPosition + new Vector3(-1, 0, 0), Quaternion.identity);
        lastEndPosition = platform.Find("EndPosition").position;
    }
}
