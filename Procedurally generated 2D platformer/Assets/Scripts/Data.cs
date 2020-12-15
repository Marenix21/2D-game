using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data
{
    public int highscore;
    public int last_score;
    public Data(Character player) {
        Data data = SaveSystem.LoadScore();
        if(data is null) {
            highscore = player.score;
            last_score = player.score;
        } else {
            Debug.Log("Loaded a score! " + data.highscore.ToString());
            Debug.Log("Player got " + player.score.ToString());
            if(player.score > data.highscore) highscore = player.score;
            else highscore = data.highscore;
            last_score = player.score;
        }
    }
}
