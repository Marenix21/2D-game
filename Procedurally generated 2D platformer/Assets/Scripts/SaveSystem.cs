using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveSystem
{

    private static string path = Application.persistentDataPath + "/player.data";
    public static void SaveScore(Character player) {
        BinaryFormatter formatter = new BinaryFormatter();
        Data data = new Data(player);
        using (var stream = File.Open(path, FileMode.OpenOrCreate)) {
            formatter.Serialize(stream, data);
        }
    }

    public static Data LoadScore() {
        if(File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            Data data;
            using (var stream = File.Open(path, FileMode.Open)) {
                data = formatter.Deserialize(stream) as Data;
            }
            return data;
        } else {
            return null;
        }
    }

    public static void endGame(Character player) {
        SaveSystem.SaveScore(player);
        SceneManager.LoadScene("Start");
    }
}
