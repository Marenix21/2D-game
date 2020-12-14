using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{

    private static string path = Application.persistentDataPath + "/player.data";
    public static void SaveScore(Character player) {
        BinaryFormatter formatter = new BinaryFormatter();
        Data data = new Data(player);
        using (var stream = File.Open(path, FileMode.OpenOrCreate)) {
            Debug.Log("Saving HS " + data.highscore.ToString());
            Debug.Log("Saving S " + data.last_score.ToString());
            formatter.Serialize(stream, data);
        }
    }

    public static Data LoadScore() {
        if(File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            Data data;
            using (var stream = File.Open(path, FileMode.Open)) {
                data = formatter.Deserialize(stream) as Data;
                Debug.Log("Loaded HS " + data.highscore.ToString());
                Debug.Log("Loaded S " + data.last_score.ToString());
            }
            return data;
        } else {
            return null;
        }
    }
}
