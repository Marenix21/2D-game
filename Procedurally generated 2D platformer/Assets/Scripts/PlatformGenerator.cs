using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField, Tooltip("Prefabs for randomly generated platforms.")]
    private GameObject[] _platforms;

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
    }

    void SpawnPlatform() {
        int x = UnityEngine.Random.Range(-1, 2), y = UnityEngine.Random.Range(-3, 2);
        int i = UnityEngine.Random.Range(0, _platforms.Length);
        Transform platform = Instantiate(_platforms[i].transform, lastEndPosition + new Vector3(x, y, 0), Quaternion.identity);
        lastEndPosition = platform.Find("EndPosition").position;
    }
}
