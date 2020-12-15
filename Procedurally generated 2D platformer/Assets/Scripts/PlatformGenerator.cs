using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    // Start is called before the first frame update
    void Start()
    {   
        lastEndPosition = new Vector3(-3, 0, 0);
        SpawnPlatform();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(playerCharacter.transform.position, lastEndPosition) < distanceFromEnd) {
            SpawnPlatform();
        }
        foreach (GameObject tmp in UnityEngine.Object.FindObjectsOfType<GameObject>()) 
        {
            if (Vector3.Distance(playerCharacter.transform.position, tmp.transform.position) > 50)
            {
                //Debug.Log(tmp.GetComponent<Renderer>().bounds.size);
                DestroyImmediate(tmp, true);
            }
        }
    }

    void SpawnPlatform() {
        float x = UnityEngine.Random.Range(-1.0f, 2.0f), y = UnityEngine.Random.Range(-3.0f, 2.0f);
        int i = UnityEngine.Random.Range(0, _platforms.Length);
        Transform platform = Instantiate(_platforms[i].transform, lastEndPosition + new Vector3(x, y, 0), Quaternion.identity);
        lastEndPosition = platform.Find("EndPosition").position;
        if(UnityEngine.Random.Range(0.0f, 1.0f) * 100 < probabilityToSpawn)
        {
            i = UnityEngine.Random.Range(0, _enemies.Length);
            x = UnityEngine.Random.Range(1.0f, platform.Find("Sprite").GetComponent<Renderer>().bounds.size[0] - 1.0f);
            Transform enemy = Instantiate(_enemies[i].transform, lastEndPosition - new Vector3(x, 0, 0), Quaternion.identity);
        }
        Debug.Log(platform.Find("Sprite").GetComponent<Renderer>().bounds.size[0]);
    }
}
