using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScore : MonoBehaviour
{
    
    private TMP_Text text;
    private int highscore;
    private int last_score;
    void Start()
    {
        Data data = SaveSystem.LoadScore();
        if(data is null) {
            highscore = 0;
            last_score = 0;
        } else {
            highscore = data.highscore;
            last_score = data.last_score;
        }
        text = GetComponent<TMP_Text>();
        text.text = "Highscore: " + highscore.ToString()+ "\nLast score: " + last_score.ToString();
    }
}
