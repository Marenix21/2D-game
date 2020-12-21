using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameScore : MonoBehaviour
{
    [SerializeField, Tooltip("The character prefab for player position.")]
    private GameObject playerCharacter;

    private TMP_Text text;
    
    void Start()
    {
        text = GetComponent<TMP_Text>();
        text.text = "0";
    }
    
    void Update()
    {
        text = GetComponent<TMP_Text>();
        text.text = playerCharacter.GetComponent<Character>().score.ToString();
    }
}
